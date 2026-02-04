using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.Manager
{
    public class CombateManager
    {
        private readonly Random random = new();
        private List<Enemigo> enemigosDisponibles = new();

        public bool EnCombate { get; private set; } = false;
        public Sala SalaActual { get; private set; }

        public void CargarEnemigos(string path)
        {
            if (!System.IO.File.Exists(path)) return;
            var json = System.IO.File.ReadAllText(path);
            enemigosDisponibles = System.Text.Json.JsonSerializer.Deserialize<List<Enemigo>>(json) ?? new();
            foreach (var e in enemigosDisponibles) e.VidaMaxima = e.Vida;
        }

        public void EntrarSala(Sala sala)
        {
            SalaActual = sala;
            EnCombate = sala.TieneEnemigos();
        }

        public List<Enemigo> GenerarEnemigosParaSala()
        {
            if (!enemigosDisponibles.Any()) return new();
            int cantidad = random.Next(1, 3);
            return enemigosDisponibles.OrderBy(_ => random.Next()).Take(cantidad)
                .Select(e => e.Clone()).ToList();
        }

        public ResultadoTurno Atacar(Personaje jugador, Enemigo enemigo)
        {
            var resultado = new ResultadoTurno { Valido = true };

            int daño = jugador.CalcularAtaque();
            enemigo.RecibirDaño(daño);
            resultado.DañoJugador = daño;
            resultado.EnemigoObjetivo = enemigo;
            resultado.Mensaje = $"Atacaste a {enemigo.Nombre} causando {daño} de daño";

            if (!enemigo.EstaVivo())
            {
                resultado.EnemigoDerrotado = true;
                SalaActual.Enemigos.Remove(enemigo);
                jugador.GanarExperiencia(25);
            }

            EjecutarTurnoEnemigos(jugador, resultado);

            if (!SalaActual.TieneEnemigos()) EnCombate = false;
            return resultado;
        }

        private void EjecutarTurnoEnemigos(Personaje jugador, ResultadoTurno resultado)
        {
            resultado.AtaquesEnemigos = new List<AtaqueEnemigo>();
            foreach (var e in SalaActual.Enemigos.Where(x => x.EstaVivo()))
            {
                int daño = Math.Max(1, e.Ataque - jugador.CalcularDefensa());
                jugador.RecibirDaño(daño);
                resultado.AtaquesEnemigos.Add(new AtaqueEnemigo { Enemigo = e, Daño = daño });
            }
            if (!jugador.EstaVivo()) EnCombate = false;
        }
    }
}
