using System.Runtime.InteropServices;

namespace xyz.cardidi.LanTransmit;

public static class TransmitClient
{
    #region Unmanaged
    
    [DllImport("lan_transmit.dll")]
    private static extern bool exist_client();
    
    [DllImport("lan_transmit.dll")]
    private static extern bool create_client();
    
    [DllImport("lan_transmit.dll")]
    private static extern bool destroy_client();
    
    [DllImport("lan_transmit.dll")]
    private static extern bool client_is_sending();
    
    [DllImport("lan_transmit.dll")]
    private static extern void client_interrupt_send();
    
    [DllImport("lan_transmit.dll")]
    private static extern int client_start_send(ref char file_path, ushort srv_port, ref char srv_address);

    #endregion

    public static bool IsCreated => exist_client();

    public static bool Create() => create_client();

    public static bool Destroy() => destroy_client();

    public static bool IsSending => client_is_sending();

    public static async Task StartSend(string filePath, string address, ushort port, CancellationToken token)
    {
        int code = 0;
        try
        {
            var fp_ary = filePath.ToArray();
            var address_ary = address.ToArray();
            code = client_start_send(ref fp_ary[0], port, ref address_ary[0]);
            while (client_is_sending())
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(50);
            }
        }
        catch (OperationCanceledException) 
        {
            client_interrupt_send();
            throw; 
        }
        catch (Exception e)
        {
            throw new InvalidOperationException("Unknown error happened!", e);
        }

        if (code == -1) throw new InvalidOperationException("Did not create properly client!");
    }

}