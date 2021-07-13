using Calculator;
using Greet;
using Grpc.Core;
using Prime;
using System;
using System.IO;

namespace server
{
    class Program
    {
        const int Port = 50051;
        static void Main(string[] args)
        {
            Server server = null;
            try
            {
                server = new Server()
                {
                    //Services = { CalculatorService.BindService(new CalculatorServiceImpl()) },
                    //Services = { GreetingService.BindService(new GreetingServiceImpl()) },
                    Services = { PrimeNumberService.BindService(new PrimeNumberServiceImpl()) },
                    Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
                };

                server.Start();
                Console.WriteLine("The server is listening on port: " + Port);
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.WriteLine("The server failed to start:" + e.Message);
                throw;
            }
            finally
            {
                if (server != null)
                {
                    server.ShutdownAsync().Wait();
                }
            }
        }
    }
}
