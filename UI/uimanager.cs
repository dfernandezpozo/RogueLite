using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RogueLite.Models;

namespace RogueLite.UI
{
    public class UIManager
    {
        public void MostrarPantallaInicio()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║        ⚔️  MAZMORRA DEL DESTINO  ⚔️             ║
    ║                                               ║
    ║          Un Roguelite de Aventuras            ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
            AnimarTexto("\n    Presiona cualquier tecla para comenzar...", ConsoleColor.Yellow);
            Console.ReadKey(true);
        }

        public Personaje MostrarSeleccionPersonaje(List<Personaje> personajes)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║          ⚔️  SELECCIÓN DE HÉROE  ⚔️             ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();

            Console.WriteLine("\n  Elige tu clase de héroe:\n");

            for (int i = 0; i < personajes.Count; i++)
            {
                var personaje = personajes[i];
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"  [{i + 1}] {personaje.Nombre}");
                Console.ResetColor();
                
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($" ({personaje.Tipo})");
                Console.ResetColor();
                
                Console.WriteLine();
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"      ❤️  Vida: {personaje.VidaMaxima}");
                Console.ResetColor();
                
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  ⚔️  Ataque: {personaje.Ataque}");
                Console.ResetColor();
                
                Console.WriteLine();
            }

            Console.WriteLine("  ─────────────────────────────────────────────");
            Console.Write("\n  » Selecciona tu héroe (1-" + personajes.Count + "): ");

            int seleccion;
            while (!int.TryParse(Console.ReadLine(), out seleccion) || seleccion < 1 || seleccion > personajes.Count)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"  ❌ Opción inválida. Elige entre 1 y {personajes.Count}: ");
                Console.ResetColor();
            }

            var personajeSeleccionado = personajes[seleccion - 1];
            
            Console.WriteLine();
            AnimarTexto($"  ✓ Has elegido a {personajeSeleccionado.Nombre}!", ConsoleColor.Green);
            Thread.Sleep(1000);

            return personajeSeleccionado;
        }

        public void MostrarTransicionSala(Sala sala)
        {
            Console.Clear();
            Console.WriteLine("\n\n");
            AnimarTexto($"    ════════════════════════════════════════", ConsoleColor.DarkMagenta, 10);
            AnimarTexto($"          {sala.Nombre.ToUpper()}", ConsoleColor.Cyan, 20);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"\n    {sala.Descripcion}");
            Console.ResetColor();
            AnimarTexto($"    ════════════════════════════════════════", ConsoleColor.DarkMagenta, 10);
            Thread.Sleep(800);
        }

        public void MostrarInterfazJuego(Personaje jugador, Sala sala)
        {
            Console.Clear();

            // Header con stats del jugador
            MostrarHeaderJugador(jugador);

            // Info de la sala
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n📍 {sala.Nombre.ToUpper()} {(sala.Completada ? "✓" : "")}");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"   {sala.Descripcion}");
            Console.ResetColor();

            // Enemigos
            MostrarEnemigos(sala);

            // Objetos en sala
            MostrarObjetosSala(sala);

            // Inventario
            MostrarInventario(jugador.Inventario);

            // Bendiciones activas
            MostrarBendiciones(jugador.BendicionesActivas);
        }

        private void MostrarHeaderJugador(Personaje jugador)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("╔═════════════════════════════════════════════════════════════════╗");
            Console.Write("║ ");
            
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{jugador.Nombre} ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write($"[{jugador.Tipo}] ");
            Console.ResetColor();
            
            MostrarBarraVida(jugador.Vida, jugador.VidaMaxima);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"  Nivel: {jugador.Nivel}  XP: {jugador.Experiencia}");
            Console.WriteLine("".PadRight(5) + "║");
            Console.WriteLine("╚═════════════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }

        private void MostrarEnemigos(Sala sala)
        {
            if (sala.TieneEnemigos())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n⚔️  ENEMIGOS:");
                Console.ResetColor();

                foreach (var enemigo in sala.Enemigos.OrderByDescending(e => e.Vida))
                {
                    Console.Write($"  ▸ {enemigo.Nombre} ");
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write($"[{enemigo.Tipo}] ");
                    Console.ResetColor();
                    MostrarBarraVida(enemigo.Vida, enemigo.VidaMaxima);
                    Console.WriteLine($" ⚔️ {enemigo.Ataque}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ Sala despejada");
                Console.ResetColor();
            }
        }

        private void MostrarObjetosSala(Sala sala)
        {
            if (sala.TieneObjetos())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n💎 OBJETOS EN SALA:");
                Console.ResetColor();
                foreach (var obj in sala.Objetos)
                {
                    Console.WriteLine($"  ▸ {obj.Nombre} [{obj.Tipo}] +{obj.Valor}");
                }
            }
        }

        private void MostrarInventario(List<Objeto> inventario)
        {
            if (inventario.Any())
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n🎒 INVENTARIO:");
                Console.ResetColor();
                foreach (var item in inventario.GroupBy(i => i.Nombre))
                {
                    var cantidad = item.Count();
                    var valorTotal = item.Sum(i => i.Valor);
                    Console.WriteLine($"  ▸ {item.Key} x{cantidad} [+{valorTotal}]");
                }
            }
        }

        private void MostrarBendiciones(List<Bendicion> bendiciones)
        {
            if (bendiciones.Any())
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\n✨ BENDICIONES ACTIVAS:");
                Console.ResetColor();
                foreach (var b in bendiciones.GroupBy(b => b.Nombre))
                {
                    var cantidad = b.Count();
                    var valorTotal = b.Sum(x => x.Valor);
                    Console.WriteLine($"  ▸ {b.Key} x{cantidad} [+{valorTotal} {b.First().Tipo}]");
                }
            }
        }

        public void MostrarMenuAcciones()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────┐");
            Console.WriteLine("│ [1] ⚔️  Atacar    [2] 🎒 Objeto             │");
            Console.WriteLine("│ [3] ✨ Bendición  [4] 🚪 Siguiente sala     │");
            Console.WriteLine("└─────────────────────────────────────────────┘");
            Console.Write("\n» Elige tu acción: ");
        }

        public void MostrarAtaque(Enemigo enemigo, int daño, bool derrotado)
        {
            Console.WriteLine();
            AnimarTexto($"⚔️  Atacas a {enemigo.Nombre}!", ConsoleColor.Yellow, 30);
            Thread.Sleep(300);
            MostrarDaño(daño, true);

            if (derrotado)
            {
                Thread.Sleep(400);
                AnimarTexto($"\n💀 {enemigo.Nombre} ha sido derrotado!", ConsoleColor.Green);
            }
            else
            {
                Thread.Sleep(200);
                MostrarBarraVida(enemigo.Vida, enemigo.VidaMaxima);
            }
        }

        public void MostrarContraataque(Enemigo enemigo, int daño)
        {
            Thread.Sleep(500);
            Console.WriteLine();
            AnimarTexto($"\n🗡️  {enemigo.Nombre} contraataca!", ConsoleColor.Red, 30);
            Thread.Sleep(300);
            MostrarDaño(daño, false);
        }

        public void MostrarLootObtenido(Objeto loot)
        {
            Thread.Sleep(300);
            AnimarTexto($"\n🎁 ¡Loot obtenido! {loot.Nombre} [+{loot.Valor}]", ConsoleColor.Cyan);
        }

        public void MostrarObjetoRecogido(Objeto objeto)
        {
            AnimarTexto($"\n✓ Has recogido: {objeto.Nombre}", ConsoleColor.Green);
            MostrarMensaje($"  └─ Tipo: {objeto.Tipo} | Poder: +{objeto.Valor}", ConsoleColor.Gray);
        }

        public void MostrarBendicionAplicada(Bendicion bendicion)
        {
            AnimarTexto($"\n✨ Bendición activada: {bendicion.Nombre}", ConsoleColor.Cyan);
            MostrarMensaje($"  └─ {bendicion.Tipo}: +{bendicion.Valor}", ConsoleColor.Gray);
        }

        public void MostrarSubidaNivel(Personaje jugador)
        {
            Console.WriteLine();
            AnimarTexto("✨ ¡LEVEL UP! ✨", ConsoleColor.Magenta, 50);
            MostrarMensaje($"  └─ Nivel {jugador.Nivel} | Vida máxima: {jugador.VidaMaxima}", ConsoleColor.Cyan);
            Thread.Sleep(1500);
        }

        public void MostrarVictoria(int dañoTotal, List<Enemigo> derrotados, List<Objeto> inventario, Personaje jugador)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║            🏆  ¡VICTORIA!  🏆                   ║
    ║                                               ║
    ║        Has conquistado la mazmorra            ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();
            Thread.Sleep(1000);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n📊 ESTADÍSTICAS FINALES:\n");
            Console.ResetColor();

            Console.WriteLine($"  🎭 Héroe: {jugador.Nombre} [{jugador.Tipo}]");
            Console.WriteLine($"  ⚔️  Daño total infligido: {dañoTotal}");
            Console.WriteLine($"  💀 Enemigos derrotados: {derrotados.Count}");
            Console.WriteLine($"  🎒 Objetos recolectados: {inventario.Count}");
            Console.WriteLine($"  ⭐ Nivel alcanzado: {jugador.Nivel}");

            var objetoMasValioso = inventario.OrderByDescending(o => o.Valor).FirstOrDefault();
            if (objetoMasValioso != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\n  💎 Objeto más valioso: {objetoMasValioso.Nombre} [{objetoMasValioso.Valor}]");
                Console.ResetColor();
            }

            var grupos = derrotados.GroupBy(e => e.Tipo);
            if (grupos.Any())
            {
                Console.WriteLine("\n  📋 Enemigos por tipo:");
                foreach (var g in grupos)
                {
                    Console.WriteLine($"     • {g.Key}: {g.Count()}");
                }
            }

            Console.WriteLine("\n\n  Presiona cualquier tecla para salir...");
            Console.ReadKey(true);
        }

        public void MostrarGameOver(int dañoTotal, List<Enemigo> derrotados, Personaje jugador)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"
    ╔═══════════════════════════════════════════════╗
    ║                                               ║
    ║            💀  GAME OVER  💀                    ║
    ║                                               ║
    ║          Has caído en la mazmorra...          ║
    ║                                               ║
    ╚═══════════════════════════════════════════════╝
