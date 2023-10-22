namespace LocalTransmit;

public partial class SendPage : ContentPage
{

	public SendPage()
	{
		InitializeComponent();
	}


    private async void FileSelector_Clicked(object sender, EventArgs e)
    {
		var result = await FilePicker.Default.PickAsync();
		if (result != null)
		{
			filepath.Text = result.FullPath;
		}
		else
		{
			filepath.Text = null;
		}
	}

    private async void Start_Clicked(object sender, EventArgs e)
    {

		if (!File.Exists(filepath.Text))
		{
            await DisplayAlert("Fatal", "Path to file is not existed!", "Ok");
            return;
        }

		if (!ushort.TryParse(rev_port.Text, out var port) || port == 0)
        {
            await DisplayAlert("Fatal", "Port is not valid!", "Ok");
            return;
        }

		if (string.IsNullOrWhiteSpace(rev_ip.Text))
        {
            await DisplayAlert("Fatal", "IP or domain is not valid!", "Ok");
            return;
        }

		await Navigation.PushAsync(new IsSendingPage(filepath.Text, rev_ip.Text, port));
    }
}