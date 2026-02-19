using System;
using System.Collections.Generic;
using RogueLite.Models;
using RogueLite.UI.Renderers;
using RogueLite.UI.Screens;

namespace RogueLite.UI
{
    /// <summary>
    /// Manejador principal de la interfaz de usuario.
    /// Coordina las diferentes pantallas y componentes visuales.
    /// </summary>
    public class UIManager
    {
        private readonly StartScreen _startScreen;
        private readonly CharacterSelectionScreen _characterSelectionScreen;
        private readonly GameScreen _gameScreen;
        private readonly ResultScreen _resultScreen;
        private readonly MessageRenderer _messageRenderer;

        public UIManager()
        {
            _startScreen = new StartScreen();
            _characterSelectionScreen = new CharacterSelectionScreen();
            _gameScreen = new GameScreen();
            _resultScreen = new ResultScreen();
            _messageRenderer = new MessageRenderer();
        }

        public void MostrarPantallaInicio()
        {
            _startScreen.Mostrar();
        }

        public Personaje MostrarSeleccionPersonaje(List<Personaje> personajes)
        {
            return _characterSelectionScreen.Mostrar(personajes);
        }

        public void MostrarTransicionSala(Sala sala)
        {
            _gameScreen.MostrarTransicionSala(sala);
        }

        public void MostrarInterfazJuego(Personaje jugador, Sala sala)
        {
            _gameScreen.MostrarInterfaz(jugador, sala);
        }

        public void MostrarMenuAcciones()
        {
            _gameScreen.MostrarMenuAcciones();
        }

        public void MostrarAtaque(Enemigo enemigo, int daño, bool derrotado)
        {
            _gameScreen.MostrarAtaque(enemigo, daño, derrotado);
        }

        public void MostrarContraataque(Enemigo enemigo, int daño)
        {
            _gameScreen.MostrarContraataque(enemigo, daño);
        }

        public void MostrarLootObtenido(Objeto loot)
        {
            _messageRenderer.MostrarLootObtenido(loot);
        }

        public void MostrarObjetoRecogido(Objeto objeto)
        {
            _messageRenderer.MostrarObjetoRecogido(objeto);
        }

        public void MostrarBendicionAplicada(Bendicion bendicion)
        {
            _messageRenderer.MostrarBendicionAplicada(bendicion);
        }

        public void MostrarSubidaNivel(Personaje jugador)
        {
            _messageRenderer.MostrarSubidaNivel(jugador);
        }

        public void MostrarVictoria(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            _resultScreen.MostrarVictoria(dañoTotal, derrotados, inventario, jugador);
        }

        public void MostrarGameOver(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            _resultScreen.MostrarGameOver(dañoTotal, derrotados, jugador);
        }

        public void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            _messageRenderer.MostrarMensaje(mensaje, color);
        }

        public void MostrarError(string mensaje)
        {
            _messageRenderer.MostrarError(mensaje);
        }

        public void MostrarAdvertencia(string mensaje)
        {
            _messageRenderer.MostrarAdvertencia(mensaje);
        }

        public void EsperarTecla()
        {
            _messageRenderer.EsperarTecla();
        }
    }
}
