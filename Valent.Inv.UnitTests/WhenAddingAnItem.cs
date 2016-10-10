using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Valent.Inv.Controllers;
using Valent.Inv.Domain;
using Valent.Inv.Models;
using FluentAssertions;

namespace Valent.Inv.UnitTests
{
    [TestFixture]
    public class WhenAddingAnItem
    {
        DateTime expires = new DateTime(2014, 3, 2, 1, 0, 0);
        Mock<INotifyRepository> notifyMock = new Mock<INotifyRepository>();
        Mock<IInventoryRepository> inventoryMock = new Mock<IInventoryRepository>();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            var controller = new InventoryController(inventoryMock.Object, notifyMock.Object);
            controller.Request = new HttpRequestMessage(HttpMethod.Get, "http://test"); //not relevant to test, but need to set this up.
            result = controller.PutByLabel("thing", new ItemPutRequest {Expires = expires, Type = "thingtype"});
        }

        [Test]
        public void CreatedStatusCodeIsReturned()
        {
            result.Should().BeOfType<CreatedNegotiatedContentResult<Models.Item>>();
        }

        [Test]
        public void ItemStateIsReturned()
        {
            var resultItem = ((CreatedNegotiatedContentResult<Models.Item>) result).Content;
            resultItem.Expires.Should().Be(expires.ToUniversalTime());
            resultItem.Label.Should().Be("thing");
            resultItem.Type.Should().Be("thingtype");
        }

        [Test]
        public void ItemIsAdded()
        {
            inventoryMock.Verify(i => i.Set(It.Is<Domain.Item>(item => 
                item.Expires == expires.ToUniversalTime()
                && item.Label == "thing"
                && item.Type == "thingtype"
            )));
        }
    }
}
