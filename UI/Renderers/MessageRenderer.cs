using System;
using System.Threading;
using RogueLite.Models;
using RogueLite.UI.Components;

namespace RogueLite.UI.Renderers
{
    /// <summary>
    /// Renderiza mensajes y notificaciones al jugador.
    /// </summary>
    public class MessageRenderer
    {
        private readonly TextAnimator _animator;

        public MessageRenderer()
        {
            _animator = new TextAnimator();
        }

        public void MostrarLootObtenido(Objeto loot)
        {
            Thread.Sleep(300);
            _animator.AnimarTexto($"\n🎁 ¡Loot obtenido! {loot.Nombre} [+{loot.Valor}]", ConsoleColor.Cyan);
        }

        public void MostrarObjetoRecogido(Objeto objeto)
        {
            _animator.AnimarTexto($"\n✓ Has recogido: {objeto.Nombre}", ConsoleColor.Green);
            MostrarMensaje($"  └─ Tipo: {objeto.Tipo} | Poder: +{objeto.Valor}", ConsoleColor.Gray);
        }

        public void MostrarBendicionAplicada(Bendicion bendicion)
        {
            _animator.AnimarTexto($"\n✨ Bendición activada: {bendicion.Nombre}", ConsoleColor.Cyan);
            MostrarMensaje($"  └─ {bendicion.Tipo}: +{bendicion.Valor}", ConsoleColor.Gray);
        }

        public void MostrarSubidaNivel(Personaje jugador)
        {
            Console.WriteLine();
            _animator.AnimarTexto("✨ ¡LEVEL UP! ✨", ConsoleColor.Magenta, 50);
            MostrarMensaje($"  └─ Nivel {jugador.Nivel} | Vida máxima: {jugador.VidaMaxima}", ConsoleColor.Cyan);
            Thread.Sleep(1500);
        }

        public void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {mensaje}");
            Console.ResetColor();
        }

        public void MostrarAdvertencia(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  {mensaje}");
            Console.ResetColor();
        }

        public void EsperarTecla()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  [Presiona cualquier tecla para continuar]");
            Console.ResetColor();
            Console.ReadKey(true);
        }
    }
}
