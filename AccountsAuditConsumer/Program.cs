using PaymentsCardConsumer.RabbitMQ;
using System;

namespace AccountsAuditConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            RabbitMQConsumer client = new RabbitMQConsumer();
            client.CreateConnection();
            client.ProcessMessage();
        }
    }
}
