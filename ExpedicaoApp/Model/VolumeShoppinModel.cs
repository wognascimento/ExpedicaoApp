using SQLite;

namespace ExpedicaoApp.Model
{
    public class VolumeShoppinModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Barcode { get; set; }
        public DateTime? Data { get; set; }
        public string Resp { get; set; }
        public string Caminhao { get; set; }
        public bool Enviado { get; set; } = false;
    }
}
