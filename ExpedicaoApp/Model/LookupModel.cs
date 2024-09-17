using SQLite;

namespace ExpedicaoApp.Model
{
    public class LookupModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string SiglaServ { get; set; }
        public string BaiaCaminhao { get; set; }
        public string Barcode { get; set; }
        public string Qrcode { get; set; }
    }
}
