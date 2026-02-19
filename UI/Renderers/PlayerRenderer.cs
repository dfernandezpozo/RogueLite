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
            
            
            inventario
                .GroupBy(i => i.Nombre)
                .Select(grupo => new
                {
                    Nombre = grupo.Key,
                    Cantidad = grupo.Count(),
                    ValorTotal = grupo.Sum(i => i.Valor)
                })
                .ToList()
                .ForEach(item => Console.WriteLine($"  ▸ {item.Nombre} x{item.Cantidad} [+{item.ValorTotal}]"));
        }

        public void MostrarBendiciones(List<Bendicion> bendiciones)
        {
            if (!bendiciones.Any()) return;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n✨ BENDICIONES ACTIVAS:");
            Console.ResetColor();
            
            
            bendiciones
                .GroupBy(b => b.Nombre)
                .Select(grupo => new
                {
                    Nombre = grupo.Key,
                    Cantidad = grupo.Count(),
                    ValorTotal = grupo.Sum(b => b.Valor),
                    Tipo = grupo.First().Tipo
                })
                .ToList()
                .ForEach(b => Console.WriteLine($"  ▸ {b.Nombre} x{b.Cantidad} [+{b.ValorTotal} {b.Tipo}]"));
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