# GrpcDemo
net framework grpc
## protocol 官网
https://developers.google.com/protocol-buffers

## 步骤
 1. 新建工程GrpcClient、GrpcServer和GrpcLibrary 。
 2. 三个项目GrpcClient、GrpcServer、GrpcLibrary均安装程序包Grpc。 
 3. 三个项目GrpcClient、GrpcServer、GrpcLibrary均安装程序包Google.Protobuf 。 
 4. 类库GrpcLibrary安装程序包Grpc.Tools。
 5. 在项目GrpcLibrary里添加HelloWorld.proto用以生成代码。
``` 
syntax = "proto3";
package GrpcLibrary;
service GrpcService {
  rpc SayHello (HelloRequest) returns (HelloReply) {}
}
  
message HelloRequest {
  string name = 1;
}
  
message HelloReply {
  string message = 1;
}
``` 
---
  注意:生成协议代码需 protoc.exe、grpc_csharp_plugin.exe工具.
  在.net framework 项目下引用安装 Grpc.Tools 会得到protoc.exe、grpc_csharp_plugin.exe，但.net core 项目引用安装是不会下载工具到项目目录的，所以我们需要建一个.net framework项目，
  我建了个.net framework类库执行Install-Package Grpc.Tools用于引用安装得到工具。
  从packages\Grpc.Tools.1.19.0\tools\windows_x64文件夹下复制grpc_csharp_plugin.exe,protoc.exe复制到GrpcLibrary文件夹下
 6. 在GrpcLibrary下创建1.cmd文件,输入一下内容并保存
 ```
 protoc -I . --csharp_out . --grpc_out . --plugin=protoc-gen-grpc=grpc_csharp_plugin.exe HelloWorld.proto
 ```
 7. 最后GrpcClient、GrpcServer分别引用类库GrpcLibrary。
 8. 服务端代码
```
class GrpcImpl : GrpcService.GrpcServiceBase
{
    // 实现SayHello方法
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        return Task.FromResult(new HelloReply { Message = "Hello " + request.Name });
    }
}
class Program
{
    const int Port = 10007;
 
    public static void Main(string[] args)
    {
        Server server = new Server
        {
            Services = { GrpcService.BindService(new GrpcImpl()) },
            Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
        };
        server.Start();
 
        Console.WriteLine("GrpcService server listening on port " + Port);
        Console.WriteLine("任意键退出...");
        Console.ReadKey();
 
        server.ShutdownAsync().Wait();
    }
}
```
 9. 客户端代码
 ```
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
 ```
