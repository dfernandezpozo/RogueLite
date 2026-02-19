using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de selección de personaje 
    /// </summary>
    public class CharacterSelectionScreen
    {
        private readonly TextAnimator _animator;

        public CharacterSelectionScreen()
        {
            _animator = new TextAnimator();
        }

        public Personaje Mostrar(List<Personaje> personajes)
        {
            Console.Clear();
            Console.CursorVisible = false;
            
            MostrarTituloEpico();
            MostrarPersonajesConEstilo(personajes);
            
            Console.CursorVisible = true;
            var seleccion = SolicitarSeleccion(personajes.Count);
            var personajeSeleccionado = personajes[seleccion - 1];
            
            MostrarConfirmacionEpica(personajeSeleccionado);
            
            return personajeSeleccionado;
        }

        private void MostrarTituloEpico()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════════════╗
    ║                                                       ║
    ║     ⚔️  ═══════ SELECCIÓN DE HÉROE ═══════  ⚔️       ║
    ║                                                       ║
    ║           Elige sabiamente tu destino...              ║
    ║                                                       ║
    ╚═══════════════════════════════════════════════════════╝
");
            Console.ResetColor();
            Thread.Sleep(400);
        }

        private void MostrarPersonajesConEstilo(List<Personaje> personajes)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            Console.WriteLine();

            
            personajes
                .Select((personaje, index) => new { Personaje = personaje, Index = index })
                .ToList()
                .ForEach(item =>
                {
                    MostrarPersonajeEpico(item.Index + 1, item.Personaje);
                    
                    if (item.Index < personajes.Count - 1)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("    ┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄");
                        Console.ResetColor();
                    }
                    
                    Thread.Sleep(150);
                });
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
        }

        private void MostrarPersonajeEpico(int numero, Personaje personaje)
        {
            // Número y marco
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"    ╔═══ [{numero}] ═══════════════════════════════════════╗");
            Console.ResetColor();
            
            // Nombre del personaje con efecto
            Console.Write("    ║  ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"⚔️  {personaje.Nombre.ToUpper()}");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($" - {personaje.Tipo}".PadRight(40) + "║");
            Console.ResetColor();
            
            // Separador
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("    ║  ┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈┈  ║");
            Console.ResetColor();
            
            // Stats con barras visuales
            Console.Write("    ║  ");
            MostrarStatConBarra("❤️  VIDA", personaje.VidaMaxima, 150, ConsoleColor.Red);
            Console.WriteLine("  ║");
            
            Console.Write("    ║  ");
            MostrarStatConBarra("⚔️  ATAQUE", personaje.Ataque, 20, ConsoleColor.Yellow);
            Console.WriteLine("  ║");
            
            // Marco inferior
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    ╚═══════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private void MostrarStatConBarra(string nombre, int valor, int valorMax, ConsoleColor color)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{nombre}: ");
            Console.ForegroundColor = color;
            Console.Write($"{valor}".PadLeft(3));
            Console.ResetColor();
            
            // Barra visual
            Console.Write(" [");
            int barLength = 20;
            int filled = (int)((double)valor / valorMax * barLength);
            
            Console.ForegroundColor = color;
            Console.Write(new string('█', Math.Min(filled, barLength)));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', Math.Max(0, barLength - filled)));
            Console.ResetColor();
            Console.Write("]");
        }

        private int SolicitarSeleccion(int maxPersonajes)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"    ⚡ Selecciona tu héroe (1-{maxPersonajes}): ");
            Console.ForegroundColor = ConsoleColor.White;

            int seleccion;
            while (!int.TryParse(Console.ReadLine(), out seleccion) || seleccion < 1 || seleccion > maxPersonajes)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"    ❌ Opción inválida. Elige entre 1 y {maxPersonajes}: ");
                Console.ForegroundColor = ConsoleColor.White;
            }

            Console.ResetColor();
            return seleccion;
        }

        private void MostrarConfirmacionEpica(Personaje personaje)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            
            // Efecto de confirmación
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ═══════════════════════════════════════════════════════");
            Console.ResetColor();
            
            Thread.Sleep(200);
            
            Console.ForegroundColor = ConsoleColor.Green;
            _animator.AnimarTexto($"              ✓ Has elegido a {personaje.Nombre}!", ConsoleColor.Green, 20);
            Console.ResetColor();
            
            Thread.Sleep(300);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            _animator.AnimarTexto("              Preparando tu aventura...", ConsoleColor.Yellow, 30);
            Console.ResetColor();
            
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ═══════════════════════════════════════════════════════");
            Console.ResetColor();
            
           
            Console.WriteLine();
            Console.Write("    ");
            Enumerable.Range(0, 50)
                .ToList()
                .ForEach(i =>
                {
                    Console.ForegroundColor = i < 25 ? ConsoleColor.DarkCyan : ConsoleColor.Cyan;
                    Console.Write("▓");
                    Thread.Sleep(30);
                });
            Console.ResetColor();
            Console.WriteLine("\n");
            
            Thread.Sleep(500);
        }
    }
}