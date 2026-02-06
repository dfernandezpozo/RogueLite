using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Servicio responsable de gestionar toda la lógica de combate del juego.
    /// </summary>
    public class CombatService
    {
        private readonly Random _random = new Random();
        private readonly LootService _lootService;

        public bool EnCombate { get; private set; }
        public Sala SalaActual { get; private set; }
        public List<Enemigo> EnemigosDerrotados { get; private set; } = new();
        public int DañoTotalInfligido { get; private set; }

        public CombatService(LootService lootService)
        {
            _lootService = lootService;
        }

        /// <summary>
        /// Entra en una sala e inicia el combate si hay enemigos.
        /// </summary>
        public void EntrarSala(Sala sala, Personaje jugador)
        {
            SalaActual = sala;
            if (sala.TieneEnemigos())
            {
                EnCombate = true;
            }
        }

        /// <summary>
        /// Ataca a un enemigo específico (consume turno).
        /// </summary>
        public ResultadoTurno AtacarEnemigo(Enemigo enemigoObjetivo, Personaje jugador)
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            if (enemigoObjetivo == null || !SalaActual.Enemigos.Contains(enemigoObjetivo))
                return new ResultadoTurno { Valido = false, Mensaje = "Enemigo no válido" };

            var resultado = new ResultadoTurno { Valido = true };

            // FASE 1: Ataque del jugador
            int daño = jugador.CalcularAtaque();
            enemigoObjetivo.RecibirDaño(daño);
            DañoTotalInfligido += daño;

            resultado.DañoJugador = daño;
            resultado.EnemigoObjetivo = enemigoObjetivo;
            resultado.Mensaje = $"⚔️ Atacaste a {enemigoObjetivo.Nombre} causando {daño} de daño!";

            // Verificar si el enemigo fue derrotado
            if (!enemigoObjetivo.EstaVivo())
            {
                ProcesarEnemigoDerrotado(enemigoObjetivo, jugador, resultado);

                // Si no quedan enemigos, termina el combate
                if (!SalaActual.TieneEnemigos())
                {
                    EnCombate = false;
                    resultado.Mensaje += "\n🎉 ¡Sala despejada!";
                    return resultado;
                }
            }

            // FASE 2: Contraataque de TODOS los enemigos vivos
            EjecutarTurnoEnemigos(resultado, jugador);

            return resultado;
        }

        /// <summary>
        /// Usa un objeto del inventario (consume turno).
        /// </summary>
        public ResultadoTurno UsarObjeto(Objeto objeto, Personaje jugador)
        {
            if (objeto == null || !jugador.Inventario.Contains(objeto))
                return new ResultadoTurno { Valido = false, Mensaje = "No tienes ese objeto" };

            if (!objeto.EsConsumible)
                return new ResultadoTurno { Valido = false, Mensaje = "Este objeto no es consumible" };

            var resultado = new ResultadoTurno { Valido = true };
            
            if (ItemEffectProcessor.AplicarEfectoObjeto(objeto, jugador, resultado))
            {
                jugador.Inventario.Remove(objeto);
                resultado.ObjetoUsado = true;

                // Los enemigos atacan después de usar el objeto
                if (EnCombate && SalaActual.TieneEnemigos())
                {
                    EjecutarTurnoEnemigos(resultado, jugador);
                }
            }
            else
            {
                resultado.Valido = false;
                resultado.Mensaje = "No se pudo usar el objeto";
            }

            return resultado;
        }

        /// <summary>
        /// Recoge un objeto de la sala (consume turno si estás en combate).
        /// </summary>
        public ResultadoTurno RecogerObjeto(Objeto objeto, Personaje jugador)
        {
            if (SalaActual == null)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en ninguna sala" };

            if (objeto == null || !SalaActual.Objetos.Contains(objeto))
                return new ResultadoTurno { Valido = false, Mensaje = "Ese objeto no está en la sala" };

            var resultado = new ResultadoTurno { Valido = true };

            // Recoger el objeto
            jugador.Inventario.Add(objeto);
            SalaActual.Objetos.Remove(objeto);
            resultado.Mensaje = $"📦 Recogiste: {objeto.Nombre}";
            resultado.ObjetoRecogido = objeto;

            // Si estás en combate, los enemigos atacan
            if (EnCombate && SalaActual.TieneEnemigos())
            {
                resultado.Mensaje += "\n⚠️ ¡Los enemigos aprovechan tu distracción!";
                EjecutarTurnoEnemigos(resultado, jugador);
            }

            return resultado;
        }

        /// <summary>
        /// Defiende (reduce el daño del próximo turno enemigo).
        /// </summary>
        public ResultadoTurno Defender(Personaje jugador)
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            var resultado = new ResultadoTurno { Valido = true };
            resultado.Mensaje = "🛡️ Te preparas para defender...";

            // Aplicar bendición temporal de defensa
            var bendicionDefensa = new Bendicion
            {
                Nombre = "Postura Defensiva",
                Tipo = "Defensa",
                Valor = 5
            };
            jugador.BendicionesActivas.Add(bendicionDefensa);

            // Los enemigos atacan
            EjecutarTurnoEnemigos(resultado, jugador);

            // Remover la bendición temporal después del turno
            jugador.BendicionesActivas.Remove(bendicionDefensa);

            return resultado;
        }

        /// <summary>
        /// Intenta huir del combate (50% probabilidad, consume turno).
        /// </summary>
        public ResultadoTurno IntentarHuir(Personaje jugador)
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            var resultado = new ResultadoTurno { Valido = true };

            // 50% de probabilidad de huir
            bool huyoExitosamente = _random.Next(100) < 50;

            if (huyoExitosamente)
            {
                resultado.HuyoExitosamente = true;
                resultado.Mensaje = "🏃 ¡Lograste huir del combate!";
                EnCombate = false;
            }
            else
            {
                resultado.Mensaje = "❌ ¡No lograste huir!";
                EjecutarTurnoEnemigos(resultado, jugador);
            }

            return resultado;
        }

        /// <summary>
        /// Salta el turno (todos los enemigos atacan).
        /// </summary>
        public ResultadoTurno PasarTurno(Personaje jugador)
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            var resultado = new ResultadoTurno { Valido = true };
            resultado.Mensaje = "⏭️ Pasas tu turno...";

            EjecutarTurnoEnemigos(resultado, jugador);

            return resultado;
        }

        /// <summary>
        /// Obtiene todos los enemigos vivos en la sala actual.
        /// </summary>
        public List<Enemigo> ObtenerEnemigosVivos()
        {
            if (SalaActual == null)
                return new List<Enemigo>();

            return SalaActual.Enemigos.Where(e => e.EstaVivo()).ToList();
        }

        /// <summary>
        /// Finaliza el combate actual.
        /// </summary>
        public void FinalizarCombate()
        {
            EnCombate = false;
        }

        private void ProcesarEnemigoDerrotado(Enemigo enemigo, Personaje jugador, ResultadoTurno resultado)
        {
            resultado.EnemigoDerrotado = true;
            resultado.Mensaje += $"\n💀 ¡Derrotaste a {enemigo.Nombre}!";

            EnemigosDerrotados.Add(enemigo);
            SalaActual.Enemigos.Remove(enemigo);
            jugador.GanarExperiencia(25);

            // Drop de loot
            var loot = _lootService.GenerarLootDrop();
            if (loot != null)
            {
                resultado.LootObtenido = loot;
                resultado.Mensaje += $"\n✨ ¡Obtuviste: {loot.Nombre}!";
            }
        }

        private void EjecutarTurnoEnemigos(ResultadoTurno resultado, Personaje jugador)
        {
            if (SalaActual == null || !SalaActual.TieneEnemigos())
                return;

            resultado.AtaquesEnemigos = new List<AtaqueEnemigo>();
            int dañoTotalRecibido = 0;

            foreach (var enemigo in SalaActual.Enemigos.Where(e => e.EstaVivo()).ToList())
            {
                int dañoBase = enemigo.Ataque;
                int dañoRecibido = Math.Max(1, dañoBase - jugador.CalcularDefensa());
                jugador.RecibirDaño(dañoRecibido);

                dañoTotalRecibido += dañoRecibido;

                resultado.AtaquesEnemigos.Add(new AtaqueEnemigo
                {
                    Enemigo = enemigo,
                    Daño = dañoRecibido
                });
            }

            resultado.DañoEnemigo = dañoTotalRecibido;
            FormatearMensajeAtaquesEnemigos(resultado, dañoTotalRecibido);

            if (!jugador.EstaVivo())
            {
                resultado.Mensaje += "\n☠️ ¡Has sido derrotado!";
                EnCombate = false;
            }
        }

        private void FormatearMensajeAtaquesEnemigos(ResultadoTurno resultado, int dañoTotal)
        {
            if (resultado.AtaquesEnemigos.Count == 1)
            {
                resultado.Mensaje += $"\n💥 {resultado.AtaquesEnemigos[0].Enemigo.Nombre} te ataca causando {resultado.AtaquesEnemigos[0].Daño} de daño";
            }
            else if (resultado.AtaquesEnemigos.Count > 1)
            {
                resultado.Mensaje += $"\n💥 Los enemigos atacan causando {dañoTotal} de daño total:";
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    resultado.Mensaje += $"\n   • {ataque.Enemigo.Nombre}: {ataque.Daño} de daño";
                }
            }
        }
    }
}
