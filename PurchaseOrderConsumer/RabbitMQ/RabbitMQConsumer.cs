using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PaymentsCardConsumer.RabbitMQ
{
    public class RabbitMQConsumer
    {
        private ConnectionFactory _factory;
        private IConnection _connection;

        private const string ExchangeName = "Topic_Exchange";
        private const string CardPaymentQueueName = "PurchaseOrderTopic_Queue";

        public void CreateConnection()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public void Close()
        {
            _connection.Close();
        }

        public void ProcessMessage()
        {
            using(_connection = _factory.CreateConnection())
            {
                using (var channel = _connection.CreateModel())
                {
                    Console.WriteLine("Listening for Topic <payment.purchaseorder>");
                    Console.WriteLine("-----------------------------------------");
                    Console.WriteLine();

                    channel.ExchangeDeclare(ExchangeName, "topic");
                    channel.QueueDeclare(CardPaymentQueueName, true, false, false, null);
                    channel.QueueBind(CardPaymentQueueName, ExchangeName, "payment.purchaseorder");

                    channel.BasicQos(0, 10, false);
                    int messageCount = Convert.ToInt16(channel.MessageCount(CardPaymentQueueName));
                    Console.WriteLine(" Listening to the queue. This channels has {0} messages on the queue", messageCount);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine(" Location received: " + message);
                        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        Thread.Sleep(1000);
                    };
                    channel.BasicConsume(queue: CardPaymentQueueName,
                                         autoAck: false,
                                         consumer: consumer);

                    Thread.Sleep(1000 * messageCount);
                    Console.WriteLine(" Connection closed, no more messages.");
                    Console.ReadLine();
                }
            }
        }

    }
}
