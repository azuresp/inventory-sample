using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Valent.Inv.Domain;
using Valent.Inv.Models;
using Item = Valent.Inv.Models.Item;
using Swashbuckle.Swagger.Annotations;

namespace Valent.Inv.Controllers
{
    [RoutePrefix("inventory")]
    public class InventoryController : ApiController
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly INotifyRepository _notifyRepository;

        public InventoryController(IInventoryRepository inventoryRepository, INotifyRepository notifyRepository)
        {
            _inventoryRepository = inventoryRepository;
            _notifyRepository = notifyRepository;
        }

        [HttpGet, Route("items/{label}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.Item))]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        public IHttpActionResult GetByLabel(string label)
        {
            var domainItem = _inventoryRepository.Get(label.ToLowerInvariant());
            if(domainItem == null)
                return NotFound();
            return Ok(MapFromDomain(domainItem));
        }

        [HttpPut, Route("items/{label}")]
        [SwaggerResponse(HttpStatusCode.Created, Type=typeof(Models.Item))]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(Models.Item))]
        public IHttpActionResult PutByLabel(string label, ItemPutRequest request)
        {
            label = label.ToLowerInvariant();
            bool creating = false;
            var domainItem = _inventoryRepository.Get(label);
            if (domainItem == null)
            {
                creating = true;
                domainItem = new Domain.Item
                {
                    Label = label
                };
            }

            domainItem.Expires = request.Expires.Value.ToUniversalTime(); //model validation guards against null
            domainItem.Type = request.Type;

            _inventoryRepository.Set(domainItem);
            //returning the object may be a bit over the top, but shown for completeness.
            if(creating)
                return Created<Models.Item>(this.Request.RequestUri, MapFromDomain(domainItem));

            return Ok<Models.Item>(MapFromDomain(domainItem));
        }

        [HttpDelete, Route("items/{label}")]
        [SwaggerResponse(HttpStatusCode.NoContent)]
        public IHttpActionResult DeleteByLabel(string label)
        {
            label = label.ToLowerInvariant();
            var deleted = _inventoryRepository.DeleteIfExists(label);
            if(deleted) _notifyRepository.Notify($"Item '{label}' has been deleted.");
            return StatusCode(HttpStatusCode.NoContent);
        }

        [HttpPut, Route("clear-expired-items")]
        [SwaggerResponse(HttpStatusCode.OK, Type=typeof(Models.Item[]))]
        public IHttpActionResult DeleteExpired()
        {
            var expiredItems = _inventoryRepository.GetExpired();
            foreach (var item in expiredItems)
            {
                _inventoryRepository.DeleteIfExists(item.Label);
                _notifyRepository.Notify($"Item '{item.Label}' has been deleted because it was expired.");
            }

            var result = expiredItems.Select(MapFromDomain).ToArray();

            return Ok(result);
        }

        //This is the unfortunate cost of abstracting the contract from the domain.  Still worth it IMO.
        private Models.Item MapFromDomain(Domain.Item domainItem)
        {
            return new Item
            {
                Expires = domainItem.Expires,
                Label = domainItem.Label,
                Type = domainItem.Type
            };
        }
    }
}
