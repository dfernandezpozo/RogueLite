public class Ataque
{
    public string Id { get; set; } = "";
    public string Nombre { get; set; } = "";
    public int Danio { get; set; } = 0;
    public int CostoEnergia { get; set; } = 0;
    public string Tipo { get; set; } = "Fisico";

   
    public double ReduccionDanio { get; set; } = 0;
    public int AumentoAtaque { get; set; } = 0;
    public int RecuperaEnergia { get; set; } = 0;
    public int AumentoVida { get; set; } = 0;
}
