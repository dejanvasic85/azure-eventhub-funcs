using System;
using Microsoft.Azure.EventHubs;
using System.Text;
using System.Threading.Tasks;

namespace EventHubClientCaller
{
    class Program
    {

        static void Main(string[] args)
        {

            Console.WriteLine("Hello! Starting the event hub client");

            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.

            var connectionStringBuilder = new EventHubsConnectionStringBuilder("Endpoint=sb://modorevents.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=pAFwOYBHv75tU+nqxTxChw+T9BG/GxEfaawo7EeaFo0=")
            {
                EntityPath = "the-roof-is-on-fire"
            };

            var eventHubClient = Microsoft.Azure.EventHubs.EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(eventHubClient, 50);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        // Creates an event hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(EventHubClient eventHubClient, int numMessagesToSend)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var failOrSucceed = i % 2 == 0 ? "true" : "false";
                    var id = Guid.NewGuid();
                    var message = "{'success' : " + failOrSucceed + ", 'id': '" + id.ToString() + "'}";
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{numMessagesToSend} messages sent.");
        }
    }
}