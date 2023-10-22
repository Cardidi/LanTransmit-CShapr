using System.Net.Sockets;
using System.Net;
using xyz.cardidi.LanTransmit;

namespace LocalTransmit;

public partial class IsReceivingPage : ContentPage
{
    CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private DirectoryInfo tempdir = Directory.CreateTempSubdirectory();

    private string tempPath => tempdir.FullName + '/' + "tempfilecache";

    private string _address;

    private ushort _port = 8090;

	public IsReceivingPage()
	{
		InitializeComponent();

        Outliner();
	}

    private void AddLog(string message)
    {
        console.Text += message;
    }


    private void AddLogLine(string message)
    {
        console.Text += message + '\n';
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new InvalidProgramException("No network adapters with an IPv4 address in the system!");
    }

    private async void Outliner()
    {
        Save.IsEnabled = false;

        try
        {
            AddLogLine("Create temp file cache at : " + tempPath);
            if (File.Exists(tempPath)) File.Delete(tempPath);
            File.Create(tempPath);

            _address = GetLocalIPAddress();
            AddLogLine($"Find local internet ip : {_address}");
            AddLogLine($"Set port to : {_port}");

            ip_setting.Text = _address + ':' + _port;
            AddLogLine("Starting Server Service...");
            if (TransmitServer.IsCreated)
            {
                AddLogLine("Server has been created without being notified. Restarting...");
                TransmitServer.Destroy();
                if (!TransmitServer.Create(_port)) throw new InvalidOperationException("Can not create server!");
            }
            AddLogLine("Server is ready.");


            AddLogLine("Await for file transmission.");
            await TransmitServer.StartReceive(tempPath, cancellationTokenSource.Token);
            AddLogLine("File is ready to save.");
            Save.IsEnabled = true;
        }
        catch (InvalidProgramException ex) 
        {
            AddLogLine(ex.Message);
        }
        catch (OperationCanceledException ex)
        {
            AddLogLine("Operation was canceled by user.");
        }
        catch (Exception ex)
        {
            AddLogLine("Operation was quitted to unhandled exception:" + ex.Message);
        }
        finally
        {
            TransmitClient.Destroy();
            AddLog("Destroy server when unused.");
        }
    }

    protected override bool OnBackButtonPressed()
    {
        cancellationTokenSource.Cancel();
        return base.OnBackButtonPressed();
    }

    private async void Save_Clicked(object sender, EventArgs e)
    {
        var result = await FilePicker.Default.PickAsync();
        if (result != null)
        {
            File.Copy(tempPath, result.FullPath, true);
        }
    }
}