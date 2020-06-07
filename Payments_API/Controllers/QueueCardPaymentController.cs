using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments_API.RabbitMQ;

namespace Payments_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueCardPaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult MakePayment([FromBody] CardPayment payment)
        {
            RabbitMQClient client = new RabbitMQClient();
            client.SendPayment(payment);
            client.Close();

            return Ok(payment);
        }
    }
}