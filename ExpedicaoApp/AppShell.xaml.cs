using ExpedicaoApp.Views;
using ExpedicaoApp.Views.Enderecamento;

namespace ExpedicaoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Enderecamento), typeof(Enderecamento));
            Routing.RegisterRoute(nameof(QrCodeScanner), typeof(QrCodeScanner));
        }
    }
}
