namespace RogueLite.Models
{
    public class Objeto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Valor { get; set; }
        public bool EsConsumible { get; set; } = false;
    }
}