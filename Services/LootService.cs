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
        private const int PROBABILIDAD_DROP = 100;

        private List<Objeto> _lootDisponible = new();

        /// <summary>
        /// Inicializa el servicio con el loot disponible.
        /// </summary>
        public void InicializarLoot(List<Objeto> lootDisponible)
        {
            _lootDisponible = lootDisponible ?? new List<Objeto>();
        }

        /// <summary>
        /// Genera un drop de loot aleatorio basado en probabilidad y rareza.
        /// </summary>
        /// <returns>Un objeto si se genera loot, null en caso contrario.</returns>
        public Objeto GenerarLootDrop()
        {
            if (!_lootDisponible.Any())
                return null;

            if (_random.Next(100) < PROBABILIDAD_DROP)
            {
                
                var rareza = DeterminarRareza();
                return GenerarObjetoPorRareza(rareza);
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



        /// <summary>
        /// Determina la rareza del objeto a generar basándose en probabilidades.
        /// </summary>
        /// <returns>Rareza determinada</returns>
        private Rareza DeterminarRareza()
        {
            int roll = _random.Next(100);

            if (roll < 60) return Rareza.Comun;      // 60%
            if (roll < 90) return Rareza.Raro;       // 30%
            if (roll < 98) return Rareza.Epico;      // 8%
            return Rareza.Legendario;                // 2%
        }

        /// <summary>
        /// Genera un objeto de la rareza especificada.
        /// </summary>
        private Objeto GenerarObjetoPorRareza(Rareza rareza)
        {
            var objetosDeRareza = _lootDisponible
                .Where(o => o.Rareza == rareza)
                .ToList();

            if (objetosDeRareza.Count == 0)
            {
                //  si no hay objetos de esa rareza, devolver cualquiera
                return _lootDisponible.OrderBy(_ => _random.Next()).FirstOrDefault();
            }

            return objetosDeRareza.OrderBy(_ => _random.Next()).First();
        }

        /// <summary>
        /// Genera un objeto garantizado de una rareza mínima (útil para bosses).
        /// </summary>
        public Objeto GenerarLootGarantizado(Rareza rarezaMinima = Rareza.Raro)
        {
            if (!_lootDisponible.Any())
                return null;

            var objetosElegibles = _lootDisponible
                .Where(o => o.Rareza >= rarezaMinima)
                .ToList();

            if (objetosElegibles.Count == 0)
            {
                //  devolver el mejor objeto disponible
                return _lootDisponible
                    .OrderByDescending(o => o.Rareza)
                    .FirstOrDefault();
            }

            return objetosElegibles.OrderBy(_ => _random.Next()).First();
        }
    }
}