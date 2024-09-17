
using ExpedicaoApp.Views.Enderecamento;
using ExpedicaoApp.Views.PreConferencia;
using ExpedicaoApp.Views.Romaneio;
using ExpedicaoApp.Views.VolumeShopping;

namespace ExpedicaoApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        async void OnButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Enderecamento));
        }

        private async void BtnRomaneioClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(Romaneio));
        }

        private async void BtnVolumeShoppingClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(VolumeShopping));
        }

        private async void BtnPreConferenciaClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(PreConferencia));
        }
    }

}
