using System;
using System.Linq;
using System.Threading;

namespace RogueLite.UI.Components
{
    /// <summary>
    /// Proporciona efectos de animación de texto.
    /// </summary>
    public class TextAnimator
    {
        private const int DELAY_PREDETERMINADO = 20;

        public void AnimarTexto(string texto, ConsoleColor color = ConsoleColor.White, int delay = DELAY_PREDETERMINADO)
        {
            Console.ForegroundColor = color;
            
            
            texto.ToList().ForEach(c =>
            {
                Console.Write(c);
                Thread.Sleep(delay);
            });
            
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}