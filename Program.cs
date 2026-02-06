using System;
using System.Linq;
using RogueLite.Manager;
using RogueLite.UI;
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

            // Pantalla de inicio
            uiManager.MostrarPantallaInicio();

            // Inicializar juego
            gameManager.InicializarJuego();
            gameManager.CargarPersonajes();
            var personajes = gameManager.ObtenerPersonajesDisponibles();

            // Mostrar selección de personaje
            var personajeSeleccionado = uiManager.MostrarSeleccionPersonaje(personajes);
            gameManager.SeleccionarPersonaje(personajeSeleccionado);

            // Game loop principal
            foreach (var sala in gameManager.Salas)
            {
                if (!gameManager.Jugador.EstaVivo())
                    break;

                // Entrar a la sala e iniciar combate si hay enemigos
                gameManager.EntrarSala(sala);
                uiManager.MostrarTransicionSala(sala);

                // Fase de combate
                while (gameManager.EnCombate && gameManager.Jugador.EstaVivo())
                {
                    uiManager.MostrarInterfazJuego(gameManager.Jugador, sala);
                    MostrarMenuCombate(sala, uiManager);

                    var accion = Console.ReadKey(true).KeyChar;
                    Console.WriteLine();

                    switch (accion)
                    {
                        case '1': // Atacar
                            ProcesarAtaque(sala, gameManager, uiManager);
                            break;

                        case '2': // Usar objeto del inventario
                            ProcesarUsarObjeto(gameManager, uiManager);
                            break;

                        case '3': // Defender
                            ProcesarDefender(gameManager, uiManager);
                            break;

                        case '4': // Recoger objeto (durante combate)
                            ProcesarRecogerObjetoCombate(sala, gameManager, uiManager);
                            break;

                        case '5': // Huir
                            ProcesarHuir(gameManager, uiManager);
                            break;

                        default:
                            uiManager.MostrarError("Opción no válida");
                            break;
                    }

                    uiManager.EsperarTecla();
                }

                // Después del combate: fase de exploración
                if (!gameManager.EnCombate && gameManager.Jugador.EstaVivo())
                {
                    bool continuarExploracion = true;
                    bool bendicionUsada = false;
                    while (continuarExploracion)
                    {
                        uiManager.MostrarInterfazJuego(gameManager.Jugador, sala);
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

                        var accionSala = Console.ReadKey(true).KeyChar;
                        Console.WriteLine();

                        switch (accionSala)
                        {
                            case '1':
                                if (sala.TieneObjetos())
                                {
                                    ProcesarRecogerObjeto(sala, gameManager, uiManager);
                                }
                                else
                                {
                                    uiManager.MostrarAdvertencia("No hay objetos en la sala");
                                }
                                break;
                            case '2':
                                if (!bendicionUsada)
                                {
                                    ProcesarBendicion(gameManager, uiManager);
                                    bendicionUsada = true;
                                }
                                else
                                {
                                    uiManager.MostrarAdvertencia("Ya has usado una bendición en la sala, no seas abusón");
                                }
                                break;
                            case '3':
                                continuarExploracion = false;
                                break;
                            default:
                                uiManager.MostrarError("Opción no válida");
                                break;
                        }

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
            }

            // Mostrar resultado final
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

        static void MostrarMenuCombate(Sala sala, UIManager uiManager)
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

        static void ProcesarAtaque(Sala sala, GameManager gameManager, UIManager uiManager)
        {
            var enemigos = gameManager.ObtenerEnemigosVivos();

            if (enemigos.Count == 0)
            {
                uiManager.MostrarAdvertencia("No hay enemigos en esta sala");
                return;
            }

            // Seleccionar enemigo objetivo
            Enemigo enemigoObjetivo;

            if (enemigos.Count == 1)
            {
                enemigoObjetivo = enemigos[0];
            }
            else
            {
                Console.WriteLine("\n🎯 ¿A qué enemigo atacas?");
                for (int i = 0; i < enemigos.Count; i++)
                {
                    var e = enemigos[i];
                    Console.WriteLine($"{i + 1}. {e.Nombre} (❤️  {e.Vida}/{e.VidaMaxima})");
                }
                Console.Write("\nElige el número: ");

                if (int.TryParse(Console.ReadLine(), out int index) &&
                    index >= 1 && index <= enemigos.Count)
                {
                    enemigoObjetivo = enemigos[index - 1];
                }
                else
                {
                    uiManager.MostrarError("Selección inválida");
                    return;
                }
            }

            // Ejecutar ataque
            var resultado = gameManager.AtacarEnemigo(enemigoObjetivo);

            if (!resultado.Valido)
            {
                uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            // Mostrar resultado del ataque
            uiManager.MostrarAtaque(enemigoObjetivo, resultado.DañoJugador, resultado.EnemigoDerrotado);

            // Mostrar loot si lo obtuvo
            if (resultado.LootObtenido != null)
            {
                uiManager.MostrarLootObtenido(resultado.LootObtenido);
            }

            // Mostrar contraataques de enemigos
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine();
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    if (ataque.Enemigo != null)
                    {
                        uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                    }
                }

                if (!gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }

        static void ProcesarUsarObjeto(GameManager gameManager, UIManager uiManager)
        {
            if (!gameManager.Jugador.Inventario.Any())
            {
                uiManager.MostrarAdvertencia("No tienes objetos en el inventario");
                return;
            }

            Console.WriteLine("\n🎒 TU INVENTARIO:");
            var objetos = gameManager.Jugador.Inventario;
            for (int i = 0; i < objetos.Count; i++)
            {
                var obj = objetos[i];
                Console.WriteLine($"{i + 1}. {obj.Nombre} ({obj.Tipo}) - {(obj.EsConsumible ? "Consumible" : "Equipable")}");
            }
            Console.Write("\n¿Qué objeto usas? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                uiManager.MostrarError("Entrada inválida");
                return;
            }

            if (index == 0)
                return;

            if (index < 1 || index > objetos.Count)
            {
                uiManager.MostrarError("Selección inválida");
                return;
            }

            var objeto = objetos[index - 1];
            var resultado = gameManager.UsarObjeto(objeto);

            if (!resultado.Valido)
            {
                uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            // Mostrar efecto del objeto
            Console.WriteLine($"\n{resultado.Mensaje}");

            // Mostrar contraataques de enemigos
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine();
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    if (ataque.Enemigo != null)
                    {
                        uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                    }
                }

                if (!gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }

        static void ProcesarDefender(GameManager gameManager, UIManager uiManager)
        {
            var resultado = gameManager.Defender();

            if (!resultado.Valido)
            {
                uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            Console.WriteLine($"\n{resultado.Mensaje}");

            // Mostrar contraataques de enemigos (con defensa reducida)
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    if (ataque.Enemigo != null)
                    {
                        uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                    }
                }

                if (!gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }

        static void ProcesarRecogerObjetoCombate(Sala sala, GameManager gameManager, UIManager uiManager)
        {
            if (!sala.TieneObjetos())
            {
                uiManager.MostrarAdvertencia("No hay objetos disponibles en la sala");
                return;
            }

            Console.WriteLine("\n📦 OBJETOS EN LA SALA:");
            for (int i = 0; i < sala.Objetos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sala.Objetos[i].Nombre}");
            }
            Console.Write("\n¿Qué objeto recoges? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                uiManager.MostrarError("Entrada inválida");
                return;
            }

            if (index == 0)
                return;

            if (index < 1 || index > sala.Objetos.Count)
            {
                uiManager.MostrarError("Selección inválida");
                return;
            }

            var objeto = sala.Objetos[index - 1];
            var resultado = gameManager.RecogerObjeto(objeto);

            if (!resultado.Valido)
            {
                uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            uiManager.MostrarObjetoRecogido(objeto);

            // Mostrar contraataques de enemigos
            if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
            {
                Console.WriteLine("\n⚠️  ¡Los enemigos aprovechan tu distracción!");
                foreach (var ataque in resultado.AtaquesEnemigos)
                {
                    if (ataque.Enemigo != null)
                    {
                        uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                    }
                }

                if (!gameManager.Jugador.EstaVivo())
                {
                    System.Threading.Thread.Sleep(500);
                    uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                }
            }
        }

        static void ProcesarRecogerObjeto(Sala sala, GameManager gameManager, UIManager uiManager)
        {
            if (!sala.TieneObjetos())
            {
                uiManager.MostrarAdvertencia("No hay objetos disponibles");
                return;
            }

            Console.WriteLine("\n📦 OBJETOS EN LA SALA:");
            for (int i = 0; i < sala.Objetos.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sala.Objetos[i].Nombre}");
            }
            Console.Write("\n¿Qué objeto recoges? (0 para cancelar): ");

            if (!int.TryParse(Console.ReadLine(), out int index))
            {
                uiManager.MostrarError("Entrada inválida");
                return;
            }

            if (index == 0)
                return;

            if (index < 1 || index > sala.Objetos.Count)
            {
                uiManager.MostrarError("Selección inválida");
                return;
            }

            var objeto = sala.Objetos[index - 1];
            var resultado = gameManager.RecogerObjeto(objeto);

            if (resultado.Valido)
            {
                uiManager.MostrarObjetoRecogido(objeto);
            }
            else
            {
                uiManager.MostrarError(resultado.Mensaje);
            }
        }

        static void ProcesarHuir(GameManager gameManager, UIManager uiManager)
        {
            var resultado = gameManager.IntentarHuir();

            if (!resultado.Valido)
            {
                uiManager.MostrarError(resultado.Mensaje);
                return;
            }

            if (resultado.HuyoExitosamente)
            {
                uiManager.MostrarMensaje("\n🏃 ¡Lograste huir del combate!", ConsoleColor.Yellow);
            }
            else
            {
                uiManager.MostrarMensaje("\n❌ ¡No lograste escapar!", ConsoleColor.Red);

                // Mostrar contraataques de enemigos
                if (resultado.AtaquesEnemigos != null && resultado.AtaquesEnemigos.Count > 0)
                {
                    Console.WriteLine();
                    foreach (var ataque in resultado.AtaquesEnemigos)
                    {
                        if (ataque.Enemigo != null)
                        {
                            uiManager.MostrarContraataque(ataque.Enemigo, ataque.Daño);
                        }
                    }

                    if (!gameManager.Jugador.EstaVivo())
                    {
                        System.Threading.Thread.Sleep(500);
                        uiManager.MostrarMensaje("\n💀 Has sido derrotado...", ConsoleColor.Red);
                    }
                }
            }
        }

        static void ProcesarBendicion(GameManager gameManager, UIManager uiManager)
        {
            var bendicion = gameManager.AplicarBendicion();
            if (bendicion != null)
            {
                uiManager.MostrarBendicionAplicada(bendicion);
            }
            else
            {
                uiManager.MostrarAdvertencia("No hay bendiciones disponibles");
            }
        }
    }
}
