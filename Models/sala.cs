using System.Collections.Generic;
using System.Linq;

namespace RogueLite.Models
{
    public class Sala
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public List<Enemigo> Enemigos { get; set; } = new();
        public List<Objeto> Objetos { get; set; } = new();
        public bool Completada { get; set; } = false;

        public bool TieneEnemigos() => Enemigos.Any();
        public bool TieneObjetos() => Objetos.Any();
        public bool EstaLimpia() => !TieneEnemigos();

        public void CompletarSala()
        {
            Completada = true;
        }
    }
}