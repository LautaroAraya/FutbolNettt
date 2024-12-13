
using CommunityToolkit.Mvvm.Messaging.Messages;
using FutbolNet.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolNet.Class
{
    public class MyMessage : ValueChangedMessage<string>
    {
        public Entrenador Entrenador { get; set; }

        public Partido Partido { get; set; }

        public Liga Liga { get; set; }

        public Jugador Jugador { get; set; }

        public Equipo Equipo { get; set; }

        public MyMessage(string value):base(value)
        {
                
        }
    }
}
