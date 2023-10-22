namespace LocalTransmit;

public partial class HelloPage : ContentPage
{
	public HelloPage()
	{
		InitializeComponent();
	}

    private async void Send_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SendPage());
    }

    private async void Receive_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new IsReceivingPage());
    }
}