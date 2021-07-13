using Average;
using Calculator;
using Greet;
using Grpc.Core;
using Max;
using Prime;
using Sqrt;
using System;
using System.Collections.Generic;
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
                var serverCert = File.ReadAllText("ssl/server.crt");
                var serverKey = File.ReadAllText("ssl/server.key");
                var keypair = new KeyCertificatePair(serverCert, serverKey);
                var cacert = File.ReadAllText("ssl/ca.crt");
                var credentials = new SslServerCredentials(new List<KeyCertificatePair>() { keypair }, cacert, true);

                server = new Server()
                {
                    Services = { GreetingService.BindService(new GreetingServiceImpl()) },
                    //Services = { CalculatorService.BindService(new CalculatorServiceImpl()) },
                    //Services = { AverageService.BindService(new AverageServiceImpl()) },
                    //Services = { PrimeNumberService.BindService(new PrimeNumberServiceImpl()) },
                    //Services = { FindMaxService.BindService(new FindMaxServiceImpl()) },
                    //Services = { SqrtService.BindService(new SqrtServiceImpl()) },
                    Ports = { new ServerPort("localhost", Port, credentials) }//ServerCredentials.Insecure
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
