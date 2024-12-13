namespace FutbolNet.Modelos
{
    public class Partido
    {
        public int Id { get; set; }

        // Relación con Equipo para local y visitante
        public int EquipoLocalId { get; set; }
        public Equipo? EquipoLocal { get; set; }

        public int EquipoVisitanteId { get; set; }
        public Equipo? EquipoVisitante { get; set; }

        // Fecha del partido con un valor predeterminado
        public DateTime Fecha { get; set; } = DateTime.Now; // Asigna la fecha actual por defecto

        // Constructor para asegurar que la fecha no sea nula
        public Partido()
        {
            Fecha = DateTime.Now; // Si no se asigna un valor, se establece la fecha actual
        }
    }
}
