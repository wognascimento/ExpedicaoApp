namespace ExpedicaoApp.Model
{
    public class MovimentacaoVolumeShoppingModel
    {
        public int? IdLinhaInserida { get; set; }
        public string BarcodeVolume { get; set; }
        public string BarcodeEndereco { get; set; }
        public string InseridoPor { get; set; }
        public DateTime? InseridoEm { get; set; }
    }
}
