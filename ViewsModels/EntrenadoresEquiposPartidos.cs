using FutbolNet.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolNet.ViewsModels
{
    public class EntrenadoresEquiposPartidos
    {
        public Entrenador Entrenador { get;}
        public Equipo Equipo { get; }
        public Partido Partido { get; }


        public EntrenadoresEquiposPartidos(Entrenador entrenador, Equipo equipo, Partido partido)
        {
            Entrenador = entrenador ?? throw new ArgumentNullException(nameof(entrenador),"El entrenador no puede ser nulo");
            Equipo = equipo ?? throw new ArgumentNullException(nameof(equipo), "El equipo no puede ser nulo");
            Partido = partido ?? throw new ArgumentNullException(nameof(partido), "El partido no puede ser nulo");
        }
        public EntrenadoresEquiposPartidos() { }
    }
}
