using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Valent.Inv.Controllers;
using Valent.Inv.Domain;
using Valent.Inv.Models;
using Valent.Inv.UnitTests.Fakes;

namespace Valent.Inv.UnitTests
{
    [TestFixture]
    public class WhenDeletingAnItem
    {
        FakeNotifyRepository notifyRepository = new FakeNotifyRepository();
        Mock<IInventoryRepository> inventoryMock = new Mock<IInventoryRepository>();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            inventoryMock.Setup(i => i.DeleteIfExists("thing1")).Returns(true);
            var controller = new InventoryController(inventoryMock.Object, notifyRepository);

            //act
            result = controller.DeleteByLabel("THING1");
        }

        [Test]
        public void ItemIsDeleted()
        {
            inventoryMock.Verify(i => i.DeleteIfExists("thing1"));
        }

        [Test]
        public void NoContentStatusCodeIsReturned()
        {
            ((StatusCodeResult) result).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public void NotificationOccurs()
        {
            notifyRepository.Messages.Count.Should().Be(1);
            notifyRepository.Messages.Single().Should().Be("Item 'thing1' has been deleted.");
        }
    }

    [TestFixture]
    public class WhenDeletingMissingItem
    {
        FakeNotifyRepository notifyRepository = new FakeNotifyRepository();
        Mock<IInventoryRepository> inventoryMock = new Mock<IInventoryRepository>();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            inventoryMock.Setup(i => i.DeleteIfExists("thing1")).Returns(false);

            var controller = new InventoryController(inventoryMock.Object, notifyRepository);

            //act
            result = controller.DeleteByLabel("THING1");
        }

        [Test]
        public void NoContentStatusCodeIsReturned()
        {
            ((StatusCodeResult)result).StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Test]
        public void NoNotificationOccurs()
        {
            notifyRepository.Messages.Count.Should().Be(0);
        }
    }
}
