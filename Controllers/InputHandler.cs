using System;
using System.Collections.Generic;
using System.Linq;
using RogueLite.Manager;
using RogueLite.UI;
using RogueLite.Models;
using RogueLite.Services;

namespace RogueLite.Controllers
{
    /// <summary>
    /// Controlador de entrada. Procesa todas las interacciones del usuario
    /// y delega las acciones a los servicios correspondientes.
    /// </summary>
    public class InputHandler
    {
        private readonly GameManager _gameManager;
        private readonly UIManager _uiManager;

        /// <summary>
        /// Inicializa el controlador con las dependencias necesarias.
        /// </summary>
        /// <param name="gameManager">Gestor principal del estado del juego.</param>
        /// <param name="uiManager">Gestor de la interfaz de usuario.</param>
        public InputHandler(GameManager gameManager, UIManager uiManager)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
        }

        // ═══════════════════════════════════════════════════════════
        // COMBATE
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Procesa la acción de combate seleccionada por el jugador.
        /// </summary>
        /// <param name="accion">Carácter que representa la acción ('1' atacar, '2' usar objeto, '3' defender, '4' recoger, '5' huir).</param>
        /// <param name="sala">Sala actual donde se desarrolla el combate.</param>
        public void ProcesarAccionCombate(char accion, Sala sala)
        {
            switch (accion)
            {
                case '1':
                    ProcesarAtaque();
                    break;
                case '2':
                    ProcesarUsarObjeto();
                    break;
                case '3':
                    ProcesarDefender();
                    break;
                case '4':
                    if (sala.TieneObjetos())
                        ProcesarRecogerObjeto(sala);
                    else
                        _uiManager.MostrarError("Opción no válida");
                    break;
                case '5':
                    ProcesarHuir();
                    break;
                default:
                    _uiManager.MostrarError("Opción no válida");
                    break;
            }
        }

