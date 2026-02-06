using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;
using RogueLite.Services;

namespace RogueLite.Manager
{
    /// <summary>
    /// Orquestador principal del juego. Coordina los diferentes servicios y mantiene el estado global.
    /// </summary>
    public class GameManager
    {
        private readonly DataLoaderService _dataLoader;
        private readonly RoomGeneratorService _roomGenerator;
        private readonly CombatService _combatService;
        private readonly LootService _lootService;
        private readonly PlayerService _playerService;

        public Personaje Jugador => _playerService.JugadorActual;
        public List<Sala> Salas { get; private set; } = new();
        public List<Enemigo> EnemigosDerrotados => _combatService.EnemigosDerrotados;
        public int DañoTotal => _combatService.DañoTotalInfligido;
        public bool EnCombate => _combatService.EnCombate;
        public Sala SalaActual => _combatService.SalaActual;

        public GameManager()
        {
            _dataLoader = new DataLoaderService();
            _lootService = new LootService();
            _playerService = new PlayerService();
            _roomGenerator = new RoomGeneratorService(_dataLoader, _lootService);
            _combatService = new CombatService(_lootService);
        }

        public void CargarPersonajes()
        {
            _playerService.CargarPersonajesDisponibles(_dataLoader);
        }

        public List<Personaje> ObtenerPersonajesDisponibles()
        {
            return _playerService.ObtenerPersonajesDisponibles();
        }

        public void SeleccionarPersonaje(Personaje personajeSeleccionado)
        {
            _playerService.SeleccionarPersonaje(personajeSeleccionado);
        }

        public void InicializarJuego()
        {
            _dataLoader.CargarTodosLosDatos();
            Salas = _roomGenerator.GenerarSalas(5);
            _combatService.FinalizarCombate();
        }

        public void EntrarSala(Sala sala)
        {
            _combatService.EntrarSala(sala, Jugador);
        }

        public ResultadoTurno AtacarEnemigo(Enemigo enemigoObjetivo)
        {
            return _combatService.AtacarEnemigo(enemigoObjetivo, Jugador);
        }

        public ResultadoTurno UsarObjeto(Objeto objeto)
        {
            return _combatService.UsarObjeto(objeto, Jugador);
        }

        public ResultadoTurno RecogerObjeto(Objeto objeto)
        {
            return _combatService.RecogerObjeto(objeto, Jugador);
        }

        public ResultadoTurno Defender()
        {
            return _combatService.Defender(Jugador);
        }

        public ResultadoTurno IntentarHuir()
        {
            return _combatService.IntentarHuir(Jugador);
        }

        public ResultadoTurno PasarTurno()
        {
            return _combatService.PasarTurno(Jugador);
        }

        public Bendicion AplicarBendicion()
        {
            var bendicion = _dataLoader.ObtenerBendicionAleatoria();
            if (bendicion != null)
            {
                Jugador.BendicionesActivas.Add(bendicion);
            }
            return bendicion;
        }

        public void CompletarSala(Sala sala)
        {
            if (sala.EstaLimpia())
            {
                sala.CompletarSala();
                Jugador.GanarExperiencia(50);
                _combatService.FinalizarCombate();
            }
        }

        public bool JuegoTerminado()
        {
            return !Jugador.EstaVivo() || Salas.All(s => s.Completada);
        }

        public bool Victoria()
        {
            return Jugador.EstaVivo() && Salas.All(s => s.Completada);
        }

        public List<Enemigo> ObtenerEnemigosVivos()
        {
            return _combatService.ObtenerEnemigosVivos();
        }
    }
}
