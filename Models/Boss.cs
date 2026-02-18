using System.Collections.Generic;

namespace RogueLite.Models
{
    public class Boss : Enemigo
    {
        public List<HabilidadBoss> Habilidades { get; set; } = new();
        public bool EnFase2 => Vida <= VidaMaxima / 2;
        public int ContadorTurnos { get; set; } = 0;
        public bool EsBoss { get; set; } = true;
        
        // Cada X turnos usa habilidad
        public int TurnosParaHabilidad { get; set; } = 3;
        
        public HabilidadBoss? ObtenerHabilidadParaUsar()
        {
            ContadorTurnos++;
            
            if (ContadorTurnos >= TurnosParaHabilidad && Habilidades.Count > 0)
            {
                ContadorTurnos = 0;
                // En fase 2 usa habilidades más seguido
                if (EnFase2)
                    TurnosParaHabilidad = 2;
                    
                return Habilidades[new Random().Next(Habilidades.Count)];
            }
            
            return null;
        }
    }
    
    public class HabilidadBoss
    {
        public string Nombre { get; set; } = "";
        public string Descripcion { get; set; } = "";
        public int Danio { get; set; }
        public bool EsAreaDanio { get; set; } // Ignora defensa
        public int CuracionPropia { get; set; }
    }
}