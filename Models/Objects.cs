using System;

namespace RogueLite.Models
{
    /// <summary>
    /// Enum que define los niveles de rareza de los objetos.
    /// </summary>
    public enum Rareza
    {
        Comun,      // 60% - Gris/Blanco
        Raro,       // 30% - Azul
        Epico,      // 8%  - Morado/Magenta
        Legendario  // 2%  - Dorado/Amarillo
    }

    public class Objeto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Valor { get; set; }
        public bool EsConsumible { get; set; } = false;
        public Rareza Rareza { get; set; } = Rareza.Comun; // ← NUEVO
        public string Efecto { get; set; } = string.Empty; // Descripción del efecto

        /// <summary>
        /// Obtiene el color de consola según la rareza del objeto.
        /// </summary>
        public ConsoleColor ObtenerColorRareza()
        {
            return Rareza switch
            {
                Rareza.Comun => ConsoleColor.Gray,
                Rareza.Raro => ConsoleColor.Cyan,
                Rareza.Epico => ConsoleColor.Magenta,
                Rareza.Legendario => ConsoleColor.Yellow,
                _ => ConsoleColor.White
            };
        }

        /// <summary>
        /// Obtiene las estrellas que representan la rareza.
        /// </summary>
        public string ObtenerEstrellas()
        {
            return Rareza switch
            {
                Rareza.Legendario => "★★★★",
                Rareza.Epico => "★★★",
                Rareza.Raro => "★★",
                Rareza.Comun => "★",
                _ => ""
            };
        }

        /// <summary>
        /// Obtiene el nombre formateado con color y estrellas.
        /// </summary>
        public string ObtenerNombreFormateado()
        {
            return $"{ObtenerEstrellas()} {Nombre}";
        }
    }
}