using System;
using System.Collections.Generic;
using System.Linq;
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
using Valent.Inv.UnitTests.Fakes;

namespace Valent.Inv.UnitTests
{
    [TestFixture]
    public class WhenRemovingExpiredItems
    {
        DateTime expires = new DateTime(2014, 3, 2, 1, 0, 0);
        FakeNotifyRepository notifyRepository = new FakeNotifyRepository();
        Mock<IInventoryRepository> inventoryMock = new Mock<IInventoryRepository>();
        private IHttpActionResult result;

        [OneTimeSetUp]
        public void ArrangeAndAct()
        {
            var items = new List<Domain.Item>
            {
                new Domain.Item
                {
                    Label = "expired1",
                    Expires = new DateTime(1950, 1, 1),
                    Type = "z"
                },
                new Domain.Item
                {
                    Label = "expired2",
                    Expires = new DateTime(1951, 1, 1),
                    Type = "q"
                }
            };

            inventoryMock.Setup(i => i.GetExpired()).Returns(items);

            var controller = new InventoryController(inventoryMock.Object, notifyRepository);

            //act
            result = controller.DeleteExpired();
        }

        [Test]
        public void OkStatusCodeIsReturned()
        {
            result.Should().BeOfType<OkNegotiatedContentResult<Models.Item[]>>();
        }

        [Test]
        public void ExpiredItemsAreReturned()
        {
            var content = ((OkNegotiatedContentResult<Models.Item[]>) result).Content;
            content.Length.Should().Be(2);
            content.Any(c => c.Label == "expired1").Should().BeTrue();
            var checkItem = content.Single(c => c.Label == "expired2");
            checkItem.Label.Should().Be("expired2");
            checkItem.Expires.Year.Should().Be(1951);
            checkItem.Type.Should().Be("q");
        }

        [Test]
        public void ExpiredItemsAreRemoved()
        {
            inventoryMock.Verify(i => i.DeleteIfExists("expired1"));
            inventoryMock.Verify(i => i.DeleteIfExists("expired2"));
        }

        [Test]
        public void NotifcationsAreGenerated()
        {
            notifyRepository.Messages.Count.Should().Be(2);
            notifyRepository.Messages.First().Should().Be($"Item 'expired1' has been deleted because it was expired.");
        }
    }
}
