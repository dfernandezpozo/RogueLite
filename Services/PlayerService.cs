using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Servicio responsable de gestionar el jugador y la selección de personajes.
    /// </summary>
    public class PlayerService
    {
        private List<Personaje> _personajesDisponibles = new();
        
        public Personaje JugadorActual { get; private set; }

        /// <summary>
        /// Carga los personajes disponibles desde el data loader.
        /// </summary>
        public void CargarPersonajesDisponibles(DataLoaderService dataLoader)
        {
            _personajesDisponibles = dataLoader.CargarPersonajes();
        }

        /// <summary>
        /// Obtiene la lista de personajes disponibles.
        /// </summary>
        public List<Personaje> ObtenerPersonajesDisponibles()
        {
            return _personajesDisponibles;
        }

        /// <summary>
        /// Selecciona un personaje como el jugador actual, creando una copia profunda.
        /// </summary>
        public void SeleccionarPersonaje(Personaje personajeSeleccionado)
        {
            JugadorActual = new Personaje
            {
                Nombre = personajeSeleccionado.Nombre,
                Tipo = personajeSeleccionado.Tipo,
                Vida = personajeSeleccionado.Vida,
                VidaMaxima = personajeSeleccionado.VidaMaxima,
                Ataque = personajeSeleccionado.Ataque,
                Experiencia = 0,
                Nivel = 1,
                Ataques = personajeSeleccionado.Ataques
                    .Select(a => new Ataque
                    {
                        Id = a.Id,
                        Nombre = a.Nombre,
                        Danio = a.Danio,
                        CostoEnergia = a.CostoEnergia,
                        Tipo = a.Tipo,
                        ReduccionDanio = a.ReduccionDanio,
                        AumentoAtaque = a.AumentoAtaque,
                        RecuperaEnergia = a.RecuperaEnergia,
                        AumentoVida = a.AumentoVida
                    })
                    .ToList(),
                Inventario = new List<Objeto>(),
                BendicionesActivas = new List<Bendicion>()
            };
        }
    }
}
