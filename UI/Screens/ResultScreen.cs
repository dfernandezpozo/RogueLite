using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RogueLite.Models;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de resultados (victoria o derrota) - Rediseño ÉPICO.
    /// </summary>
    public class ResultScreen
    {
        public void MostrarVictoria(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            Console.Clear();
            Console.CursorVisible = false;
            
            EfectoVictoria();
            MostrarBannerVictoriaEpico();
            MostrarEstadisticasVictoriaEpicas(dañoTotal, derrotados, inventario, jugador);
            EsperarSalida();
            
            Console.CursorVisible = true;
        }

        public void MostrarGameOver(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            Console.Clear();
            Console.CursorVisible = false;
            
            EfectoDerrota();
            MostrarBannerDerrotaEpico();
            MostrarEstadisticasDerrotaEpicas(dañoTotal, derrotados, jugador);
            EsperarSalida();
            
            Console.CursorVisible = true;
        }

        private void EfectoVictoria()
        {
            // Efecto de partículas de victoria
            Console.WriteLine("\n\n");
            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Yellow : ConsoleColor.Green;
                Console.WriteLine("    ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨ ★ ✨");
                Thread.Sleep(100);
            }
            Console.ResetColor();
            Thread.Sleep(300);
        }

        private void EfectoDerrota()
        {
            // Efecto de desvanecimiento
            Console.WriteLine("\n\n");
            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
                Thread.Sleep(150);
            }
            Console.ResetColor();
            Thread.Sleep(400);
        }

        private void MostrarBannerVictoriaEpico()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════════════╗
    ║                                                       ║
    ║     🏆 ═══════════ ¡VICTORIA! ═══════════ 🏆         ║
    ║                                                       ║
    ║          ¡Has conquistado la mazmorra!               ║
    ║                                                       ║
    ║            Tu leyenda será recordada                 ║
    ║                                                       ║
    ╚═══════════════════════════════════════════════════════╝
");
            Console.ResetColor();
            Thread.Sleep(800);
            
            // Efecto de brillos
            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(10 + i * 8, 4);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("✨");
                Thread.Sleep(100);
            }
            Console.ResetColor();
            Thread.Sleep(500);
        }

        private void MostrarBannerDerrotaEpico()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════════════╗
    ║                                                       ║
    ║     💀 ══════════ GAME OVER ══════════ 💀            ║
    ║                                                       ║
    ║          Has caído en la mazmorra...                 ║
    ║                                                       ║
    ║         Pero tu espíritu perdurará                   ║
    ║                                                       ║
    ╚═══════════════════════════════════════════════════════╝
