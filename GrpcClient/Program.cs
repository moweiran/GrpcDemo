using Grpc.Core;
using GrpcLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Channel channel = new Channel("127.0.0.1:10007", ChannelCredentials.Insecure);

            var client = new GrpcService.GrpcServiceClient(channel);

            var input = Console.ReadLine();
            var reply = client.SayHello(new HelloRequest { Name = input });
            Console.WriteLine("来自" + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("任意键退出...");
            Console.ReadKey();
        }
    }
}
