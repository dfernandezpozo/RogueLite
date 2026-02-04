using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using RogueLite.Models;

namespace RogueLite.Manager
{
    public class GameManager
    {
        private readonly Random random = new Random();

        private List<Enemigo> enemigosDisponibles = new();
        private List<Objeto> lootDisponibles = new();
        private List<Bendicion> bendicionesDisponibles = new();
        private List<Maldicion> maldicionesDisponibles = new();
        private List<Personaje> personajesDisponibles = new();

        public Personaje Jugador { get; private set; } = new();
        public List<Sala> Salas { get; private set; } = new();
        public List<Enemigo> EnemigosDerrotados { get; private set; } = new();
        public int DañoTotal { get; private set; } = 0;

        // Sistema de combate
        public bool EnCombate { get; private set; } = false;
        public Sala SalaActual { get; private set; } = null;

        public void CargarPersonajes()
        {
            personajesDisponibles = CargarJSON<Personaje>(Path.Combine("Data", "Personajes", "personajes.json"));

            foreach (var personaje in personajesDisponibles)
            {
                personaje.Inventario = new List<Objeto>();
                personaje.BendicionesActivas = new List<Bendicion>();
            }
        }

        public List<Personaje> ObtenerPersonajesDisponibles()
        {
            return personajesDisponibles;
        }

        public void SeleccionarPersonaje(Personaje personajeSeleccionado)
        {
            Jugador = new Personaje
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

        public void InicializarJuego()
        {
            CargarDatos();
            GenerarSalas(5);
            EnCombate = false;
        }

        private void CargarDatos()
        {
            enemigosDisponibles = CargarJSON<Enemigo>(Path.Combine("Data", "Enemigos", "enemigos.json"));
            lootDisponibles = CargarJSON<Objeto>(Path.Combine("Data", "Loot", "loot.json"));
            bendicionesDisponibles = CargarJSON<Bendicion>(Path.Combine("Data", "Bendiciones", "bendiciones.json"));
            maldicionesDisponibles = CargarJSON<Maldicion>(Path.Combine("Data", "Maldiciones", "maldiciones.json"));

            foreach (var enemigo in enemigosDisponibles)
            {
                enemigo.VidaMaxima = enemigo.Vida;
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
                var data = JsonSerializer.Deserialize<List<T>>(json);
                return data ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ERROR al cargar {path}: {ex.Message}");
                return new List<T>();
            }
        }

        private void GenerarSalas(int cantidad)
        {
            for (int i = 1; i <= cantidad; i++)
            {
                var sala = new Sala
                {
                    Id = i,
                    Nombre = $"Sala {i}",
                    Descripcion = ObtenerDescripcionSala(i),
                    Enemigos = GenerarEnemigosParaSala(),
                    Objetos = GenerarObjetosParaSala()
                };
                Salas.Add(sala);
            }
        }

        private string ObtenerDescripcionSala(int id)
        {
            var descripciones = new[]
            {
                "Una sala oscura con antorchas parpadeantes",
                "Un corredor estrecho con ecos lejanos",
                "Una cámara amplia con pilares antiguos",
                "Un pasillo húmedo con musgos en las paredes",
                "La sala final, un gran salón amenazante"
            };

            return id <= descripciones.Length ? descripciones[id - 1] : "Una sala misteriosa";
        }

        private List<Enemigo> GenerarEnemigosParaSala()
        {
            if (!enemigosDisponibles.Any())
                return new List<Enemigo>();

            int cantidad = random.Next(1, 4);
            return enemigosDisponibles
                .OrderBy(_ => random.Next())
                .Take(cantidad)
                .Select(e => e.Clone())
                .ToList();
        }

        private List<Objeto> GenerarObjetosParaSala()
        {
            if (!lootDisponibles.Any())
                return new List<Objeto>();

            int cantidad = random.Next(0, 3);
            return lootDisponibles
                .OrderBy(_ => random.Next())
                .Take(cantidad)
                .ToList();
        }

        // ==================== SISTEMA DE COMBATE ROGUELIKE ====================

        /// <summary>
        /// Entra en una sala e inicia el combate si hay enemigos
        /// </summary>
        public void EntrarSala(Sala sala)
        {
            SalaActual = sala;
            if (sala.TieneEnemigos())
            {
                EnCombate = true;
            }
        }

        /// <summary>
        /// Ataca a un enemigo específico (consume turno)
        /// </summary>
        public ResultadoTurno AtacarEnemigo(Enemigo enemigoObjetivo)
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            if (enemigoObjetivo == null || !SalaActual.Enemigos.Contains(enemigoObjetivo))
                return new ResultadoTurno { Valido = false, Mensaje = "Enemigo no válido" };

            var resultado = new ResultadoTurno { Valido = true };

            // FASE 1: Ataque del jugador
            int daño = Jugador.CalcularAtaque();
            enemigoObjetivo.RecibirDaño(daño);
            DañoTotal += daño;

            resultado.DañoJugador = daño;
            resultado.EnemigoObjetivo = enemigoObjetivo;
            resultado.Mensaje = $"⚔️ Atacaste a {enemigoObjetivo.Nombre} causando {daño} de daño!";

            // Verificar si el enemigo fue derrotado
            if (!enemigoObjetivo.EstaVivo())
            {
                resultado.EnemigoDerrotado = true;
                resultado.Mensaje += $"\n💀 ¡Derrotaste a {enemigoObjetivo.Nombre}!";

                EnemigosDerrotados.Add(enemigoObjetivo);
                SalaActual.Enemigos.Remove(enemigoObjetivo);
                Jugador.GanarExperiencia(25);

                // Drop de loot (60% de probabilidad)
                if (random.Next(100) < 60 && lootDisponibles.Any())
                {
                    resultado.LootObtenido = lootDisponibles.OrderBy(_ => random.Next()).First();
                    resultado.Mensaje += $"\n✨ ¡Obtuviste: {resultado.LootObtenido.Nombre}!";
                }

                // Si no quedan enemigos, termina el combate
                if (!SalaActual.TieneEnemigos())
                {
                    EnCombate = false;
                    resultado.Mensaje += "\n🎉 ¡Sala despejada!";
                    return resultado;
                }
            }

            // FASE 2: Contraataque de TODOS los enemigos vivos
            EjecutarTurnoEnemigos(resultado);

            return resultado;
        }

