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

        public InputHandler(GameManager gameManager, UIManager uiManager)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
        }

        // ═══════════════════════════════════════════════════════════
        // COMBATE
        // ═══════════════════════════════════════════════════════════

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

        private Enemigo? SeleccionarEnemigo(List<Enemigo> enemigos)
        {
            if (enemigos.Count == 1)
            {
                return enemigos[0];
            }

            Console.WriteLine("\n🎯 ¿A qué enemigo atacas?");
            for (int i = 0; i < enemigos.Count; i++)
            {
                var e = enemigos[i];
                Console.WriteLine($"{i + 1}. {e.Nombre} (❤️  {e.Vida}/{e.VidaMaxima})");
            }
            Console.Write("\nElige el número: ");

            if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= enemigos.Count)
            {
                return enemigos[index - 1];
            }

            _uiManager.MostrarError("Selección inválida");
            return null;
        }

        private Objeto? SeleccionarObjetoInventario()
        {
            Console.WriteLine("\n🎒 TU INVENTARIO:");
            var objetos = _gameManager.Jugador.Inventario;
            
            for (int i = 0; i < objetos.Count; i++)
            {
                var obj = objetos[i];
                Console.WriteLine($"{i + 1}. {obj.Nombre} ({obj.Tipo}) - {(obj.EsConsumible ? "Consumible" : "Equipable")}");
            }
            
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

        private Objeto? SeleccionarObjetoSala(Sala sala)
        {
            Console.WriteLine("\n📦 OBJETOS EN LA SALA:");
            for (int i = 0; i < sala.Objetos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sala.Objetos[i].Nombre}");
            }
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

        private void MostrarContraataques(ResultadoTurno resultado)
        {
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine();
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    if (ataque.Enemigo != null)
                    {
                        _uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                    }
                }

                if (!_gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    _uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }
    }
}