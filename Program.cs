using System;
using RogueLite.Manager;
using RogueLite.UI;
using RogueLite.Controllers;
using RogueLite.Models;
namespace RogueLite
{
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var gameManager = new GameManager();
            var uiManager = new UIManager();
            var inputHandler = new InputHandler(gameManager, uiManager);

            // Inicializar juego
            uiManager.MostrarPantallaInicio();
            gameManager.InicializarJuego();
            gameManager.CargarPersonajes();

            // Selección de personaje
            var personajes = gameManager.ObtenerPersonajesDisponibles();
            var personajeSeleccionado = uiManager.MostrarSeleccionPersonaje(personajes);
            gameManager.SeleccionarPersonaje(personajeSeleccionado);

            // Game loop principal
            foreach (var sala in gameManager.Salas)
            {
                if (!gameManager.Jugador.EstaVivo())
                    break;

                // *** NUEVO: Tienda cada 3 salas (antes de sala 3) ***
                int indiceSala = gameManager.Salas.IndexOf(sala);
                if (indiceSala > 0 && indiceSala % 3 == 0 && indiceSala < gameManager.Salas.Count - 1)
                {
                    Console.Clear();
                    uiManager.MostrarMensaje("\n✨ Un mercader errante aparece en tu camino...", ConsoleColor.Yellow);
                    System.Threading.Thread.Sleep(1500);
                    
                    var tiendaController = new TiendaController(gameManager, uiManager);
                    tiendaController.AbrirTienda();
                }

                // Entrar a la sala
                gameManager.EntrarSala(sala);
                uiManager.MostrarTransicionSala(sala);

                // FASE 1: Combate
                EjecutarFaseCombate(sala, gameManager, uiManager, inputHandler);

                // FASE 2: Exploración (solo si ganaste el combate)
                if (!gameManager.EnCombate && gameManager.Jugador.EstaVivo())
                {
                    EjecutarFaseExploracion(sala, gameManager, uiManager, inputHandler);
                }
            }

            // Pantalla final
            MostrarResultadoFinal(gameManager, uiManager);
        }

        static void EjecutarFaseCombate(Sala sala, GameManager gameManager, UIManager uiManager, InputHandler inputHandler)
        {
            while (gameManager.EnCombate && gameManager.Jugador.EstaVivo())
            {
                uiManager.MostrarInterfazJuego(gameManager.Jugador, sala);
                MostrarMenuCombate(sala);

                var accion = Console.ReadKey(true).KeyChar;
                Console.WriteLine();

                inputHandler.ProcesarAccionCombate(accion, sala);
                uiManager.EsperarTecla();
            }
        }

        static void EjecutarFaseExploracion(Sala sala, GameManager gameManager, UIManager uiManager, InputHandler inputHandler)
        {
            bool continuarExploracion = true;
            bool bendicionUsada = false;

            while (continuarExploracion)
            {
                uiManager.MostrarInterfazJuego(gameManager.Jugador, sala);
                MostrarMenuExploracion(sala, bendicionUsada);

                var accion = Console.ReadKey(true).KeyChar;
                Console.WriteLine();

                inputHandler.ProcesarAccionExploracion(accion, sala, ref continuarExploracion, ref bendicionUsada);

                if (continuarExploracion)
                {
                    uiManager.EsperarTecla();
                }
            }

            // Completar sala
            gameManager.CompletarSala(sala);

            if (gameManager.Jugador.SubioNivel())
            {
                uiManager.MostrarSubidaNivel(gameManager.Jugador);
                uiManager.EsperarTecla();
            }
        }

        static void MostrarMenuCombate(Sala sala)
        {
            Console.WriteLine("\n⚔️  --- ACCIONES DE COMBATE ---");
            Console.WriteLine("1. ⚔️  Atacar enemigo");
            Console.WriteLine("2. 🧪 Usar objeto del inventario");
            Console.WriteLine("3. 🛡️  Defender");
            
            if (sala.TieneObjetos())
            {
                Console.WriteLine("4. 📦 Recoger objeto de la sala");
            }
            
            Console.WriteLine("5. 🏃 Intentar huir");
            Console.Write("\nElige tu acción: ");
        }

        static void MostrarMenuExploracion(Sala sala, bool bendicionUsada)
        {
            Console.WriteLine("\n🔍 FASE DE EXPLORACIÓN");

            if (sala.TieneObjetos())
            {
                Console.WriteLine("1. 📦 Recoger objeto");
            }

            if (!bendicionUsada)
            {
                Console.WriteLine("2. ✨ Aplicar bendición");
            }

            Console.WriteLine("3. 🚪 Continuar a siguiente sala");
            Console.Write("\nElige una opción: ");
        }

        static void MostrarResultadoFinal(GameManager gameManager, UIManager uiManager)
        {
            if (gameManager.Victoria())
            {
                uiManager.MostrarVictoria(
                    gameManager.DañoTotal,
                    gameManager.EnemigosDerrotados,
                    gameManager.Jugador.Inventario,
                    gameManager.Jugador
                );
            }
            else
            {
                uiManager.MostrarGameOver(
                    gameManager.DañoTotal,
                    gameManager.EnemigosDerrotados,
                    gameManager.Jugador
                );
            }
        }
    }
}