");
            Console.ResetColor();

            Console.WriteLine($"\n  🎭 {jugador.Nombre} ha caído en batalla...");
            Console.WriteLine($"  ⚔️  Daño total: {dañoTotal}");
            Console.WriteLine($"  💀 Enemigos derrotados: {derrotados.Count}");

            Console.WriteLine("\n\n  Presiona cualquier tecla para salir...");
            Console.ReadKey(true);
        }

        public void MostrarMensaje(string mensaje, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(mensaje);
            Console.ResetColor();
        }

        public void MostrarError(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"❌ {mensaje}");
            Console.ResetColor();
        }

        public void MostrarAdvertencia(string mensaje)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"⚠️  {mensaje}");
            Console.ResetColor();
        }

        public void EsperarTecla()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\n  [Presiona cualquier tecla para continuar]");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        private void MostrarBarraVida(int vidaActual, int vidaMaxima)
        {
            int anchoTotal = 20;
            int vidaLlena = (int)((double)vidaActual / vidaMaxima * anchoTotal);
            vidaLlena = Math.Clamp(vidaLlena,0, anchoTotal);

            Console.ForegroundColor = vidaActual > vidaMaxima * 0.5 ? ConsoleColor.Green :
                                     vidaActual > vidaMaxima * 0.25 ? ConsoleColor.Yellow :
                                     ConsoleColor.Red;

            Console.Write("❤️  [");
            Console.Write(new string('█', vidaLlena));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(new string('░', anchoTotal - vidaLlena));
            Console.ResetColor();
            Console.Write($"] {vidaActual}/{vidaMaxima}");
        }

        private void MostrarDaño(int cantidad, bool esJugador)
        {
            var color = esJugador ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.ForegroundColor = color;
            Console.WriteLine($"  └─ {'▼'} {cantidad} de daño");
            Console.ResetColor();
        }

        private void AnimarTexto(string texto, ConsoleColor color = ConsoleColor.White, int delay = 20)
        {
            Console.ForegroundColor = color;
            foreach (char c in texto)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        internal void MostrarGameOver(int dañoTotal, List<Enemigo> enemigosDerrotados)
        {
            throw new NotImplementedException();
        }
    }
}