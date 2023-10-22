namespace LocalTransmit;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        var naviPage = new NavigationPage(new HelloPage());

        MainPage = naviPage;
    }
}