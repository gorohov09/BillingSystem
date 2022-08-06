using Billing;
using Grpc.Net.Client;

Console.WriteLine("Hellow wo");
Thread.Sleep(1000);
var none = new None();

var grpcChannel = GrpcChannel.ForAddress("https://localhost:7175");
var client = new Billing.Billing.BillingClient(grpcChannel);

using (var clientData = client.ListUsers(none))
{
    while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
    {
        var thisUserProfile = clientData.ResponseStream.Current;
        Console.WriteLine(thisUserProfile);
    }
}

//var response_1 = await client.CoinsEmissionAsync(new EmissionAmount { Amount = 10});
//Console.WriteLine(response_1);

//using (var clientData = client.ListUsers(none))
//{
//    while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
//    {
//        var thisUserProfile = clientData.ResponseStream.Current;
//        Console.WriteLine(thisUserProfile);
//    }
//}

var response_2 = await client.MoveCoinsAsync(new MoveCoinsTransaction { SrcUser = "maria", DstUser = "oleg", Amount = 100 });
Console.WriteLine(response_2);

using (var clientData = client.ListUsers(none))
{
    while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
    {
        var thisUserProfile = clientData.ResponseStream.Current;
        Console.WriteLine(thisUserProfile);
    }
}

Console.ReadLine();



//var data = new HelloRequest { Name = "Mukesh" };
//var grpcChannel = GrpcChannel.ForAddress("https://localhost:7175");
//var client = new Greeter.GreeterClient(grpcChannel);
//var response = await client.SayHelloAsync(data);
//Console.WriteLine(response.Message);
//Console.ReadLine();
