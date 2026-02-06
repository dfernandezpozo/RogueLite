using System;

namespace RogueLite.UI.Components
{
    /// <summary>
    /// Renderiza indicadores de daño en combate.
    /// </summary>
    public class DamageRenderer
    {
        public void MostrarDaño(int cantidad, bool esJugador)
        {
            var color = esJugador ? ConsoleColor.Yellow : ConsoleColor.Red;
            var simbolo = esJugador ? '▼' : '▲';
            
            Console.ForegroundColor = color;
            Console.WriteLine($"  └─ {simbolo} {cantidad} de daño");
            Console.ResetColor();
        }
    }
}
