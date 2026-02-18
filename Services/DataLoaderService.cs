using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Servicio responsable de cargar y gestionar todos los datos del juego desde archivos JSON.
    /// </summary>
    public class DataLoaderService
    {
        private readonly Random _random = new Random();

        private List<Enemigo> _enemigosDisponibles = new();
        private List<Objeto> _lootDisponibles = new();
        private List<Bendicion> _bendicionesDisponibles = new();
        private List<Maldicion> _maldicionesDisponibles = new();
        private List<Boss> _bossesDisponibles = new();

        public IReadOnlyList<Enemigo> EnemigosDisponibles => _enemigosDisponibles.AsReadOnly();
        public IReadOnlyList<Objeto> LootDisponibles => _lootDisponibles.AsReadOnly();
        public IReadOnlyList<Bendicion> BendicionesDisponibles => _bendicionesDisponibles.AsReadOnly();
        public IReadOnlyList<Maldicion> MaldicionesDisponibles => _maldicionesDisponibles.AsReadOnly();
        public IReadOnlyList<Boss> BossesDisponibles => _bossesDisponibles.AsReadOnly();
        
        // Para acceso directo (necesario para la tienda)
        public List<Objeto> Objetos => _lootDisponibles.ToList();

        /// <summary>
        /// Carga todos los datos del juego desde archivos JSON.
        /// </summary>
        public void CargarTodosLosDatos()
        {
            _enemigosDisponibles = CargarJSON<Enemigo>(Path.Combine("Data", "Enemies", "enemies.json"));
            _lootDisponibles = CargarJSON<Objeto>(Path.Combine("Data", "Loot", "loot.json"));
            _bendicionesDisponibles = CargarJSON<Bendicion>(Path.Combine("Data", "Blessings", "blessings.json"));
            _maldicionesDisponibles = CargarJSON<Maldicion>(Path.Combine("Data", "Curses", "curses.json"));
            _bossesDisponibles = CargarJSON<Boss>(Path.Combine("Data", "Bosses", "bosses.json"));

            InicializarEnemigos();
            InicializarBosses();
        }
        

        /// <summary>
        /// Carga personajes desde archivo JSON.
        /// </summary>
        public List<Personaje> CargarPersonajes()
        {
            var personajes = CargarJSON<Personaje>(Path.Combine("Data", "Characters", "characters.json"));
            
            foreach (var personaje in personajes)
            {
                personaje.Inventario = new List<Objeto>();
                personaje.BendicionesActivas = new List<Bendicion>();
            }
            
            return personajes;
        }

        /// <summary>
        /// Obtiene una bendición aleatoria de las disponibles.
        /// </summary>
        public Bendicion ObtenerBendicionAleatoria()
        {
            if (!_bendicionesDisponibles.Any())
                return null;

            return _bendicionesDisponibles.OrderBy(_ => _random.Next()).First();
        }

        /// <summary>
        /// Obtiene enemigos aleatorios para una sala.
        /// </summary>
        public List<Enemigo> ObtenerEnemigosAleatorios(int cantidad)
        {
            if (!_enemigosDisponibles.Any())
                return new List<Enemigo>();

            return _enemigosDisponibles
                .OrderBy(_ => _random.Next())
                .Take(cantidad)
                .Select(e => e.Clone())
                .ToList();
        }

        /// <summary>
        /// Obtiene objetos aleatorios para una sala.
        /// </summary>
        public List<Objeto> ObtenerObjetosAleatorios(int cantidad)
        {
            if (!_lootDisponibles.Any())
                return new List<Objeto>();

            return _lootDisponibles
                .OrderBy(_ => _random.Next())
                .Take(cantidad)
                .ToList();
        }

        /// <summary>
        /// Obtiene un boss aleatorio.
        /// </summary>
        public Boss? ObtenerBossAleatorio()
        {
            if (!_bossesDisponibles.Any())
            {
                Console.WriteLine("⚠️  No hay bosses disponibles");
                return null;
            }

            var boss = _bossesDisponibles.OrderBy(_ => _random.Next()).First();
            return boss.Clone() as Boss;
        }

        private void InicializarEnemigos()
        {
            foreach (var enemigo in _enemigosDisponibles)
            {
                enemigo.VidaMaxima = enemigo.Vida;
            }
        }

        private void InicializarBosses()
        {
            foreach (var boss in _bossesDisponibles)
            {
                boss.VidaMaxima = boss.Vida;
            }
        }

        private List<T> CargarJSON<T>(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"⚠️  ADVERTENCIA: No se encontró el archivo {path}");
                return new List<T>();
            }

            try
            {
                var json = File.ReadAllText(path);
                
                // Opciones para deserializar enums correctamente
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
                };
                
                var data = JsonSerializer.Deserialize<List<T>>(json, options);
                return data ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR al cargar {path}: {ex.Message}");
                return new List<T>();
            }
        }
    }
}