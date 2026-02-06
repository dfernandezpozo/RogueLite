using System;
using RogueLite.Models;

namespace RogueLite.Services
{
    /// <summary>
    /// Procesador de efectos de objetos consumibles.
    /// </summary>
    public static class ItemEffectProcessor
    {
        /// <summary>
        /// Aplica el efecto de un objeto al jugador.
        /// </summary>
        /// <returns>True si el objeto fue usado exitosamente, false en caso contrario.</returns>
        public static bool AplicarEfectoObjeto(Objeto objeto, Personaje jugador, ResultadoTurno resultado)
        {
            switch (objeto.Tipo)
            {
                case "Poción":
                    return ProcesarPocion(objeto, jugador, resultado);
                case "Pergamino":
                    return ProcesarPergamino(objeto, jugador, resultado);
                default:
                    return false;
            }
        }

        private static bool ProcesarPocion(Objeto objeto, Personaje jugador, ResultadoTurno resultado)
        {
            if (objeto.Nombre.Contains("Vida") || objeto.Nombre.Contains("Curación") ||
                objeto.Nombre.Contains("Regeneración") || objeto.Nombre.Contains("Completo"))
            {
                int curacion = objeto.Valor * 10;
                jugador.Curar(curacion);
                resultado.Mensaje = $"🧪 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                return true;
            }

            if (objeto.Nombre.Contains("Fuerza"))
            {
                AplicarBendicionAtaque(objeto, jugador);
                resultado.Mensaje = $"💪 Usaste {objeto.Nombre}. ¡Tu ataque aumentó en {objeto.Valor}!";
                return true;
            }

            if (objeto.Nombre.Contains("Resistencia"))
            {
                AplicarBendicionDefensa(objeto, jugador);
                resultado.Mensaje = $"🛡️ Usaste {objeto.Nombre}. ¡Tu defensa aumentó en {objeto.Valor}!";
                return true;
            }

            // Poción genérica
            int curacionGenerica = objeto.Valor * 5;
            jugador.Curar(curacionGenerica);
            resultado.Mensaje = $"🧪 Usaste {objeto.Nombre} y recuperaste {curacionGenerica} de vida";
            return true;
        }

        private static bool ProcesarPergamino(Objeto objeto, Personaje jugador, ResultadoTurno resultado)
        {
            if (objeto.Nombre.Contains("Curación"))
            {
                int curacion = objeto.Valor * 10;
                jugador.Curar(curacion);
                resultado.Mensaje = $"📜 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                return true;
            }

            if (objeto.Nombre.Contains("Fuego") || objeto.Nombre.Contains("Rayo"))
            {
                var bendicion = new Bendicion
                {
                    Nombre = objeto.Nombre,
                    Tipo = "Ataque",
                    Valor = objeto.Valor
                };
                jugador.BendicionesActivas.Add(bendicion);
                resultado.Mensaje = $"📜 Usaste {objeto.Nombre}. ¡Tu ataque mágico aumentó!";
                return true;
            }

            return false;
        }

        private static void AplicarBendicionAtaque(Objeto objeto, Personaje jugador)
        {
            var bendicion = new Bendicion
            {
                Nombre = objeto.Nombre,
                Tipo = "Ataque",
                Valor = objeto.Valor
            };
            jugador.BendicionesActivas.Add(bendicion);
        }

        private static void AplicarBendicionDefensa(Objeto objeto, Personaje jugador)
        {
            var bendicion = new Bendicion
            {
                Nombre = objeto.Nombre,
                Tipo = "Defensa",
                Valor = objeto.Valor
            };
            jugador.BendicionesActivas.Add(bendicion);
        }
    }
}
