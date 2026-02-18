using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RogueLite.Models;
using RogueLite.UI.Components;
using RogueLite.UI.Renderers;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla principal del juego durante el gameplay - Rediseño ÉPICO.
    /// </summary>
    public class GameScreen
    {
        private readonly TextAnimator _animator;
        private readonly PlayerRenderer _playerRenderer;
        private readonly RoomRenderer _roomRenderer;
        private readonly CombatRenderer _combatRenderer;

        public GameScreen()
        {
            _animator = new TextAnimator();
            _playerRenderer = new PlayerRenderer();
            _roomRenderer = new RoomRenderer();
            _combatRenderer = new CombatRenderer();
        }

        public void MostrarTransicionSala(Sala sala)
        {
            Console.Clear();
            Console.CursorVisible = false;
            
            // Efecto de entrada dramática
            EfectoEntradaSala();
            
            // Título de la sala con marco épico
            MostrarTituloSala(sala);
            
            // Descripción con efecto
            MostrarDescripcionSala(sala);
            
            // Efecto de salida
            EfectoSalidaTransicion();
            
            Console.CursorVisible = true;
        }

        private void EfectoEntradaSala()
        {
            // Cortina de apertura
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine();
            }
            
            for (int i = 0; i < 5; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("    ▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
                Thread.Sleep(40);
            }
            Console.ResetColor();
            Thread.Sleep(200);
            Console.Clear();
        }

        private void MostrarTituloSala(Sala sala)
        {
            Console.WriteLine("\n");
            
            // Marco superior
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("    ╔═══════════════════════════════════════════════════════╗");
            Console.ResetColor();
            
            // Espacio
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("    ║");
            Console.ResetColor();
            Console.Write("                                                       ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("║");
            Console.ResetColor();
            
            // Título central con animación
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("    ║");
            Console.ResetColor();
            
            string titulo = sala.Nombre.ToUpper();
            int padding = (55 - titulo.Length - 6) / 2; // 6 por los iconos
            Console.Write(new string(' ', padding));
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"⚔️  {titulo}  ⚔️");
            Console.ResetColor();
            
            Console.Write(new string(' ', padding));
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("║");
            Console.ResetColor();
            
            Thread.Sleep(300);
            
            // Espacio
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("    ║");
            Console.ResetColor();
            Console.Write("                                                       ");
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("║");
            Console.ResetColor();
            
            // Marco inferior
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("    ╚═══════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private void MostrarDescripcionSala(Sala sala)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("    ");
            _animator.AnimarTexto(sala.Descripcion, ConsoleColor.Gray, 15);
            Console.ResetColor();
            Console.WriteLine("\n");
        }

        private void EfectoSalidaTransicion()
        {
            Thread.Sleep(600);
            
            // Barra de progreso animada
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            
            Thread.Sleep(400);
        }

        public void MostrarInterfaz(Personaje jugador, Sala sala)
        {
            Console.Clear();
            
            // Header con marco decorativo
            MostrarHeaderEpico();
            
            // Información del jugador con estilo
            _playerRenderer.MostrarHeader(jugador);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            
            // Info de la sala
            _roomRenderer.MostrarInfoSala(sala);
            _roomRenderer.MostrarEnemigos(sala);
            _roomRenderer.MostrarObjetos(sala);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("    ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            Console.ResetColor();
            
            // Inventario y bendiciones
            _playerRenderer.MostrarInventario(jugador.Inventario);
            _playerRenderer.MostrarBendiciones(jugador.BendicionesActivas);
        }

        private void MostrarHeaderEpico()
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════════════╗
    ║          ⚔️  MAZMORRA DEL DESTINO  ⚔️                  ║
    ╚═══════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        public void MostrarMenuAcciones()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("    ┌─────────────────────────────────────────────────────┐");
            Console.WriteLine("    │  [1] ⚔️  Atacar      [2] 🎒 Usar Objeto            │");
            Console.WriteLine("    │  [3] 🛡️  Defender     [4] 📦 Recoger               │");
            Console.WriteLine("    │  [5] 🏃 Huir                                        │");
            Console.WriteLine("    └─────────────────────────────────────────────────────┘");
            Console.ResetColor();
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\n    » Tu decisión: ");
            Console.ResetColor();
        }

        public void MostrarAtaque(Enemigo enemigo, int daño, bool derrotado)
        {
            _combatRenderer.MostrarAtaqueJugador(enemigo, daño, derrotado);
        }

        public void MostrarContraataque(Enemigo enemigo, int daño)
        {
            _combatRenderer.MostrarAtaqueEnemigo(enemigo, daño);
        }
    }
}