using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments_API.RabbitMQ;

namespace Payments_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueuePurchaseOrderController : ControllerBase
    {
        [HttpPost]
        public IActionResult MakePayment([FromBody] PurchaseOrder payment)
        {
            RabbitMQClient client = new RabbitMQClient();
            client.SendPurchaseOrder(payment);
            client.Close();
            return Ok(payment);
        }
    }
}