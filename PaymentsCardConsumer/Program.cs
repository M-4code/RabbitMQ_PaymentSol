using PaymentsCardConsumer.RabbitMQ;
using System;

namespace PaymentsCardConsumer
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
