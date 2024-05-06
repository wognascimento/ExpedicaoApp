namespace ExpedicaoApp
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM5MkAzMjM0MkUzMTJFMzlZRnNmeEdKa0haRGU0S0MyZUR3b05vcDJFNURBbnFRTi9STUVidExydWswPQ==");

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
