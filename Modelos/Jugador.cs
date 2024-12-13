namespace FutbolNet.Modelos
{
    public class Jugador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Posicion { get; set; } = string.Empty; // Ej: Portero, Defensa, Mediocampista, Delantero
        public string Equipo { get; set; } = string.Empty; // Nombre del equipo

        public override string ToString()
        {
            return Nombre;
        }
    }
}
