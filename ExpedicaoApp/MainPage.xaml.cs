
using ExpedicaoApp.Views.Enderecamento;

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
    }

}
