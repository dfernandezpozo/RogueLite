using System;
using System.Threading;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Renderers
{
    /// <summary>
    /// Renderiza acciones de combate en la interfaz.
    /// </summary>
    public class CombatRenderer
    {
        private readonly TextAnimator _animator;
        private readonly HealthBarRenderer _healthBar;
        private readonly DamageRenderer _damageRenderer;

        public CombatRenderer()
        {
            _animator = new TextAnimator();
            _healthBar = new HealthBarRenderer();
            _damageRenderer = new DamageRenderer();
        }

        public void MostrarAtaqueJugador(Enemigo enemigo, int daño, bool derrotado)
        {
            Console.WriteLine();
            _animator.AnimarTexto($"⚔️  Atacas a {enemigo.Nombre}!", ConsoleColor.Yellow, 30);
            Thread.Sleep(300);
            
            _damageRenderer.MostrarDaño(daño, true);

            if (derrotado)
            {
                Thread.Sleep(400);
                _animator.AnimarTexto($"\n💀 {enemigo.Nombre} ha sido derrotado!", ConsoleColor.Green);
            }
            else
            {
                Thread.Sleep(200);
                _healthBar.Mostrar(enemigo.Vida, enemigo.VidaMaxima);
            }
        }

        public void MostrarAtaqueEnemigo(Enemigo enemigo, int daño)
        {
            Thread.Sleep(500);
            Console.WriteLine();
            _animator.AnimarTexto($"\n🗡️  {enemigo.Nombre} contraataca!", ConsoleColor.Red, 30);
            Thread.Sleep(300);
            _damageRenderer.MostrarDaño(daño, false);
        }
    }
}