        /// <summary>
        /// Gestiona el flujo de ataque: selecciona un enemigo objetivo,
        /// ejecuta el ataque y muestra el resultado junto a los contraataques.
        /// </summary>
        private void ProcesarAtaque()
        {
            var enemigos = _gameManager.ObtenerEnemigosVivos();

            if (enemigos.Count == 0)
            {
                _uiManager.MostrarAdvertencia("No hay enemigos en esta sala");
                return;
            }

            var enemigoObjetivo = SeleccionarEnemigo(enemigos);
            if (enemigoObjetivo == null) return;

            var resultado = _gameManager.AtacarEnemigo(enemigoObjetivo);

            if (!resultado.Valido)
            {
                _uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            _uiManager.MostrarAtaque(enemigoObjetivo, resultado.DañoJugador, resultado.EnemigoDerrotado);

            if (resultado.LootObtenido != null)
            {
                _uiManager.MostrarLootObtenido(resultado.LootObtenido);
            }

            MostrarContraataques(resultado);
        }

        /// <summary>
        /// Permite al jugador seleccionar y usar un objeto del inventario durante el combate.
        /// Muestra el resultado de uso y procesa los contraataques enemigos.
        /// </summary>
        private void ProcesarUsarObjeto()
        {
            if (!_gameManager.Jugador.Inventario.Any())
            {
                _uiManager.MostrarAdvertencia("No tienes objetos en el inventario");
                return;
            }

            var objeto = SeleccionarObjetoInventario();
            if (objeto == null) return;

            var resultado = _gameManager.UsarObjeto(objeto);

            if (!resultado.Valido)
            {
                _uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            Console.WriteLine($"\n{resultado.Mensaje}");
            MostrarContraataques(resultado);
        }

        /// <summary>
        /// Ejecuta la acción de defensa del jugador y procesa los contraataques enemigos.
        /// </summary>
        private void ProcesarDefender()
        {
            var resultado = _gameManager.Defender();

            if (!resultado.Valido)
            {
                _uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            Console.WriteLine($"\n{resultado.Mensaje}");
            MostrarContraataques(resultado);
        }

        /// <summary>
        /// Permite al jugador recoger un objeto de la sala durante el combate.
        /// Si hay enemigos vivos, estos atacan al jugador mientras recoge.
        /// </summary>
        /// <param name="sala">Sala de la que se recoge el objeto.</param>
        private void ProcesarRecogerObjeto(Sala sala)
        {
            if (!sala.TieneObjetos())
            {
                _uiManager.MostrarAdvertencia("No hay objetos disponibles en la sala");
                return;
            }

            var objeto = SeleccionarObjetoSala(sala);
            if (objeto == null) return;

            var resultado = _gameManager.RecogerObjeto(objeto);

            if (!resultado.Valido)
            {
                _uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            _uiManager.MostrarObjetoRecogido(objeto);

            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine("\n⚠️  ¡Los enemigos aprovechan tu distracción!");
                MostrarContraataques(resultado);
            }
        }

        /// <summary>
        /// Intenta que el jugador huya del combate.
        /// Si falla, los enemigos contraatacan.
        /// </summary>
        private void ProcesarHuir()
        {
            var resultado = _gameManager.IntentarHuir();

            if (!resultado.Valido)
            {
                _uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            if (resultado.HuyoExitosamente)
            {
                _uiManager.MostrarMensaje("\n🏃 ¡Lograste huir del combate!", ConsoleColor.Yellow);
            }
            else
            {
                _uiManager.MostrarMensaje("\n❌ ¡No lograste escapar!", ConsoleColor.Red);
                MostrarContraataques(resultado);
            }
        }

        // ═══════════════════════════════════════════════════════════
        // EXPLORACIÓN
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Procesa la acción de exploración seleccionada por el jugador fuera del combate.
        /// </summary>
        /// <param name="accion">Carácter que representa la acción ('1' recoger objeto, '2' bendición, '3' continuar).</param>
        /// <param name="sala">Sala que se está explorando.</param>
        /// <param name="continuarExploracion">Referencia al flag que controla si el jugador sigue explorando la sala.</param>
        /// <param name="bendicionUsada">Referencia al flag que evita usar más de una bendición por sala.</param>
        public void ProcesarAccionExploracion(char accion, Sala sala, ref bool continuarExploracion, ref bool bendicionUsada)
        {
            switch (accion)
            {
                case '1':
                    if (sala.TieneObjetos())
                    {
                        ProcesarRecogerObjetoExploracion(sala);
                    }
                    else
                    {
                        _uiManager.MostrarAdvertencia("No hay objetos en la sala");
                    }
                    break;

                case '2':
                    if (!bendicionUsada)
                    {
                        ProcesarBendicion();
                        bendicionUsada = true;
                    }
                    else
                    {
                        _uiManager.MostrarAdvertencia("Ya has usado una bendición en la sala, no seas abusón");
                    }
                    break;

                case '3':
                    continuarExploracion = false;
                    break;

                default:
                    _uiManager.MostrarError("Opción no válida");
                    break;
            }
        }

        /// <summary>
        /// Permite al jugador recoger un objeto de la sala durante la exploración,
        /// sin penalización por parte de los enemigos.
        /// </summary>
        /// <param name="sala">Sala de la que se recoge el objeto.</param>
        private void ProcesarRecogerObjetoExploracion(Sala sala)
        {
            var objeto = SeleccionarObjetoSala(sala);
            if (objeto == null) return;

            var resultado = _gameManager.RecogerObjeto(objeto);

            if (resultado.Valido)
            {
                _uiManager.MostrarObjetoRecogido(objeto);
            }
            else
            {
                _uiManager.MostrarError(resultado.Mensaje);
            }
        }

        /// <summary>
        /// Solicita y aplica una bendición aleatoria al jugador.
        /// Solo puede usarse una vez por sala.
        /// </summary>
        private void ProcesarBendicion()
        {
            var bendicion = _gameManager.AplicarBendicion();
            if (bendicion != null)
            {
                _uiManager.MostrarBendicionAplicada(bendicion);
            }
            else
            {
                _uiManager.MostrarAdvertencia("No hay bendiciones disponibles");
            }
        }

        // ═══════════════════════════════════════════════════════════
        // SELECCIÓN (métodos auxiliares)
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Muestra la lista de enemigos vivos y solicita al jugador que elija uno.
        /// Si solo hay un enemigo, lo devuelve directamente sin preguntar.
        /// </summary>
        /// <param name="enemigos">Lista de enemigos disponibles para atacar.</param>
        /// <returns>El enemigo seleccionado, o <c>null</c> si la selección es inválida.</returns>
        private Enemigo? SeleccionarEnemigo(List<Enemigo> enemigos)
        {
            if (enemigos.Count == 1)
            {
                return enemigos[0];
            }

            Console.WriteLine("\n🎯 ¿A qué enemigo atacas?");
            
            var enemigosMostrados = enemigos
                .Select((e, index) => $"{index + 1}. {e.Nombre} (❤️  {e.Vida}/{e.VidaMaxima})")
                .ToList();
            
            enemigosMostrados.ForEach(Console.WriteLine);
            
            Console.Write("\nElige el número: ");

            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= enemigos.Count)
            {
                return enemigos[index - 1];
            }

            _uiManager.MostrarError("Selección inválida");
            return null;
        }

        /// <summary>
        /// Muestra el inventario del jugador y solicita que seleccione un objeto para usar.
        /// </summary>
        /// <returns>El objeto seleccionado, o <c>null</c> si se cancela o la selección es inválida.</returns>
        private Objeto? SeleccionarObjetoInventario()
        {
            Console.WriteLine("\n🎒 TU INVENTARIO:");
            var objetos = _gameManager.Jugador.Inventario;
            
            var objetosMostrados = objetos
                .Select((obj, index) => $"{index + 1}. {obj.Nombre} ({obj.Tipo}) - {(obj.EsConsumible ? "Consumible" : "Equipable")}")
                .ToList();
            
            objetosMostrados.ForEach(Console.WriteLine);
            
            Console.Write("\n¿Qué objeto usas? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                _uiManager.MostrarError("Entrada inválida");
                return null;
            }

            if (index == 0) return null;

            if (index < 1 || index > objetos.Count)
            {
                _uiManager.MostrarError("Selección inválida");
                return null;
            }

            return objetos[index - 1];
        }

        /// <summary>
        /// Muestra los objetos disponibles en la sala y solicita al jugador que elija uno.
        /// </summary>
        /// <param name="sala">Sala cuyos objetos se mostrarán.</param>
        /// <returns>El objeto seleccionado, o <c>null</c> si se cancela o la selección es inválida.</returns>
        private Objeto? SeleccionarObjetoSala(Sala sala)
        {
            Console.WriteLine("\n📦 OBJETOS EN LA SALA:");
            
            var objetosMostrados = sala.Objetos
                .Select((obj, index) => $"{index + 1}. {obj.Nombre}")
                .ToList();
            
            objetosMostrados.ForEach(Console.WriteLine);
            
            Console.Write("\n¿Qué objeto recoges? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                _uiManager.MostrarError("Entrada inválida");
                return null;
            }

            if (index == 0) return null;

            if (index < 1 || index > sala.Objetos.Count)
            {
                _uiManager.MostrarError("Selección inválida");
                return null;
            }

            return sala.Objetos[index - 1];
        }

        // ═══════════════════════════════════════════════════════════
        // HELPERS
        // ═══════════════════════════════════════════════════════════

        /// <summary>
        /// Muestra en pantalla los contraataques enemigos del turno actual.
        /// Si el jugador muere como consecuencia, muestra el mensaje de derrota.
        /// </summary>
        /// <param name="resultado">Resultado del turno que contiene la lista de ataques enemigos.</param>
        private void MostrarContraataques(ResultadoTurno resultado)
        {
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine();
                
                resultado.AtaquesEnemigos
                    .Where(ataque => ataque.Enemigo != null)
                    .ToList()
                    .ForEach(ataque => _uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño));

                if (!_gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    _uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }
    }
}