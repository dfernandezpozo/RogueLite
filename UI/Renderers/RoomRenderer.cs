using System;
using System.Linq;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Renderers
{
    /// <summary>
    /// Renderiza información de la sala actual.
    /// </summary>
    public class RoomRenderer
    {
        private readonly HealthBarRenderer _healthBar;

        public RoomRenderer()
        {
            _healthBar = new HealthBarRenderer();
        }

        public void MostrarInfoSala(Sala sala)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n📍 {sala.Nombre.ToUpper()} {(sala.Completada ? "✓" : "")}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"   {sala.Descripcion}");
            Console.ResetColor();
        }

        public void MostrarEnemigos(Sala sala)
        {
            if (sala.TieneEnemigos())
            {
                MostrarListaEnemigos(sala);
            }
            else
            {
                MostrarSalaDespejada();
            }
        }

        public void MostrarObjetos(Sala sala)
        {
            if (!sala.TieneObjetos()) return;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n💎 OBJETOS EN SALA:");
            Console.ResetColor();
            
            foreach (var obj in sala.Objetos)
            {
                Console.WriteLine($"  ▸ {obj.Nombre} [{obj.Tipo}] +{obj.Valor}");
            }
        }

        private void MostrarListaEnemigos(Sala sala)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n⚔️  ENEMIGOS:");
            Console.ResetColor();

            foreach (var enemigo in sala.Enemigos.OrderByDescending(e => e.Vida))
            {
                MostrarEnemigo(enemigo);
            }
        }

        private void MostrarEnemigo(Enemigo enemigo)
        {
            Console.Write($"  ▸ {enemigo.Nombre} ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{enemigo.Tipo}] ");
            Console.ResetColor();
            _healthBar.Mostrar(enemigo.Vida, enemigo.VidaMaxima);
            Console.WriteLine($" ⚔️ {enemigo.Ataque}");
        }

        private void MostrarSalaDespejada()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n✓ Sala despejada");
            Console.ResetColor();
        }
    }
}
