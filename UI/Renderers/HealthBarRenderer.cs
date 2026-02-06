using System;

namespace RogueLite.UI.Components
{
    /// <summary>
    /// Renderiza barras de vida visuales.
    /// </summary>
    public class HealthBarRenderer
    {
        private const int ANCHO_BARRA = 20;

        public void Mostrar(int vidaActual, int vidaMaxima)
        {
            int vidaLlena = CalcularBarraLlena(vidaActual, vidaMaxima);
            var color = ObtenerColorVida(vidaActual, vidaMaxima);

            Console.ForegroundColor = color;
            Console.Write("❤️  [");
            Console.Write(new string('█', vidaLlena));
            
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', ANCHO_BARRA - vidaLlena));
            Console.ResetColor();
            
            Console.Write($"] {vidaActual}/{vidaMaxima}");
        }

        private int CalcularBarraLlena(int vidaActual, int vidaMaxima)
        {
            int vidaLlena = (int)((double)vidaActual / vidaMaxima * ANCHO_BARRA);
            return Math.Clamp(vidaLlena, 0, ANCHO_BARRA);
        }

        private ConsoleColor ObtenerColorVida(int vidaActual, int vidaMaxima)
        {
            if (vidaActual > vidaMaxima * 0.5)
                return ConsoleColor.Green;
            
            if (vidaActual > vidaMaxima * 0.25)
                return ConsoleColor.Yellow;
            
            return ConsoleColor.Red;
        }
    }
}
