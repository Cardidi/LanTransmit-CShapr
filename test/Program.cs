// See https://aka.ms/new-console-template for more information

using xyz.cardidi.LanTransmit;

if (TransmitClient.IsCreated)
{
    Console.WriteLine("Client has not being created. Start create.");
    if (!TransmitClient.Create())
    {
        Console.WriteLine("Creation was failed! Exit.");
        return;
    }
    
    Console.WriteLine("Creation was successful!");
}