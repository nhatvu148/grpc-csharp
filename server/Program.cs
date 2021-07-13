using Grpc.Core;
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
