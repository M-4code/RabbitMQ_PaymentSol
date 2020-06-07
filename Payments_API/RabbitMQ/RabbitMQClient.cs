using Payments_API.Controllers;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payments_API.RabbitMQ
{
    public class RabbitMQClient
    {
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _model;

        private const string ExchangeName = "Topic_Exchange";
        private const string CardPaymentQueueName = "CardPaymentTopic_Queue";
        private const string PurchaseOrderQueueName = "PurchaseOrderTopic_Queue";
        private const string AllQueueName = "AllTopic_Queue";

        public RabbitMQClient()
        {
            CreateConnection();
        }

        public void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _connection = _factory.CreateConnection();
            _model = _connection.CreateModel();
            _model.ExchangeDeclare(ExchangeName, "topic");

            _model.QueueDeclare(CardPaymentQueueName, true, false, false, null);
            _model.QueueDeclare(PurchaseOrderQueueName, true, false, false, null);
            _model.QueueDeclare(AllQueueName, true, false, false, null);

            _model.QueueBind(CardPaymentQueueName, ExchangeName, "payment.card");
            _model.QueueBind(PurchaseOrderQueueName, ExchangeName, "payment.purchaseorder");
            _model.QueueBind(AllQueueName, ExchangeName, "payment.*");
        }

        public void Close()
        {
            _connection.Close();
        }

        public void SendPayment(CardPayment cardPayment)
        {
            SendMessage(cardPayment.Serialize(), "payment.card");
            Console.WriteLine("Payment Sent {0}, $ {1}", cardPayment.CardNumber, cardPayment.Amount);
        }

        public void SendPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            SendMessage(purchaseOrder.Serialize(), "payment.purchaseorder");
            Console.WriteLine("Purchase order Sent {0}, $ {1}, {2}  {3}", purchaseOrder.PONumber, purchaseOrder.AmountToPay, purchaseOrder.CompanyName, purchaseOrder.PaymentDayTerms);
        }

        public void SendMessage(byte[] message, string routingKey)
        {
            _model.BasicPublish(ExchangeName, routingKey, null, message);
        }
    }
}
