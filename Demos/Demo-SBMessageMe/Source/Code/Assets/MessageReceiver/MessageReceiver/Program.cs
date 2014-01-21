using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace MessageReceiver
{
    class Program
    {
        static void Main(string[] args) {

            QueueClient client = QueueClient.Create("demomessages");

            while (true) {

                var message = client.Receive();
                if (message != null) {

                    try {
                        Console.WriteLine(message.GetBody<string>());
                    }
                    finally {
                        message.Complete();
                    }
                }
            }
        }
    }
}