        /// <summary>
        /// Usa un objeto del inventario (consume turno)
        /// </summary>
        public ResultadoTurno UsarObjeto(Objeto objeto)
        {
            if (objeto == null || !Jugador.Inventario.Contains(objeto))
                return new ResultadoTurno { Valido = false, Mensaje = "No tienes ese objeto" };

            if (!objeto.EsConsumible)
                return new ResultadoTurno { Valido = false, Mensaje = "Este objeto no es consumible" };

            var resultado = new ResultadoTurno { Valido = true };
            bool usado = false;

            // Procesar el efecto del objeto
            if (objeto.Tipo == "Poción")
            {
                if (objeto.Nombre.Contains("Vida") || objeto.Nombre.Contains("Curación") ||
                    objeto.Nombre.Contains("Regeneración") || objeto.Nombre.Contains("Completo"))
                {
                    int curacion = objeto.Valor * 10;
                    Jugador.Curar(curacion);
                    resultado.Mensaje = $"🧪 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                    usado = true;
                }
                else if (objeto.Nombre.Contains("Fuerza"))
                {
                    var bendicion = new Bendicion
                    {
                        Nombre = objeto.Nombre,
                        Tipo = "Ataque",
                        Valor = objeto.Valor
                    };
                    Jugador.BendicionesActivas.Add(bendicion);
                    resultado.Mensaje = $"💪 Usaste {objeto.Nombre}. ¡Tu ataque aumentó en {objeto.Valor}!";
                    usado = true;
                }
                else if (objeto.Nombre.Contains("Resistencia"))
                {
                    var bendicion = new Bendicion
                    {
                        Nombre = objeto.Nombre,
                        Tipo = "Defensa",
                        Valor = objeto.Valor
                    };
                    Jugador.BendicionesActivas.Add(bendicion);
                    resultado.Mensaje = $"🛡️ Usaste {objeto.Nombre}. ¡Tu defensa aumentó en {objeto.Valor}!";
                    usado = true;
                }
                else
                {
                    int curacion = objeto.Valor * 5;
                    Jugador.Curar(curacion);
                    resultado.Mensaje = $"🧪 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                    usado = true;
                }
            }
            else if (objeto.Tipo == "Pergamino")
            {
                if (objeto.Nombre.Contains("Curación"))
                {
                    int curacion = objeto.Valor * 10;
                    Jugador.Curar(curacion);
                    resultado.Mensaje = $"📜 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                    usado = true;
                }
                else if (objeto.Nombre.Contains("Fuego") || objeto.Nombre.Contains("Rayo"))
                {
                    var bendicion = new Bendicion
                    {
                        Nombre = objeto.Nombre,
                        Tipo = "Ataque",
                        Valor = objeto.Valor
                    };
                    Jugador.BendicionesActivas.Add(bendicion);
                    resultado.Mensaje = $"📜 Usaste {objeto.Nombre}. ¡Tu ataque mágico aumentó!";
                    usado = true;
                }
            }

            if (usado)
            {
                Jugador.Inventario.Remove(objeto);
                resultado.ObjetoUsado = true;

                // Los enemigos atacan después de usar el objeto
                if (EnCombate && SalaActual.TieneEnemigos())
                {
                    EjecutarTurnoEnemigos(resultado);
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
        /// Recoge un objeto de la sala (consume turno si estás en combate)
        /// </summary>
        public ResultadoTurno RecogerObjeto(Objeto objeto)
        {
            if (SalaActual == null)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en ninguna sala" };

            if (objeto == null || !SalaActual.Objetos.Contains(objeto))
                return new ResultadoTurno { Valido = false, Mensaje = "Ese objeto no está en la sala" };

            var resultado = new ResultadoTurno { Valido = true };

            // Recoger el objeto
            Jugador.Inventario.Add(objeto);
            SalaActual.Objetos.Remove(objeto);
            resultado.Mensaje = $"📦 Recogiste: {objeto.Nombre}";
            resultado.ObjetoRecogido = objeto;

            // Si estás en combate, los enemigos atacan
            if (EnCombate && SalaActual.TieneEnemigos())
            {
                resultado.Mensaje += "\n⚠️ ¡Los enemigos aprovechan tu distracción!";
                EjecutarTurnoEnemigos(resultado);
            }

            return resultado;
        }

        /// <summary>
        /// Defiende (reduce el daño del próximo turno enemigo)
        /// </summary>
        public ResultadoTurno Defender()
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
            Jugador.BendicionesActivas.Add(bendicionDefensa);

            // Los enemigos atacan
            EjecutarTurnoEnemigos(resultado);

            // Remover la bendición temporal después del turno
            Jugador.BendicionesActivas.Remove(bendicionDefensa);

            return resultado;
        }

        /// <summary>
        /// Intenta huir del combate (50% probabilidad, consume turno)
        /// </summary>
        public ResultadoTurno IntentarHuir()
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            var resultado = new ResultadoTurno { Valido = true };

            // 50% de probabilidad de huir
            bool huyoExitosamente = random.Next(100) < 50;

            if (huyoExitosamente)
            {
                resultado.HuyoExitosamente = true;
                resultado.Mensaje = "🏃 ¡Lograste huir del combate!";
                EnCombate = false;
                // Opcional: podrías añadir una penalización aquí
            }
            else
            {
                resultado.Mensaje = "❌ ¡No lograste huir!";
                EjecutarTurnoEnemigos(resultado);
            }

            return resultado;
        }

        /// <summary>
        /// Salta el turno (todos los enemigos atacan)
        /// </summary>
        public ResultadoTurno PasarTurno()
        {
            if (!EnCombate)
                return new ResultadoTurno { Valido = false, Mensaje = "No estás en combate" };

            var resultado = new ResultadoTurno { Valido = true };
            resultado.Mensaje = "⏭️ Pasas tu turno...";

            EjecutarTurnoEnemigos(resultado);

            return resultado;
        }

        /// <summary>
        /// Todos los enemigos vivos atacan al jugador
        /// </summary>
        private void EjecutarTurnoEnemigos(ResultadoTurno resultado)
        {
            if (SalaActual == null || !SalaActual.TieneEnemigos())
                return;

            resultado.AtaquesEnemigos = new List<AtaqueEnemigo>();
            int dañoTotalRecibido = 0;

            foreach (var enemigo in SalaActual.Enemigos.Where(e => e.EstaVivo()).ToList())
            {
                int dañoBase = enemigo.Ataque;
                int dañoRecibido = Math.Max(1, dañoBase - Jugador.CalcularDefensa());
                Jugador.RecibirDaño(dañoRecibido);

                dañoTotalRecibido += dañoRecibido;

                resultado.AtaquesEnemigos.Add(new AtaqueEnemigo
                {
                    Enemigo = enemigo,
                    Daño = dañoRecibido
                });
            }

            resultado.DañoEnemigo = dañoTotalRecibido;

            if (resultado.AtaquesEnemigos.Count == 1)
            {
                resultado.Mensaje += $"\n💥 {resultado.AtaquesEnemigos[0].Enemigo.Nombre} te ataca causando {resultado.AtaquesEnemigos[0].Daño} de daño";
            }
            else if (resultado.AtaquesEnemigos.Count > 1)
            {
                resultado.Mensaje += $"\n💥 Los enemigos atacan causando {dañoTotalRecibido} de daño total:";
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    resultado.Mensaje += $"\n   • {ataque.Enemigo.Nombre}: {ataque.Daño} de daño";
                }
            }

            if (!Jugador.EstaVivo())
            {
                resultado.Mensaje += "\n☠️ ¡Has sido derrotado!";
                EnCombate = false;
            }
        }

        // ==================== MÉTODOS AUXILIARES ====================

        public Bendicion AplicarBendicion()
        {
            if (!bendicionesDisponibles.Any())
                return null;

            var bendicion = bendicionesDisponibles.OrderBy(_ => random.Next()).First();
            Jugador.BendicionesActivas.Add(bendicion);
            return bendicion;
        }

        public void CompletarSala(Sala sala)
        {
            if (sala.EstaLimpia())
            {
                sala.CompletarSala();
                Jugador.GanarExperiencia(50);
                EnCombate = false;
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

        /// <summary>
        /// Obtiene todos los enemigos vivos en la sala actual
        /// </summary>
        public List<Enemigo> ObtenerEnemigosVivos()
        {
            if (SalaActual == null)
                return new List<Enemigo>();

            return SalaActual.Enemigos.Where(e => e.EstaVivo()).ToList();
        }
    }

    // ==================== CLASES DE RESULTADO ====================

    /// <summary>
    /// Representa el resultado de una acción en el combate
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
    /// Representa un ataque individual de un enemigo
    /// </summary>
    public class AtaqueEnemigo
    {
        public Enemigo Enemigo { get; set; }
        public int Daño { get; set; }
    }
}
