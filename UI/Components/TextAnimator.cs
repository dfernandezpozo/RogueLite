using System;
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
            
            foreach (char c in texto)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            
            Console.ResetColor();
            Console.WriteLine();
        }
    }
}
