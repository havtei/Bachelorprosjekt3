using Microsoft.AspNetCore.Identity;

namespace Bachelorprosjekt.Models
{
    public class ProsjektDescription
    {
        public ProsjektDescription()
        {
            this.Domains = new List<Domain>();
            this.ProjectTypes = new List<ProjectType>();
            this.Meldinger = new List<Melding>();

        }
        public int Id { get; set; }
        public string Navn { get; set; }
        public string Prosjektgiver { get; set; }
        public string Webadresse { get; set; }
        public string IntroOppdragsgiver { get; set; }
        public string BagrunnProsjekt { get; set; }
        public string? TeknologiMetoder { get; set; }
        public string Arbeidsoppgaver { get; set; }
        public string Annet { get; set; }
        public int status { get; set; }
        public bool ErValgt { get; set; }
        
        //lokasjon vil sjelden eller aldri bli lagt til. 1 for Bergen, 2 for Førde, 3 for Ekstern
        public int Lokasjon { get; set; }

        //Indeks per lokasjon
        public string? IndeksString { get; set; }
        public int Indeks { get; set; }

        public virtual ICollection<Melding> Meldinger { get; set; }

        public virtual ICollection<Domain> Domains { get; set; }
        public virtual ICollection<ProjectType> ProjectTypes { get; set; }

        public string? IdentityUserID { get; set; }
        public byte[]? Image { get; set; }
    }
}
