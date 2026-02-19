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

            // Ataque del jugador
            int daño = jugador.CalcularAtaque();
            enemigoObjetivo.RecibirDaño(daño);
            DañoTotalInfligido += daño;

            resultado.DañoJugador = daño;
            resultado.EnemigoObjetivo = enemigoObjetivo;
            resultado.Mensaje = $"⚔️ Atacaste a {enemigoObjetivo.Nombre} causando {daño} de daño!";

            // Verificar si el enemigo ha sido derrotado
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

            // Contraataque de TODOS los enemigos vivos
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

            
            int oroGanado;
            if (enemigo is Boss)
            {
                // Bosses dan  más oro
                oroGanado = _random.Next(100, 200);
                resultado.Mensaje += $"\n👑 ¡RECOMPENSA DEL JEFE!";
            }
            else
            {
                // Enemigos normales
                oroGanado = _random.Next(10, 30);
            }
            
            jugador.GanarOro(oroGanado);
            resultado.OroGanado = oroGanado;
            resultado.Mensaje += $"\n💰 +{oroGanado} oro";

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

            
            var ataques = SalaActual.Enemigos
                .Where(e => e.EstaVivo())
                .Select(enemigo =>
                {
                    // Verificar si es un Boss con habilidad
                    if (enemigo is Boss boss)
                    {
                        var habilidad = boss.ObtenerHabilidadParaUsar();
                        
                        if (habilidad != null)
                        {
                            return EjecutarHabilidadBossConRetorno(boss, habilidad, jugador);
                        }
                    }

                    // Ataque normal
                    int dañoBase = enemigo.Ataque;
                    int dañoRecibido = Math.Max(1, dañoBase - jugador.CalcularDefensa());
                    jugador.RecibirDaño(dañoRecibido);

                    return new AtaqueEnemigo
                    {
                        Enemigo = enemigo,
                        Daño = dañoRecibido
                    };
                })
                .ToList();

            resultado.AtaquesEnemigos = ataques;
            int dañoTotalRecibido = ataques.Sum(a => a.Daño);
            resultado.DañoEnemigo = dañoTotalRecibido;
            
            FormatearMensajeAtaquesEnemigos(resultado, dañoTotalRecibido);

            if (!jugador.EstaVivo())
            {
                resultado.Mensaje += "\n☠️ ¡Has sido derrotado!";
                EnCombate = false;
            }
        }

        /// <summary>
        /// Ejecuta una habilidad especial de un boss y devuelve el ataque.
        /// </summary>
        private AtaqueEnemigo EjecutarHabilidadBossConRetorno(Boss boss, HabilidadBoss habilidad, Personaje jugador)
        {
            Console.WriteLine($"\n🔥 ¡{boss.Nombre} usa {habilidad.Nombre}!");
            Console.WriteLine($"   {habilidad.Descripcion}");
            System.Threading.Thread.Sleep(800);
            
            int daño = 0;
            
            // Daño de la habilidad
            if (habilidad.Danio > 0)
            {
                daño = habilidad.EsAreaDanio 
                    ? habilidad.Danio  
                    : Math.Max(1, habilidad.Danio - jugador.CalcularDefensa());
                    
                jugador.RecibirDaño(daño);
            }
            
            // Curación del boss
            if (habilidad.CuracionPropia > 0)
            {
                int vidaAntes = boss.Vida;
                boss.Vida = Math.Min(boss.VidaMaxima, boss.Vida + habilidad.CuracionPropia);
                int vidaCurada = boss.Vida - vidaAntes;
                
                Console.WriteLine($"💚 {boss.Nombre} se cura {vidaCurada} HP!");
                System.Threading.Thread.Sleep(400);
            }

            return new AtaqueEnemigo
            {
                Enemigo = boss,
                Daño = daño,
                EsHabilidadEspecial = true
            };
        }

        private void FormatearMensajeAtaquesEnemigos(ResultadoTurno resultado, int dañoTotal)
        {
            if (resultado.AtaquesEnemigos.Count == 1)
            {
                var ataque = resultado.AtaquesEnemigos[0];
                string tipoAtaque = ataque.EsHabilidadEspecial ? "usa una habilidad especial" : "te ataca";
                resultado.Mensaje += $"\n💥 {ataque.Enemigo.Nombre} {tipoAtaque} causando {ataque.Daño} de daño";
            }
            else if (resultado.AtaquesEnemigos.Count > 1)
            {
                resultado.Mensaje += $"\n💥 Los enemigos atacan causando {dañoTotal} de daño total:";
                
               
                var mensajesAtaques = resultado.AtaquesEnemigos
                    .Select(ataque =>
                    {
                        string tipoAtaque = ataque.EsHabilidadEspecial ? "⚡ Habilidad" : "Ataque";
                        return $"\n   • {ataque.Enemigo.Nombre} ({tipoAtaque}): {ataque.Daño} de daño";
                    });
                
                resultado.Mensaje += string.Join("", mensajesAtaques);
            }
        }
    }
}