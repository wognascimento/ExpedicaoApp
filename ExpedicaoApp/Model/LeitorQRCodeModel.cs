namespace ExpedicaoApp.Model
{
    public class LeitorQRCodeModel(string tipo, bool leitura)
    {
        public string Tipo { get; set; } = tipo;
        public bool Leitura { get; set; } = leitura;
    }
}
