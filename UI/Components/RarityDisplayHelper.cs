using System;
using RogueLite.Models;

namespace RogueLite.UI
{
    /// <summary>
    /// Métodos de extensión para mostrar objetos con formato de rareza.
    /// Añade esto a tu MessageRenderer o crea un nuevo archivo.
    /// </summary>
    public static class RarityDisplayHelper
    {
        /// <summary>
        /// Muestra un mensaje de loot obtenido con colores de rareza.
        /// </summary>
        public static void MostrarLootConRareza(Objeto loot)
        {
            if (loot == null) return;

            Console.WriteLine();
            Console.ForegroundColor = loot.ObtenerColorRareza();
            
            string mensaje = $"✨ ¡LOOT! {loot.ObtenerEstrellas()} {loot.Nombre}";
            
            Console.WriteLine(mensaje);
            Console.ResetColor();
            
            if (!string.IsNullOrEmpty(loot.Efecto))
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine($"   {loot.Efecto}");
                Console.ResetColor();
            }
            
            System.Threading.Thread.Sleep(500);
        }

        /// <summary>
        /// Muestra el nombre de un objeto con su color de rareza.
        /// </summary>
        public static void MostrarNombreConColor(Objeto objeto)
        {
            Console.ForegroundColor = objeto.ObtenerColorRareza();
            Console.Write(objeto.ObtenerNombreFormateado());
            Console.ResetColor();
        }

        /// <summary>
        /// Muestra una lista de objetos con sus colores de rareza.
        /// </summary>
        public static void MostrarInventarioConRareza(System.Collections.Generic.List<Objeto> inventario)
        {
            for (int i = 0; i < inventario.Count; i++)
            {
                var obj = inventario[i];
                Console.Write($"{i + 1}. ");
                MostrarNombreConColor(obj);
                Console.WriteLine($" ({obj.Tipo})");
                
                if (!string.IsNullOrEmpty(obj.Efecto))
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"   {obj.Efecto}");
                    Console.ResetColor();
                }
            }
        }
    }
}