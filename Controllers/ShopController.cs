using System;
using System.Linq;
using RogueLite.Models;
using RogueLite.Manager;
using RogueLite.UI;

namespace RogueLite.Controllers
{
    /// <summary>
    /// Controlador de la tienda. Maneja las interacciones de compra y venta.
    /// </summary>
    public class TiendaController
    {
        private readonly GameManager _gameManager;
        private readonly UIManager _uiManager;
        private readonly Tienda _tienda;

        public TiendaController(GameManager gameManager, UIManager uiManager)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _tienda = new Tienda();
        }

        /// <summary>
        /// Abre la tienda y permite al jugador comprar/vender hasta que decida salir.
        /// </summary>
        public void AbrirTienda()
        {
            // Generar inventario aleatorio de la tienda
            _tienda.GenerarInventario(_gameManager.ObtenerTodosLosObjetos(), 6);

            bool enTienda = true;

            while (enTienda)
            {
                MostrarTienda();

                Console.Write("\n🎯 Elige una opción: ");
                var input = Console.ReadKey(true).KeyChar;
                Console.WriteLine();

                switch (input)
                {
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                        int index = int.Parse(input.ToString()) - 1;
                        if (index >= 0 && index < _tienda.ObjetosDisponibles.Count)
                        {
                            ComprarObjeto(index);
                        }
                        else
                        {
                            _uiManager.MostrarError("Opción no válida");
                        }
                        break;

                    case '7':
                        VenderObjetos();
                        break;

                    case '8':
                        enTienda = false;
                        Console.WriteLine("\n👋 ¡Gracias por tu visita! Vuelve pronto.");
                        break;

                    default:
                        _uiManager.MostrarError("Opción no válida");
                        break;
                }

                if (enTienda)
                {
                    _uiManager.EsperarTecla();
                }
            }
        }

        private void MostrarTienda()
        {
            Console.Clear();
            
            // Header
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.WriteLine("         🏪 TIENDA DEL MERCADER ERRANTE 🏪");
            Console.WriteLine("═══════════════════════════════════════════════════════");
            Console.ResetColor();

            // Oro del jugador
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n💰 Tu oro: {_gameManager.Jugador.Oro}");
            Console.ResetColor();

            // Objetos disponibles
            Console.WriteLine("\n📦 OBJETOS DISPONIBLES:");
            Console.WriteLine("───────────────────────────────────────────────────────");

            for (int i = 0; i < _tienda.ObjetosDisponibles.Count; i++)
            {
                var item = _tienda.ObjetosDisponibles[i];

                if (item.Vendido)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"{i + 1}. [VENDIDO]");
                    Console.ResetColor();
                }
                else
                {
                    Console.Write($"{i + 1}. ");
                    
                    // Mostrar nombre con color de rareza
                    Console.ForegroundColor = item.Objeto.ObtenerColorRareza();
                    Console.Write($"{item.Objeto.ObtenerEstrellas()} {item.Objeto.Nombre}");
                    Console.ResetColor();
                    
                    // Precio
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($" - {item.Precio} oro");
                    Console.ResetColor();
                    
                    // Efecto
                    if (!string.IsNullOrEmpty(item.Objeto.Efecto))
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine($"   {item.Objeto.Efecto}");
                        Console.ResetColor();
                    }
                }
            }

            // Opciones
            Console.WriteLine("\n───────────────────────────────────────────────────────");
            Console.WriteLine("7. 💸 Vender objetos de tu inventario");
            Console.WriteLine("8. 🚪 Salir de la tienda");
        }

        private void ComprarObjeto(int index)
        {
            if (index < 0 || index >= _tienda.ObjetosDisponibles.Count)
            {
                _uiManager.MostrarError("Selección inválida");
                return;
            }

            var item = _tienda.ObjetosDisponibles[index];

            if (item.Vendido)
            {
                _uiManager.MostrarAdvertencia("Este objeto ya fue vendido");
                return;
            }

            if (!_gameManager.Jugador.PuedeComprar(item.Precio))
            {
                _uiManager.MostrarError($"❌ No tienes suficiente oro (necesitas {item.Precio}, tienes {_gameManager.Jugador.Oro})");
                return;
            }

            // Realizar compra
            _gameManager.Jugador.GastarOro(item.Precio);
            _gameManager.Jugador.Inventario.Add(item.Objeto);
            item.Vendido = true;

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✅ ¡Compraste {item.Objeto.Nombre}!");
            Console.WriteLine($"💰 Oro restante: {_gameManager.Jugador.Oro}");
            Console.ResetColor();
        }

        private void VenderObjetos()
        {
            if (!_gameManager.Jugador.Inventario.Any())
            {
                _uiManager.MostrarAdvertencia("No tienes objetos para vender");
                return;
            }

            Console.WriteLine("\n🎒 TU INVENTARIO:");
            Console.WriteLine("───────────────────────────────────────────────────────");
            
            for (int i = 0; i < _gameManager.Jugador.Inventario.Count; i++)
            {
                var obj = _gameManager.Jugador.Inventario[i];
                int precioVenta = _tienda.CalcularPrecioVenta(obj);
                
                Console.Write($"{i + 1}. ");
                Console.ForegroundColor = obj.ObtenerColorRareza();
                Console.Write($"{obj.Nombre}");
                Console.ResetColor();
                
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($" - {precioVenta} oro");
                Console.ResetColor();
            }

            Console.Write("\n¿Qué objeto vendes? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                _uiManager.MostrarError("Entrada inválida");
                return;
            }

            if (index == 0)
            {
                Console.WriteLine("Venta cancelada");
                return;
            }

            if (index < 1 || index > _gameManager.Jugador.Inventario.Count)
            {
                _uiManager.MostrarError("Selección inválida");
                return;
            }

            var objeto = _gameManager.Jugador.Inventario[index - 1];
            int oroGanado = _tienda.CalcularPrecioVenta(objeto);

            _gameManager.Jugador.Inventario.Remove(objeto);
            _gameManager.Jugador.GanarOro(oroGanado);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n💰 Vendiste {objeto.Nombre} por {oroGanado} oro");
            Console.WriteLine($"💰 Oro total: {_gameManager.Jugador.Oro}");
            Console.ResetColor();
        }
    }
}