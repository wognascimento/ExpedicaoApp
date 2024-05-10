using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpedicaoApp.ViewModels
{
    [QueryProperty("Acao", "Acao")]
    public partial class LeitorQrCodeViewModel : ObservableObject
    {
        public LeitorQrCodeViewModel() { }

        [ObservableProperty]
        string acao;
    }
}
