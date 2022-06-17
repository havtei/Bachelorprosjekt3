namespace Bachelorprosjekt.Models
{
    public class Oppdragsgiver
    {
        public Oppdragsgiver()
        {
            this.ProsjektDescriptions = new List<ProsjektDescription>();
        }
        public string Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int? AntInnmeldteProsjekt { get; set; }
        public virtual ICollection<ProsjektDescription> ProsjektDescriptions { get; set; }
    }
}
