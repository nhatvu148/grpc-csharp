using Average;
using Calculator;
using Dummy;
using Greet;
using Grpc.Core;
using Max;
using Prime;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace client
{
    class Program
    {
        const string target = "127.0.0.1:43567";

        static async Task Main(string[] args)
        {
            var clientCert = File.ReadAllText("ssl/client.crt");
            var clientKey = File.ReadAllText("ssl/client.key");
            var caCrt = File.ReadAllText("ssl/ca.crt");

            var channelCredentials = new SslCredentials(caCrt, new KeyCertificatePair(clientCert, clientKey));

            Channel channel = new Channel("localhost", 43567, channelCredentials); //new Channel(target, ChannelCredentials.Insecure)

            await channel.ConnectAsync().ContinueWith(task =>
            {
                if (task.Status == TaskStatus.RanToCompletion)
                {
                    Console.WriteLine("The client connected successfully");
                }
            });

            //var client = new DummyService.DummyServiceClient(channel);

            //var client = new CalculatorService.CalculatorServiceClient(channel);
            //CallCalculator(client);

            //var client = new PrimeNumberService.PrimeNumberServiceClient(channel);
            //await CallPrimeNumber(client);

            //var client = new AverageService.AverageServiceClient(channel);
            //await CallAverage(client);

            //var client = new FindMaxService.FindMaxServiceClient(channel);
            //await CallMax(client);

            var client = new GreetingService.GreetingServiceClient(channel);
            //DoSimpleGreet(client);
            await DoManyGreetings(client);
            //await DoLongGreet(client);
            //await DoGreetEveryone(client);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }

        public static void CallDummy()
        {

        }

        public static void CallCalculator(CalculatorService.CalculatorServiceClient client)
        {
            var request = new SumRequest()
            {
                A = 3,
                B = 15
            };
            var response = client.Sum(request);
            Console.WriteLine(response.Result);
        }

        public static async Task CallPrimeNumber(PrimeNumberService.PrimeNumberServiceClient client)
        {
            var request = new PrimeNumberDecompositionRequest()
            {
                Number = 120
            };
            var response = client.PrimeNumberDecomposition(request);
            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.PrimeFactor);
                await Task.Delay(200);
            }
        }

        public static async Task CallAverage(AverageService.AverageServiceClient client)
        {
            var stream = client.ComputeAverage();
            foreach (int number in Enumerable.Range(1, 4))
            {
                var request = new AverageRequest() { Number = number };

                await stream.RequestStream.WriteAsync(request);
            }
            await stream.RequestStream.CompleteAsync();
            var response = await stream.ResponseAsync;
            Console.WriteLine(response.Result);
        }

        public static async Task CallMax(FindMaxService.FindMaxServiceClient client)
        {
            var stream = client.findMaximum();

            var responseReaderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                    Console.WriteLine(stream.ResponseStream.Current.Max);
            });
            int[] numbers = { 1, 5, 3, 6, 2, 20 };
            foreach (var number in numbers)
            {
                await stream.RequestStream.WriteAsync(new FindMaxRequest() { Number = number });
            }
            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;
        }

        public static void DoSimpleGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Nhat",
                LastName = "Vu"
            };

            var request = new GreetingRequest() { Greeting = greeting };
            var response = client.Greet(request);

            Console.WriteLine(response.Result);
        }
        public static async Task DoManyGreetings(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Nhat",
                LastName = "Vu"
            };

            var request = new GreetManyTimesRequest() { Greeting = greeting };
            var response = client.GreetManyTimes(request);

            while (await response.ResponseStream.MoveNext())
            {
                Console.WriteLine(response.ResponseStream.Current.Result);
                await Task.Delay(200);
            }
        }
        public static async Task DoLongGreet(GreetingService.GreetingServiceClient client)
        {
            var greeting = new Greeting()
            {
                FirstName = "Nhat",
                LastName = "Vu"
            };

            var request = new LongGreetRequest() { Greeting = greeting };
            var stream = client.LongGreet();

            foreach (int i in Enumerable.Range(1, 10))
            {
                await stream.RequestStream.WriteAsync(request);
            }

            await stream.RequestStream.CompleteAsync();

            var response = await stream.ResponseAsync;

            Console.WriteLine(response.Result);
        }
        public static async Task DoGreetEveryone(GreetingService.GreetingServiceClient client)
        {
            var stream = client.GreetEveryone();

            var responseReaderTask = Task.Run(async () =>
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    Console.WriteLine("Received : " + stream.ResponseStream.Current.Result);
                }
            });

            Greeting[] greetings =
            {
                new Greeting() { FirstName = "John", LastName = "Doe" },
                new Greeting() { FirstName = "Nhat", LastName = "Vu" },
                new Greeting() { FirstName = "Fuad", LastName = "Akbar" }
            };

            foreach (var greeting in greetings)
            {
                Console.WriteLine("Sending : " + greeting.ToString());
                await stream.RequestStream.WriteAsync(new GreetEveryoneRequest()
                {
                    Greeting = greeting
                });
            }

            await stream.RequestStream.CompleteAsync();
            await responseReaderTask;
        }
    }
}
