using System;
using System.Collections.Generic;
using System.Threading;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de selección de personaje.
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
            MostrarTitulo();
            MostrarListaPersonajes(personajes);
            
            var seleccion = SolicitarSeleccion(personajes.Count);
            var personajeSeleccionado = personajes[seleccion - 1];
            
            MostrarConfirmacion(personajeSeleccionado);
            
            return personajeSeleccionado;
        }

        private void MostrarTitulo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║          ⚔️  SELECCIÓN DE HÉROE  ⚔️             ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
            Console.WriteLine("\n  Elige tu clase de héroe:\n");
        }

        private void MostrarListaPersonajes(List<Personaje> personajes)
        {
            for (int i = 0; i < personajes.Count; i++)
            {
                var personaje = personajes[i];
                MostrarPersonaje(i + 1, personaje);
            }
            Console.WriteLine("  ─────────────────────────────────────────────");
        }

        private void MostrarPersonaje(int numero, Personaje personaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write($"  [{numero}] {personaje.Nombre}");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($" ({personaje.Tipo})");
            Console.ResetColor();
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"      ❤️  Vida: {personaje.VidaMaxima}");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  ⚔️  Ataque: {personaje.Ataque}");
            Console.ResetColor();
            Console.WriteLine();
        }

        private int SolicitarSeleccion(int maxPersonajes)
        {
            Console.Write("\n  » Selecciona tu héroe (1-" + maxPersonajes + "): ");

            int seleccion;
            while (!int.TryParse(Console.ReadLine(), out seleccion) || seleccion < 1 || seleccion > maxPersonajes)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"  ❌ Opción inválida. Elige entre 1 y {maxPersonajes}: ");
                Console.ResetColor();
            }

            return seleccion;
        }

        private void MostrarConfirmacion(Personaje personaje)
        {
            Console.WriteLine();
            _animator.AnimarTexto($"  ✓ Has elegido a {personaje.Nombre}!", ConsoleColor.Green);
            Thread.Sleep(1000);
        }
    }
}
