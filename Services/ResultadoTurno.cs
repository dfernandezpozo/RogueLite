using System.Collections.Generic;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Representa el resultado de una acción en el combate.
    /// </summary>
    public class ResultadoTurno
    {
        public bool Valido { get; set; }
        public string Mensaje { get; set; } = "";

        // Daño del jugador
        public int DañoJugador { get; set; }
        public Enemigo EnemigoObjetivo { get; set; }
        public bool EnemigoDerrotado { get; set; }

        // Daño de enemigos
        public int DañoEnemigo { get; set; }
        public List<AtaqueEnemigo> AtaquesEnemigos { get; set; } = new();

        // Objetos
        public Objeto LootObtenido { get; set; }
        public bool ObjetoUsado { get; set; }
        public Objeto ObjetoRecogido { get; set; }

        // Otras acciones
        public bool HuyoExitosamente { get; set; }
    }

    /// <summary>
    /// Representa un ataque individual de un enemigo.
    /// </summary>
    public class AtaqueEnemigo
    {
        public Enemigo Enemigo { get; set; }
        public int Daño { get; set; }
    }
}
