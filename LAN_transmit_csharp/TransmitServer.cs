using System.Runtime.InteropServices;

namespace xyz.cardidi.LanTransmit;

public static class TransmitServer
{
    #region Unmanaged
    
    [DllImport("lan_transmit.dll")]
    private static extern bool exist_server();
    
    [DllImport("lan_transmit.dll")]
    private static extern bool create_server(ushort port);
    
    [DllImport("lan_transmit.dll")]
    private static extern bool destroy_server();
    
    [DllImport("lan_transmit.dll")]
    private static extern bool server_is_receiving();
    
    [DllImport("lan_transmit.dll")]
    private static extern void server_interrupt_receive();
    
    [DllImport("lan_transmit.dll")]
    private static extern int server_start_receive(ref char save_path);

    #endregion

    public static bool IsCreated => exist_server();

    public static bool Create(ushort port) => create_server(port);

    public static bool Destroy() => destroy_server();

    public static bool IsReceiving => server_is_receiving();

    public static async Task StartReceive(string savepath, CancellationToken token)
    {
        int code = 0;
        try
        {
            var fp_ary = savepath.ToArray();
            await Task.Run(() =>
            {
                return server_start_receive(ref fp_ary[0]);
            }).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            server_interrupt_receive();
            throw;
        }
        catch (Exception e)
        {
            server_interrupt_receive();
            throw new InvalidOperationException("Unknown error happened!", e);
        }

        if (code == -1) throw new InvalidOperationException("Did not create properly server!");
    }

}