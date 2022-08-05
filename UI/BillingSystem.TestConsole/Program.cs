using Billing;
using Grpc.Net.Client;

Console.WriteLine("Hellow wo");
Thread.Sleep(1000);
var none = new None();

var grpcChannel = GrpcChannel.ForAddress("https://localhost:7175");
var client = new Billing.Billing.BillingClient(grpcChannel);

var response = await client.CoinsEmissionAsync(new EmissionAmount { Amount = 10});
Console.WriteLine(response);

Console.ReadLine();

using (var clientData = client.ListUsers(none))
{
    while (await clientData.ResponseStream.MoveNext(new CancellationToken()))
    {
        var thisUserProfile = clientData.ResponseStream.Current;
        Console.WriteLine(thisUserProfile);
    }
}

//var data = new HelloRequest { Name = "Mukesh" };
//var grpcChannel = GrpcChannel.ForAddress("https://localhost:7175");
//var client = new Greeter.GreeterClient(grpcChannel);
//var response = await client.SayHelloAsync(data);
//Console.WriteLine(response.Message);
//Console.ReadLine();