");
            Console.ResetColor();
            Thread.Sleep(1000);
        }

        private void MostrarEstadisticasVictoriaEpicas(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("                    📊 ESTADÍSTICAS FINALES");
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();

            // Héroe
            MostrarStatEpica("🎭 HÉROE", $"{jugador.Nombre} [{jugador.Tipo}]", ConsoleColor.Yellow);
            Thread.Sleep(200);
            
            // Stats principales con barras
            MostrarStatConValor("⚔️  DAÑO TOTAL", dañoTotal, ConsoleColor.Red);
            Thread.Sleep(200);
            
            MostrarStatConValor("💀 ENEMIGOS DERROTADOS", derrotados.Count, ConsoleColor.DarkRed);
            Thread.Sleep(200);
            
            MostrarStatConValor("🎒 OBJETOS RECOLECTADOS", inventario.Count, ConsoleColor.Cyan);
            Thread.Sleep(200);
            
            MostrarStatConValor("⭐ NIVEL ALCANZADO", jugador.Nivel, ConsoleColor.Yellow);
            Thread.Sleep(200);
            
            MostrarStatConValor("💰 ORO FINAL", jugador.Oro, ConsoleColor.Yellow);
            Thread.Sleep(300);

            // Separador
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄");
            Console.ResetColor();
            Console.WriteLine();

            // Detalles adicionales
            MostrarObjetoMasValiosoEpico(inventario);
            MostrarEnemigosDerrotadosEpico(derrotados);
            
            // Ranking final
            MostrarRankingFinal(dañoTotal, derrotados.Count, jugador.Nivel);
        }

        private void MostrarEstadisticasDerrotaEpicas(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.WriteLine("                    📊 TU ÚLTIMA BATALLA");
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();

            MostrarStatEpica("🎭 HÉROE CAÍDO", jugador.Nombre, ConsoleColor.DarkYellow);
            Thread.Sleep(200);
            
            MostrarStatConValor("⚔️  DAÑO INFLIGIDO", dañoTotal, ConsoleColor.Red);
            Thread.Sleep(200);
            
            MostrarStatConValor("💀 ENEMIGOS ELIMINADOS", derrotados.Count, ConsoleColor.DarkRed);
            Thread.Sleep(200);
            
            MostrarStatConValor("⭐ NIVEL ALCANZADO", jugador.Nivel, ConsoleColor.DarkYellow);
            Thread.Sleep(400);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("    Tu valentía será recordada en las leyendas...");
            Console.ResetColor();
            Console.WriteLine();
        }

        private void MostrarStatEpica(string nombre, string valor, ConsoleColor color)
        {
            Console.Write("    ");
            Console.ForegroundColor = color;
            Console.Write($"{nombre}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(valor);
            Console.ResetColor();
        }

        private void MostrarStatConValor(string nombre, int valor, ConsoleColor color)
        {
            Console.Write("    ");
            Console.ForegroundColor = color;
            Console.Write($"{nombre}: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{valor}");
            Console.ResetColor();
            
            // Mini barra visual
            Console.Write(" [");
            Console.ForegroundColor = color;
            int barras = Math.Min(valor / 10, 20);
            Console.Write(new string('█', barras));
            Console.ResetColor();
            Console.WriteLine("]");
        }

        private void MostrarObjetoMasValiosoEpico(List<Objeto> inventario)
        {
            var objetoMasValioso = inventario.OrderByDescending(o => o.Valor).FirstOrDefault();
            if (objetoMasValioso != null)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("    💎 TESORO MÁS VALIOSO: ");
                Console.ForegroundColor = objetoMasValioso.ObtenerColorRareza();
                Console.Write($"{objetoMasValioso.ObtenerEstrellas()} {objetoMasValioso.Nombre}");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" [Valor: {objetoMasValioso.Valor}]");
                Console.ResetColor();
                Thread.Sleep(200);
            }
        }

        private void MostrarEnemigosDerrotadosEpico(List<Enemigo> derrotados)
        {
            var grupos = derrotados.GroupBy(e => e.Tipo);
            if (grupos.Any())
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("    📋 ENEMIGOS DERROTADOS POR TIPO:");
                Console.ResetColor();
                
                foreach (var g in grupos.OrderByDescending(x => x.Count()))
                {
                    Console.Write("       ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("▸ ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{g.Key}: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{g.Count()}");
                    Console.ResetColor();
                    Thread.Sleep(100);
                }
            }
        }

        private void MostrarRankingFinal(int daño, int enemigos, int nivel)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            
            int puntuacion = daño + (enemigos * 50) + (nivel * 100);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n    ⭐ PUNTUACIÓN FINAL: {puntuacion} puntos");
            Console.ResetColor();
            
            // Ranking
            string ranking = puntuacion switch
            {
                > 2000 => "🏆 LEYENDA INMORTAL",
                > 1500 => "⭐ HÉROE ÉPICO",
                > 1000 => "💪 GUERRERO VALIENTE",
                > 500 => "⚔️  AVENTURERO PROMETEDOR",
                _ => "🗡️  APRENDIZ DETERMINADO"
            };
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"    {ranking}");
            Console.ResetColor();
            
            Console.WriteLine();
        }

        private void EsperarSalida()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("              >>> Presiona cualquier tecla para salir <<<");
            Console.ResetColor();
            
            Console.ReadKey(true);
        }
    }
}