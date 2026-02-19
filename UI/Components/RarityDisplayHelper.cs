using System;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.UI
{
    /// <summary>
    /// Métodos de extensión para mostrar objetos con formato de rareza.
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
            
            inventario
                .Select((obj, index) => new { Objeto = obj, Index = index })
                .ToList()
                .ForEach(item =>
                {
                    Console.Write($"{item.Index + 1}. ");
                    MostrarNombreConColor(item.Objeto);
                    Console.WriteLine($" ({item.Objeto.Tipo})");
                    
                    if (!string.IsNullOrEmpty(item.Objeto.Efecto))
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"   {item.Objeto.Efecto}");
                        Console.ResetColor();
                    }
                });
        }
    }
}