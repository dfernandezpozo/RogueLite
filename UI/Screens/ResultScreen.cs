using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de resultados (victoria o derrota).
    /// </summary>
    public class ResultScreen
    {
        public void MostrarVictoria(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            Console.Clear();
            MostrarBannerVictoria();
            MostrarEstadisticasVictoria(dañoTotal, derrotados, inventario, jugador);
            EsperarSalida();
        }

        public void MostrarGameOver(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            Console.Clear();
            MostrarBannerDerrota();
            MostrarEstadisticasDerrota(dañoTotal, derrotados, jugador);
            EsperarSalida();
        }

        private void MostrarBannerVictoria()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║            🏆  ¡VICTORIA!  🏆                   ║
    ║                                               ║
    ║        Has conquistado la mazmorra            ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
            System.Threading.Thread.Sleep(1000);
        }

        private void MostrarBannerDerrota()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║            💀  GAME OVER  💀                    ║
    ║                                               ║
    ║          Has caído en la mazmorra...          ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
        }

        private void MostrarEstadisticasVictoria(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n📊 ESTADÍSTICAS FINALES:\n");
            Console.ResetColor();

            Console.WriteLine($"  🎭 Héroe: {jugador.Nombre} [{jugador.Tipo}]");
            Console.WriteLine($"  ⚔️  Daño total infligido: {dañoTotal}");
            Console.WriteLine($"  💀 Enemigos derrotados: {derrotados.Count}");
            Console.WriteLine($"  🎒 Objetos recolectados: {inventario.Count}");
            Console.WriteLine($"  ⭐ Nivel alcanzado: {jugador.Nivel}");

            MostrarObjetoMasValioso(inventario);
            MostrarEnemigosDerrotados(derrotados);
        }

        private void MostrarEstadisticasDerrota(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            Console.WriteLine($"\n  🎭 {jugador.Nombre} ha caído en batalla...");
            Console.WriteLine($"  ⚔️  Daño total: {dañoTotal}");
            Console.WriteLine($"  💀 Enemigos derrotados: {derrotados.Count}");
        }

        private void MostrarObjetoMasValioso(List<Objeto> inventario)
        {
            var objetoMasValioso = inventario.OrderByDescending(o => o.Valor).FirstOrDefault();
            if (objetoMasValioso != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  💎 Objeto más valioso: {objetoMasValioso.Nombre} [{objetoMasValioso.Valor}]");
                Console.ResetColor();
            }
        }

        private void MostrarEnemigosDerrotados(List<Enemigo> derrotados)
        {
            var grupos = derrotados.GroupBy(e => e.Tipo);
            if (grupos.Any())
            {
                Console.WriteLine("\n  📋 Enemigos por tipo:");
                foreach (var g in grupos)
                {
                    Console.WriteLine($"     • {g.Key}: {g.Count()}");
                }
            }
        }

        private void EsperarSalida()
        {
            Console.WriteLine("\n\n  Presiona cualquier tecla para salir...");
            Console.ReadKey(true);
        }
    }
}
