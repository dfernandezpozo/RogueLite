using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using RogueLite.Models;

namespace RogueLite.Manager
{
    public class LootManager
    {
        private readonly Random random = new();
        private List<Objeto> lootDisponibles = new();

        public void CargarLoot(string path)
        {
            if (!File.Exists(path)) return;
            var json = File.ReadAllText(path);
            lootDisponibles = JsonSerializer.Deserialize<List<Objeto>>(json) ?? new();
        }

        public List<Objeto> GenerarObjetosParaSala()
        {
            if (!lootDisponibles.Any()) return new List<Objeto>();
            int cantidad = random.Next(0, 2);
            return lootDisponibles.OrderBy(_ => random.Next()).Take(cantidad).ToList();
        }
    }
}
