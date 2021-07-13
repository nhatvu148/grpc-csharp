using Calculator;
using Dummy;
using Greet;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:50051";

        static void Main(string[] args)
        {
            Channel channel = new Channel(target, ChannelCredentials.Insecure);

            channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The client connected successfully");
                }
            });

            //var client = new DummyService.DummyServiceClient(channel);
            //var client = new GreetingService.GreetingServiceClient(channel);
            var client = new CalculatorService.CalculatorServiceClient(channel);

            //var greeting = new Greeting()
            //{
            //    FirstName = "Nhat",
            //    LastName = "Vu"
            //};
            //var request = new GreetingRequest() { Greeting = greeting };
            //var response = client.Greet(request);

            var request = new SumRequest()
            {
                A = 3,
                B = 15
            };

            var response = client.Sum(request);


            Console.WriteLine(response.Result);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
