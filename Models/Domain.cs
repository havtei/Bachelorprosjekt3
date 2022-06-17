namespace Bachelorprosjekt.Models
{
    public class Domain
    {
        public Domain()
        {
            this.ProsjektDescriptions = new List<ProsjektDescription>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProsjektDescription> ProsjektDescriptions{ get;set; }
    }
}
