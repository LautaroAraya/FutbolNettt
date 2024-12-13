using FutbolNet.Modelos;

namespace FutbolNet.Class
{
    public class ApiEndPoints
    {
        public static string Entrenador { get; set; } = "entrenadors";
        public static string Jugador { get; set; } = "jugadores";
        public static string Equipo { get; set; } = "equipoes";
        public static string Liga { get; set; } = "ligas";
        public static string Partido { get; set; } = "partidoes";


        public static string GetEndPoint(string name)
        {
            return name switch
            {
                nameof(Entrenador) => Entrenador,
                nameof(Equipo) => Equipo,
                nameof(Jugador) => Jugador,
                nameof(Liga) => Liga,
                nameof(Partido) => Partido,
                _ => throw new ArgumentException($"Endpoint '{name}' no está definido.")
            };
        }
    }

}