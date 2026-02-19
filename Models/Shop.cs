using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.Models
{
    /// <summary>
    /// Representa una tienda donde el jugador puede comprar y vender objetos.
    /// </summary>
    public class Tienda
    {
        public List<ObjetoTienda> ObjetosDisponibles { get; set; } = new();

        /// <summary>
        /// Genera el inventario de la tienda con objetos aleatorios.
        /// </summary>
        public void GenerarInventario(List<Objeto> todosLosObjetos, int cantidadObjetos = 4)
        {
            var random = new Random();
            ObjetosDisponibles.Clear();

            if (todosLosObjetos == null || !todosLosObjetos.Any())
            {
                Console.WriteLine("⚠️ No hay objetos disponibles para la tienda");
                return;
            }

            var objetosSeleccionados = todosLosObjetos
                .OrderBy(_ => random.Next())
                .Take(Math.Min(cantidadObjetos, todosLosObjetos.Count));

            foreach (var objeto in objetosSeleccionados)
            {
                ObjetosDisponibles.Add(new ObjetoTienda
                {
                    Objeto = objeto,
                    Precio = CalcularPrecio(objeto)
                });
            }
        }

        /// <summary>
        /// Calcula el precio de un objeto basándose en su rareza y valor.
        /// </summary>
        private int CalcularPrecio(Objeto objeto)
        {
            int precioBase = objeto.Rareza switch
            {
                Rareza.Comun => 20,
                Rareza.Raro => 50,
                Rareza.Epico => 100,
                Rareza.Legendario => 200,
                _ => 20
            };

            
            int ajustePorValor = objeto.Valor * 2;

            return precioBase + ajustePorValor;
        }

        /// <summary>
        /// Calcula el precio de venta de un objeto (mitad del precio de compra).
        /// </summary>
        public int CalcularPrecioVenta(Objeto objeto)
        {
            return Math.Max(1, CalcularPrecio(objeto) / 2);
        }
    }

    /// <summary>
    /// Representa un objeto en la tienda con su precio.
    /// </summary>
    public class ObjetoTienda
    {
        public Objeto Objeto { get; set; } = null!;
        public int Precio { get; set; }
        public bool Vendido { get; set; } = false;
    }
}