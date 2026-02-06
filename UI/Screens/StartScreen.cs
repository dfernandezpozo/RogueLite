using System;
using System.Threading;
using RogueLite.UI.Components;

namespace RogueLite.UI.Screens
{
    /// <summary>
    /// Pantalla de inicio del juego.
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
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║        ⚔️  MAZMORRA DEL DESTINO  ⚔️             ║
    ║                                               ║
    ║          Un Roguelite de Aventuras            ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
            _animator.AnimarTexto("\n    Presiona cualquier tecla para comenzar...", ConsoleColor.Yellow);
            Console.ReadKey(true);
        }
    }
}
