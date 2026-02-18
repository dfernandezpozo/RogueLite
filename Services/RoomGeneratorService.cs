using System;
using System.Collections.Generic;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Servicio responsable de generar salas del juego.
    /// </summary>
    public class RoomGeneratorService
    {
        private readonly Random _random = new Random();
        private readonly DataLoaderService _dataLoader;
        private readonly LootService _lootService;

        private static readonly string[] DESCRIPCIONES_SALAS = new[]
        {
            "Una sala oscura con antorchas parpadeantes",
            "Un corredor estrecho con ecos lejanos",
            "Una cámara amplia con pilares antiguos",
            "Un pasillo húmedo con musgos en las paredes",
            "La sala final, un gran salón amenazante"
        };

        public RoomGeneratorService(DataLoaderService dataLoader, LootService lootService)
        {
            _dataLoader = dataLoader;
            _lootService = lootService;
        }

        /// <summary>
        /// Genera una cantidad específica de salas.
        /// </summary>
        public List<Sala> GenerarSalas(int cantidad)
        {
            var salas = new List<Sala>();

            for (int i = 1; i <= cantidad; i++)
            {
                // La última sala es una sala de boss
                if (i == cantidad)
                {
                    salas.Add(GenerarSalaBoss(i));
                }
                else
                {
                    salas.Add(GenerarSalaNormal(i));
                }
            }

            return salas;
        }

        /// <summary>
        /// Genera una sala normal con enemigos regulares.
        /// </summary>
        private Sala GenerarSalaNormal(int id)
        {
            return new Sala
            {
                Id = id,
                Nombre = $"Sala {id}",
                Descripcion = ObtenerDescripcionSala(id),
                Enemigos = GenerarEnemigosParaSala(),
                Objetos = GenerarObjetosParaSala()
            };
        }

        /// <summary>
        /// Genera una sala con un boss como enemigo final.
        /// </summary>
        private Sala GenerarSalaBoss(int id)
        {
            var boss = _dataLoader.ObtenerBossAleatorio();
            
            if (boss == null)
            {
                // Fallback: si no hay bosses disponibles, genera sala normal
                Console.WriteLine("⚠️  No hay bosses disponibles, generando sala normal");
                return GenerarSalaNormal(id);
            }

            return new Sala
            {
                Id = id,
                Nombre = $"🔥 SALA DEL JEFE FINAL",
                Descripcion = $"Una presencia aterradora llena la sala... {boss.Descripcion}",
                Enemigos = new List<Enemigo> { boss },
                Objetos = new List<Objeto>() // Los bosses no tienen objetos en el suelo
            };
        }

        /// <summary>
        /// Obtiene la descripción de una sala basada en su ID.
        /// </summary>
        private string ObtenerDescripcionSala(int id)
        {
            return id <= DESCRIPCIONES_SALAS.Length 
                ? DESCRIPCIONES_SALAS[id - 1] 
                : "Una sala misteriosa";
        }

        /// <summary>
        /// Genera enemigos aleatorios para una sala.
        /// </summary>
        private List<Enemigo> GenerarEnemigosParaSala()
        {
            int cantidad = _random.Next(1, 4);
            return _dataLoader.ObtenerEnemigosAleatorios(cantidad);
        }

        /// <summary>
        /// Genera objetos aleatorios para una sala.
        /// </summary>
        private List<Objeto> GenerarObjetosParaSala()
        {
            int cantidad = _random.Next(0, 3);
            return _lootService.ObtenerObjetosAleatorios(cantidad);
        }
    }
}