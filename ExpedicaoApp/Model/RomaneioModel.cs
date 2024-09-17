using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;

namespace ExpedicaoApp.Model
{
    public class RomaneioModel : ObservableObject
    {
        [PrimaryKey]
        public int CodRomaneiro { get; set; }
        public DateTime DataCarregamento { get; set; } = DateTime.Now;
        public TimeSpan HoraChegada { get; set; }
        public int Codtransportadora { get; set; }
        public string NomeMotorista { get; set; }

        //public string PlacaCaminhao { get; set; }
        private string _placaCaminhao;
        public string PlacaCaminhao
        {
            get => _placaCaminhao;
            set => SetProperty(ref _placaCaminhao, value);
        }

        //public string PlacaCidade { get; set; }
        private string _placaCidade;
        public string PlacaCidade
        {
            get => _placaCidade;
            set => SetProperty(ref _placaCidade, value);
        }

        //public string PlacaEstado { get; set; }
        private string _placaEstado;
        public string PlacaEstado
        {
            get => _placaEstado;
            set => SetProperty(ref _placaEstado, value);
        }

        //public string PlacaCarroceria { get; set; }
        private string _placaCarroceria;
        public string PlacaCarroceria
        {
            get => _placaCarroceria;
            set
            {
                if (SetProperty(ref _placaCarroceria, value))
                {
                    PlacaCaminhao = _placaCarroceria; // Atualiza PlacaCava quando PlacaCarroseria mudar
                }
            }
        }

        //public string PlacaCarroceriaCidade { get; set; }
        private string _placaCarroceriaCidade;
        public string PlacaCarroceriaCidade
        {
            get => _placaCarroceriaCidade;
            set
            {
                if (SetProperty(ref _placaCarroceriaCidade, value))
                {
                    PlacaCidade = _placaCarroceriaCidade; // Atualiza PlacaCava quando PlacaCarroseria mudar
                }
            }
        }

        //public string PlacaCarroceriaEstado { get; set; }
        private string _placaCarroceriaEstado;
        public string PlacaCarroceriaEstado
        {
            get => _placaCarroceriaEstado;
            set
            {
                if (SetProperty(ref _placaCarroceriaEstado, value))
                {
                    PlacaEstado = _placaCarroceriaEstado; // Atualiza PlacaCava quando PlacaCarroseria mudar
                }
            }
        }

        public string NumeroContainer { get; set; }
        public double? BauAltura { get; set; }
        public double? BauLargura { get; set; }
        public double? BauProfundidade { get; set; }
        public double? M3Carregado { get; set; }
        public double? BauSoba { get; set; }
        public string CondicaoCaminhao { get; set; }
        //public TimeOnly? InicioCarregamento { get; set; }
        //public TimeOnly? TerminoCarregamento { get; set; }
        public int? NumeroCaminhao { get; set; }
        public string ShoppingDestino { get; set; }
        public string LocalCarregamento { get; set; }
        public string NumLacres { get; set; }
        public string NomeConferente { get; set; }
        public string IncluidoPor { get; set; }
        public DateTime? IncluidoData { get; set; }
        public string AlteradoPor { get; set; }
        public DateTime? AlteradoData { get; set; }
        public string NumeroCnh { get; set; }
        public string TelefoneMotorista { get; set; }
        public float? M3Portaria { get; set; }
        public string Operacao { get; set; }
        public string ConferenteDescarregamento { get; set; }
        public string LacreChegada { get; set; }
        //public TimeOnly? HoraInicioDescarregamento { get; set; }
        //public TimeOnly? HoraTerminoDescarregamento { get; set; }
        //public DateOnly? DataInicioDescarregamento { get; set; }
        //public DateOnly? DataTerminoDescarregamento { get; set; }
        //public DateOnly? DataSaidaCaminhao { get; set; }
        //public TimeOnly? HoraSaidaCaminhao { get; set; }
        //public DateTime? DataHoraLiberacao { get; set; }

    }
}
