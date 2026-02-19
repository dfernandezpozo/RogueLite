namespace RogueLite.Models
{
    public class Enemigo
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty; // ← NUEVO
        public int Vida { get; set; }
        public int VidaMaxima { get; set; }
        public int Ataque { get; set; }
        public string Tipo { get; set; } = string.Empty;

        public virtual Enemigo Clone()
        {
            return new Enemigo
            {
                Nombre = this.Nombre,
                Descripcion = this.Descripcion, 
                Vida = this.Vida,
                VidaMaxima = this.VidaMaxima,
                Ataque = this.Ataque,
                Tipo = this.Tipo
            };
        }

        public bool EstaVivo() => Vida > 0;

        public void RecibirDaño(int cantidad)
        {
            Vida -= cantidad;
            if (Vida < 0) Vida = 0;
        }
    }
}