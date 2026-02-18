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
                case "Consumible":  // ← AÑADIDO
                case "Poción":
                    return ProcesarConsumible(objeto, jugador, resultado);
                case "Pergamino":
                    return ProcesarPergamino(objeto, jugador, resultado);
                default:
                    return false;
            }
        }

        private static bool ProcesarConsumible(Objeto objeto, Personaje jugador, ResultadoTurno resultado)
        {
            // Pociones de salud
            if (objeto.Nombre.Contains("Salud") || objeto.Nombre.Contains("Poción") || 
                objeto.Nombre.Contains("Vida") || objeto.Nombre.Contains("Curación") ||
                objeto.Nombre.Contains("Regeneración") || objeto.Nombre.Contains("Completo"))
            {
                int curacion = objeto.Valor * 3; // Valor * 3 para que sea balanceado
                jugador.Curar(curacion);
                resultado.Mensaje = $"🧪 Usaste {objeto.Nombre} y recuperaste {curacion} de vida";
                return true;
            }

            // Elixir del Titán (legendario especial)
            if (objeto.Nombre.Contains("Elixir") || objeto.Nombre.Contains("Titán"))
            {
                jugador.Curar(jugador.VidaMaxima); // Cura completa
                jugador.Ataque += 10; // Bonus permanente
                resultado.Mensaje = $"✨ ¡Usaste {objeto.Nombre}! Vida restaurada y +10 ATK permanente";
                return true;
            }

            // Antídoto
            if (objeto.Nombre.Contains("Antídoto"))
            {
                int curacion = 20;
                jugador.Curar(curacion);
                resultado.Mensaje = $"💊 Usaste {objeto.Nombre} y te curaste {curacion} HP";
                return true;
            }

            // Pociones de fuerza
            if (objeto.Nombre.Contains("Fuerza"))
            {
                AplicarBendicionAtaque(objeto, jugador);
                resultado.Mensaje = $"💪 Usaste {objeto.Nombre}. ¡Tu ataque aumentó en {objeto.Valor}!";
                return true;
            }

            // Pociones de resistencia/defensa
            if (objeto.Nombre.Contains("Resistencia") || objeto.Nombre.Contains("Defensa"))
            {
                AplicarBendicionDefensa(objeto, jugador);
                resultado.Mensaje = $"🛡️ Usaste {objeto.Nombre}. ¡Tu defensa aumentó en {objeto.Valor}!";
                return true;
            }

            // Consumible genérico - cura basado en valor
            int curacionGenerica = objeto.Valor * 2;
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