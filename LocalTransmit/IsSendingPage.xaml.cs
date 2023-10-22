using xyz.cardidi.LanTransmit;

namespace LocalTransmit;

public partial class IsSendingPage : ContentPage
{

	CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

	private string _path;

	private string _address;

	private ushort _port;

	public IsSendingPage(string path, string address, ushort port)
	{
		InitializeComponent();
		_path = path;
		_address = address;
		_port = port;

		OutlinerManager();
	}

	private void AddLog(string message)
	{
		console.Text += message;
	}


    private void AddLogLine(string message)
    {
        console.Text += message + '\n';
    }

	private async void OutlinerManager()
	{
        cancel.IsEnabled = true;

		try
        {

            AddLogLine("Starting Client Service...");
            if (TransmitClient.IsCreated)
            {
                AddLogLine("Client has been created without being notified. Restarting...");
                TransmitClient.Destroy();
                if (!TransmitClient.Create()) throw new InvalidOperationException("Can not create client!");
            }
            AddLogLine("Client is ready.");

            await SendingTask(cancellationTokenSource.Token);
            AddLogLine("Transmit finished.");
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
            AddLog("Destroy client when unused.");
        }

        cancel.IsEnabled = false;
        NavigationPage.SetHasBackButton(this, true);
	}

    private async Task SendingTask(CancellationToken cancelToken)
	{
        AddLogLine($"Start sending: {_address}:{_port}");
        var send_task = TransmitClient.StartSend(_path, _address, _port, cancelToken);
        var log_task = Task.Run(async () =>
        {
            while (TransmitClient.IsSending && !cancelToken.IsCancellationRequested)
            {
                await Task.Delay(5000, cancelToken);
                AddLogLine("Still under progress, next test is 5s later.");
            }
        });

        
        await Task.WhenAll(log_task, send_task);
    }

    private void cancel_Clicked(object sender, EventArgs e)
    {
		if (TransmitClient.IsSending)
        {
            cancellationTokenSource.Cancel();
        }
    }
}