using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using Valent.Inv.Controllers;
using Valent.Inv.Domain;
using Valent.Inv.Models;

namespace Valent.Inv.UnitTests
{
    [TestFixture]
    public class WhenUpdatingAnItem
    {
        DateTime expires1 = new DateTime(2011, 3, 2, 1, 0, 0);
        DateTime expires2 = new DateTime(2014, 3, 2, 1, 0, 0);
        Mock<INotifyRepository> notifyMock = new Mock<INotifyRepository>();
        Mock<IInventoryRepository> inventoryMock = new Mock<IInventoryRepository>();
        private IHttpActionResult updateResult;
        private InventoryController controller;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            inventoryMock.Setup(i => i.Get("thing")).Returns(new Domain.Item
            {
                Label = "thing",
                Expires = expires1.ToUniversalTime(),
                Type = "oldType"
            });

            controller = new InventoryController(inventoryMock.Object, notifyMock.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://test")
            };
            //not relevant to test, but need to set this up.
            updateResult = controller.PutByLabel("THING", new ItemPutRequest { Expires = expires2, Type = "newType" });
        }

        [Test]
        public void OkStatusCodeIsReturned()
        {
            updateResult.Should().BeOfType<OkNegotiatedContentResult<Models.Item>>();
        }

        [Test]
        public void NewItemStateIsReturned()
        {
            var resultItem = ((OkNegotiatedContentResult<Models.Item>)updateResult).Content;

            resultItem.Expires.Should().Be(expires2.ToUniversalTime());
            resultItem.Label.Should().Be("thing");
            resultItem.Type.Should().Be("newType");
        }

        [Test]
        public void ItemIsUpdated()
        {
            inventoryMock.Verify(i => i.Set(It.Is<Domain.Item>(item =>
                item.Expires == expires2.ToUniversalTime()
                && item.Label == "thing"
                && item.Type == "newType"
            )));
        }
    }
}
