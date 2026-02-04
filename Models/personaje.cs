using System.Collections.Generic;
using System.Linq;

namespace RogueLite.Models
{
    public class Personaje
    {
        public string Nombre { get; set; } = "Héroe";
        public string Tipo { get; set; } = "Jugador";
        public int Vida { get; set; } = 100;
        public int VidaMaxima { get; set; } = 100;
        public int Ataque { get; set; } = 10;
        public int Experiencia { get; set; } = 0;
        public int Nivel { get; set; } = 1;
        public List<Objeto> Inventario { get; set; } = new();
        public List<Bendicion> BendicionesActivas { get; set; } = new();

        public List<Ataque> Ataques { get; set; } = new();


        public bool EstaVivo() => Vida > 0;

        public void RecibirDaño(int cantidad)
        {
            Vida -= cantidad;
            if (Vida < 0) Vida = 0;
        }

        public void Curar(int cantidad)
        {
            Vida += cantidad;
            if (Vida > VidaMaxima) Vida = VidaMaxima;
        }

        
        public int CalcularAtaque() =>
            Ataque
            + Inventario.Where(i => i.Tipo == "Arma").Sum(i => i.Valor)
            + BendicionesActivas.Where(b => b.Tipo == "Ataque").Sum(b => b.Valor);

        public int CalcularDefensa() =>
            Inventario.Where(i => i.Tipo is "Armadura" or "Escudo").Sum(i => i.Valor)
            + BendicionesActivas.Where(b => b.Tipo == "Defensa").Sum(b => b.Valor);

        public void GanarExperiencia(int cantidad)
        {
            Experiencia += cantidad;
            VerificarSubidaNivel();
        }

        private void VerificarSubidaNivel()
        {
            int xpNecesaria = Nivel * 100;
            if (Experiencia >= xpNecesaria)
            {
                Nivel++;
                Experiencia -= xpNecesaria;
                VidaMaxima += 20;
                Vida = VidaMaxima;
                Ataque += 2; 
            }
        }

        public bool SubioNivel()
        {
            int xpNecesaria = Nivel * 100;
            return Experiencia >= xpNecesaria;
        }
    }
}
