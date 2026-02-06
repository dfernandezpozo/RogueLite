using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Renderers
{
    /// <summary>
    /// Renderiza información del jugador en la interfaz.
    /// </summary>
    public class PlayerRenderer
    {
        private readonly HealthBarRenderer _healthBar;

        public PlayerRenderer()
        {
            _healthBar = new HealthBarRenderer();
        }

        public void MostrarHeader(Personaje jugador)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.Write("║ ");
            
            MostrarNombreYClase(jugador);
            _healthBar.Mostrar(jugador.Vida, jugador.VidaMaxima);
            MostrarNivelYExperiencia(jugador);
            
            Console.WriteLine("".PadRight(5) + "║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void MostrarInventario(List<Objeto> inventario)
        {
            if (!inventario.Any()) return;

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n🎒 INVENTARIO:");
            Console.ResetColor();
            
            foreach (var item in inventario.GroupBy(i => i.Nombre))
            {
                var cantidad = item.Count();
                var valorTotal = item.Sum(i => i.Valor);
                Console.WriteLine($"  ▸ {item.Key} x{cantidad} [+{valorTotal}]");
            }
        }

        public void MostrarBendiciones(List<Bendicion> bendiciones)
        {
            if (!bendiciones.Any()) return;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n✨ BENDICIONES ACTIVAS:");
            Console.ResetColor();
            
            foreach (var b in bendiciones.GroupBy(b => b.Nombre))
            {
                var cantidad = b.Count();
                var valorTotal = b.Sum(x => x.Valor);
                Console.WriteLine($"  ▸ {b.Key} x{cantidad} [+{valorTotal} {b.First().Tipo}]");
            }
        }

        private void MostrarNombreYClase(Personaje jugador)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{jugador.Nombre} ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{jugador.Tipo}] ");
            Console.ResetColor();
        }

        private void MostrarNivelYExperiencia(Personaje jugador)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  Nivel: {jugador.Nivel}  XP: {jugador.Experiencia}");
        }
    }
}
