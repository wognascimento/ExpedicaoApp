using ExpedicaoApp.Views;
using ExpedicaoApp.Views.Enderecamento;
using ExpedicaoApp.Views.PreConferencia;
using ExpedicaoApp.Views.Romaneio;
using ExpedicaoApp.Views.VolumeShopping;

namespace ExpedicaoApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Enderecamento), typeof(Enderecamento));
            Routing.RegisterRoute(nameof(QrCodeScanner), typeof(QrCodeScanner));
            Routing.RegisterRoute(nameof(Romaneio), typeof(Romaneio));
            Routing.RegisterRoute(nameof(VolumeShopping), typeof(VolumeShopping));
            Routing.RegisterRoute(nameof(PreConferencia), typeof(PreConferencia));
        }
    }
}
