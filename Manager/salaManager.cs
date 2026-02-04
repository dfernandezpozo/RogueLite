using System.Collections.Generic;
using RogueLite.Models;

namespace RogueLite.Manager
{
    public class SalaManager
    {
        public List<Sala> Salas { get; private set; } = new();

        public void GenerarSalas(int cantidad, CombateManager combateManager, LootManager lootManager)
        {
            for (int i = 1; i <= cantidad; i++)
            {
                var sala = new Sala
                {
                    Id = i,
                    Nombre = $"Sala {i}",
                    Descripcion = $"Descripción de la sala {i}",
                    Enemigos = combateManager.GenerarEnemigosParaSala(),
                    Objetos = lootManager.GenerarObjetosParaSala()
                };
                Salas.Add(sala);
            }
        }
    }
}
