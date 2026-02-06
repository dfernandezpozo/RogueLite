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
    /// Pantalla principal del juego durante el gameplay.
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
            Console.WriteLine("\n\n");
            _animator.AnimarTexto($"    ════════════════════════════════════════", ConsoleColor.DarkMagenta, 10);
            _animator.AnimarTexto($"          {sala.Nombre.ToUpper()}", ConsoleColor.Cyan, 20);
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n    {sala.Descripcion}");
            Console.ResetColor();
            
            _animator.AnimarTexto($"    ════════════════════════════════════════", ConsoleColor.DarkMagenta, 10);
            Thread.Sleep(800);
        }

        public void MostrarInterfaz(Personaje jugador, Sala sala)
        {
            Console.Clear();
            
            _playerRenderer.MostrarHeader(jugador);
            _roomRenderer.MostrarInfoSala(sala);
            _roomRenderer.MostrarEnemigos(sala);
            _roomRenderer.MostrarObjetos(sala);
            _playerRenderer.MostrarInventario(jugador.Inventario);
            _playerRenderer.MostrarBendiciones(jugador.BendicionesActivas);
        }

        public void MostrarMenuAcciones()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────┐");
            Console.WriteLine("│ [1] ⚔️  Atacar    [2] 🎒 Objeto             │");
            Console.WriteLine("│ [3] ✨ Bendición  [4] 🚪 Siguiente sala     │");
            Console.WriteLine("└─────────────────────────────────────────────┘");
            Console.Write("\n» Elige tu acción: ");
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
