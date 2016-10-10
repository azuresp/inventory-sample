using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Valent.Inv.Controllers;
using Valent.Inv.Domain;

namespace Valent.Inv.UnitTests
{
    [TestFixture]
    public class WhenGettingAnItem
    {
        Mock<INotifyRepository> notifyRepositoryMock = new Mock<INotifyRepository>();
        Mock<IInventoryRepository> inventoryRepositoryMock = new Mock<IInventoryRepository>();
        DateTime expires = new DateTime(2011,5,6).ToUniversalTime();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            inventoryRepositoryMock.Setup(i => i.Get("zzz")).Returns(new Item()
            {
                Label = "zzz",
                Expires = expires,
                Type = "ztype"
            });

            var controller = new InventoryController(inventoryRepositoryMock.Object, notifyRepositoryMock.Object);
            result = controller.GetByLabel("ZZZ");
        }

        [Test]
        public void OkStatusCodeIsReturned()
        {
            result.Should().BeOfType<OkNegotiatedContentResult<Models.Item>>();
        }

        [Test]
        public void TheItemIsCorrect()
        {
            var resultItem = ((OkNegotiatedContentResult<Models.Item>) result).Content;
            resultItem.Label.Should().Be("zzz");
            resultItem.Expires.Should().Be(expires);
            resultItem.Type.Should().Be("ztype");
        }
    }

    [TestFixture]
    public class WhenGettingAMissingItem
    {
        Mock<INotifyRepository> notifyRepositoryMock = new Mock<INotifyRepository>();
        Mock<IInventoryRepository> inventoryRepositoryMock = new Mock<IInventoryRepository>();
        DateTime expires = new DateTime(2011, 5, 6).ToUniversalTime();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            var controller = new InventoryController(inventoryRepositoryMock.Object, notifyRepositoryMock.Object);
            result = controller.GetByLabel("ZZZ");
        }

        [Test]
        public void A404IsReturned()
        {
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}
