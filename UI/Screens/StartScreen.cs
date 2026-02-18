using System;
using System.Threading;
using RogueLite.UI.Components;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de inicio del juego - Rediseño ÉPICO.
    /// </summary>
    public class StartScreen
    {
        private readonly TextAnimator _animator;

        public StartScreen()
        {
            _animator = new TextAnimator();
        }

        public void Mostrar()
        {
            Console.Clear();
            Console.CursorVisible = false;

            // Efecto de fade in
            MostrarEfectoInicial();
            
            // Logo principal con ASCII art
            MostrarLogoEpico();
            
            // Detalles y créditos
            MostrarDetalles();
            
            // Prompt animado
            MostrarPromptInicio();
            
            Console.ReadKey(true);
            Console.CursorVisible = true;
        }

        private void MostrarEfectoInicial()
        {
            // Efecto de "escaneo" inicial
            for (int i = 0; i < 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("\n    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
                Thread.Sleep(50);
            }
            Console.Clear();
        }

        private void MostrarLogoEpico()
        {
            // Título principal con efecto de aparición letra por letra
            string[] logo = new[]
            {
                "",
                "    ╔══════════════════════════════════════════════════════╗",
                "    ║                                                      ║",
                "    ║     ███╗   ███╗ █████╗ ███████╗███╗   ███╗ █████╗   ║",
                "    ║     ████╗ ████║██╔══██╗╚══███╔╝████╗ ████║██╔══██╗  ║",
                "    ║     ██╔████╔██║███████║  ███╔╝ ██╔████╔██║███████║  ║",
                "    ║     ██║╚██╔╝██║██╔══██║ ███╔╝  ██║╚██╔╝██║██╔══██║  ║",
                "    ║     ██║ ╚═╝ ██║██║  ██║███████╗██║ ╚═╝ ██║██║  ██║  ║",
                "    ║     ╚═╝     ╚═╝╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝╚═╝  ╚═╝  ║",
                "    ║                                                      ║",
                "    ║              ⚔️  DEL DESTINO  ⚔️                      ║",
                "    ║                                                      ║",
                "    ╚══════════════════════════════════════════════════════╝",
                ""
            };

            foreach (var linea in logo)
            {
                if (linea.Contains("███") || linea.Contains("╗") || linea.Contains("╝"))
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                }
                else if (linea.Contains("⚔️") || linea.Contains("DESTINO"))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                }
                
                Console.WriteLine(linea);
                Thread.Sleep(30);
            }
            Console.ResetColor();
        }

        private void MostrarDetalles()
        {
            Thread.Sleep(200);
            
            // Subtítulo con efecto
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ═══════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.White;
            _animator.AnimarTexto("              Un Roguelite de Aventuras Épicas", ConsoleColor.White, 15);
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ═══════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Thread.Sleep(300);
            
            // Características con íconos
            Console.WriteLine();
            string[] features = new[]
            {
                "    ⚔️  Combate estratégico por turnos",
                "    🎲 Generación procedural de mazmorras",
                "    ✨ Sistema de bendiciones y mejoras",
                "    💀 Jefes épicos con habilidades únicas",
                "    🏪 Comercio y gestión de recursos"
            };

            foreach (var feature in features)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(feature);
                Thread.Sleep(100);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private void MostrarPromptInicio()
        {
            Thread.Sleep(500);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ═══════════════════════════════════════════════════════");
            Console.ResetColor();
            
            // Efecto de parpadeo en el prompt
            for (int i = 0; i < 3; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("              >>> Presiona cualquier tecla para comenzar <<<");
                Thread.Sleep(400);
                
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("                                                              ");
                Thread.Sleep(300);
            }
            
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("              >>> Presiona cualquier tecla para comenzar <<<");
            Console.ResetColor();
        }
    }
}