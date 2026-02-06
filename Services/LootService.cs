using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Servicio responsable de gestionar la generación y distribución de loot.
    /// </summary>
    public class LootService
    {
        private readonly Random _random = new Random();
        private const int PROBABILIDAD_DROP = 60;

        private List<Objeto> _lootDisponible = new();

        /// <summary>
        /// Inicializa el servicio con el loot disponible.
        /// </summary>
        public void InicializarLoot(List<Objeto> lootDisponible)
        {
            _lootDisponible = lootDisponible ?? new List<Objeto>();
        }

        /// <summary>
        /// Genera un drop de loot aleatorio basado en probabilidad.
        /// </summary>
        /// <returns>Un objeto si se genera loot, null en caso contrario.</returns>
        public Objeto GenerarLootDrop()
        {
            if (!_lootDisponible.Any())
                return null;

            if (_random.Next(100) < PROBABILIDAD_DROP)
            {
                return _lootDisponible.OrderBy(_ => _random.Next()).First();
            }

            return null;
        }

        /// <summary>
        /// Obtiene una cantidad específica de objetos aleatorios.
        /// </summary>
        public List<Objeto> ObtenerObjetosAleatorios(int cantidad)
        {
            if (!_lootDisponible.Any())
                return new List<Objeto>();

            return _lootDisponible
                .OrderBy(_ => _random.Next())
                .Take(cantidad)
                .ToList();
        }
    }
}
