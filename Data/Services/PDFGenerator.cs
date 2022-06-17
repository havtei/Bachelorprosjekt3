using Bachelorprosjekt.Models;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace GeneratePDF
{
    //TODO
   
    public class PDFGenerator : IPDFGenerator
    {
        
        static readonly object _object = new object ();
        static readonly object _object2 = new object();

        List<string> tempOverskrifter = new();
        //Todo generalize
        string Prosjektnavn = "not implemented";
        string Prosjektgiver = "not implemented";
        string Type = "not implemented";
        string BeskrivelseOppdragsgjevar = "not implemented";
        string BeskrivelseBakgrunn = "not implemented";
        string TeknologierMetoder = "not implemented";
        string BeskrivelseArbeidsoppgaver = "not implemented";
        string Annet = "not implemented";

        byte[] image;

        string Filnavn = "not implemented";
        //Todo
        //replace these?
        string Kontaktperson1Navn = "not implemented";
        string Kontaktperson1StillingRolle = "not implemented";
        string Kontaktperson1Epost = "not implemented";
        string Kontaktperson1Mobil = "not implemented";
        string Kontaktperson2Navn = "not implemented";
        string Kontaktperson2StillingRolle = "not implemented";
        string Kontaktperson2Epost = "not implemented";
        string Kontaktperson2Mobil = "not implemented";

        //List<Kontaktperson> Kontaktpersoner;

        string Indeks = "not implemented";

        List<string> overskrifter;

        List<string> textLeft = new(); //prosjektkategoriforside
        List<string> textMiddle = new(); //prosjektkategoriforside


        public byte[]? GenerateProsjektbeskrivelseDelete(string Prosjektnavn, string Prosjektgiver, string Type, string BeskrivelseOppdragsgjevar, string BeskrivelseBakgrunn, string TeknologierMetoder, string BeskrivelseArbeidsoppgaver, string Annet, string Kontaktperson1Navn, string Kontaktperson1StillingRolle, string Kontaktperson1Epost, string Kontaktperson1Mobil, string Kontaktperson2Navn, string Kontaktperson2StillingRolle, string Kontaktperson2Epost, string Kontaktperson2Mobil, string Filnavn, byte[] image, string Indeks, bool SkalSlette)
        {
            GenerateProsjektbeskrivelse(Prosjektnavn, Prosjektgiver, Type, BeskrivelseOppdragsgjevar, BeskrivelseBakgrunn, TeknologierMetoder, BeskrivelseArbeidsoppgaver, Annet, Kontaktperson1Navn, Kontaktperson1StillingRolle, Kontaktperson1Epost, Kontaktperson1Mobil, Kontaktperson2Navn, Kontaktperson2StillingRolle, Kontaktperson2Epost, Kontaktperson2Mobil, Filnavn, image, Indeks);


            try
            {
                byte[] FileBytes = System.IO.File.ReadAllBytes(Filnavn);

                if (SkalSlette)
                {
                    File.Delete(Filnavn);
                }

                return FileBytes;
            }
            catch (Exception e)
            {
                //Logging

            }



            return null;

        }
        
        public byte[]? GenerateProsjektbeskrivelse(ProsjektDescription prosjektDescription, string filename)
        {
            byte[] fileBytes = null;
            lock (_object2)
            {
                fileBytes= GenerateProsjektbeskrivelseDelete(prosjektDescription.Navn, prosjektDescription.Prosjektgiver, "", prosjektDescription.IntroOppdragsgiver, prosjektDescription.BagrunnProsjekt, prosjektDescription.TeknologiMetoder, prosjektDescription.Arbeidsoppgaver, prosjektDescription.Annet, "", "", "", "", "", "", "", "", filename, prosjektDescription.Image, prosjektDescription.IndeksString, true); 
            }
            return fileBytes;

        }
        public byte[]? GenerateProsjektbeskrivelseDelete(ProsjektDescription prosjektDescription, string filename, bool SkalSlette) => GenerateProsjektbeskrivelseDelete(prosjektDescription.Navn, prosjektDescription.Prosjektgiver, "", prosjektDescription.IntroOppdragsgiver, prosjektDescription.BagrunnProsjekt, prosjektDescription.TeknologiMetoder, prosjektDescription.Arbeidsoppgaver, prosjektDescription.Annet, "", "", "", "", "", "", "", "", filename, prosjektDescription.Image, prosjektDescription.IndeksString, SkalSlette);

        //Todo remove, or implement using GenerateProsektbeskrivelse
        public void Test()
        {

            string KortIntroOppdragsgiver = "står her kort om oppdragsgjevar";


            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginVertical(40);
                    page.MarginHorizontal(50);
                    page.Size(PageSizes.A4);


                    //page.Header().AlignCenter().Text(Header).FontColor(Colors.Blue.Darken4).Bold().FontSize(16);

                    //Placeholders.Paragraph();
                    page.Content().Element(ComposeContent);
                });


            });

            document.GeneratePdf("test.pdf");

            // Show in previewer
            //document.ShowInPreviewer();

        }

        public void GenerateProsjektbeskrivelseTilKatalog(ProsjektDescription p)

        {
            string Kontaktperson1Navn = "";
            string Kontaktperson1StillingRolle = "";
            string Kontaktperson1Epost = "";
            string Kontaktperson1Mobil = "";
            string Kontaktperson2Navn = "";
            string Kontaktperson2StillingRolle = "";
            string Kontaktperson2Epost = "";
            string Kontaktperson2Mobil = "";
            string Prosjekttyper = "";


            GenerateProsjektbeskrivelse(p.Navn, p.Prosjektgiver, Prosjekttyper, p.IntroOppdragsgiver, p.BagrunnProsjekt, p.TeknologiMetoder, p.Arbeidsoppgaver, p.Annet, Kontaktperson1Navn, Kontaktperson1StillingRolle, Kontaktperson1Epost, Kontaktperson1Mobil, Kontaktperson2Navn, Kontaktperson2StillingRolle, Kontaktperson2Epost, Kontaktperson2Mobil, p.Id+".pdf", p.Image,p.IndeksString);
        }
        
        //TODO generalize
        public void GenerateProsjektbeskrivelse(string Prosjektnavn, string Prosjektgiver, string Type, string BeskrivelseOppdragsgjevar, string BeskrivelseBakgrunn, string TeknologierMetoder, string BeskrivelseArbeidsoppgaver, string Annet, string Kontaktperson1Navn, string Kontaktperson1StillingRolle, string Kontaktperson1Epost, string Kontaktperson1Mobil, string Kontaktperson2Navn, string Kontaktperson2StillingRolle, string Kontaktperson2Epost, string Kontaktperson2Mobil, string Filnavn,byte[] image,string Indeks)
        {
            this.image = image;
            this.Prosjektnavn = Prosjektnavn;
            this.Prosjektgiver = Prosjektgiver;
            this.Type = Type;
            this.BeskrivelseOppdragsgjevar = BeskrivelseOppdragsgjevar;
            this.BeskrivelseBakgrunn = BeskrivelseBakgrunn;
            this.TeknologierMetoder = TeknologierMetoder;
            this.BeskrivelseArbeidsoppgaver = BeskrivelseArbeidsoppgaver;
            this.Annet = Annet;
            
            this.Prosjektnavn = Indeks + "  " + Prosjektnavn;
            //Todo replace
            this.Kontaktperson1Navn = Kontaktperson1Navn;
            this.Kontaktperson1StillingRolle = Kontaktperson1StillingRolle;
            this.Kontaktperson1Epost = Kontaktperson1Epost;
            this.Kontaktperson1Mobil = Kontaktperson1Mobil;
            this.Kontaktperson2Navn = Kontaktperson2Navn;
            this.Kontaktperson2StillingRolle = Kontaktperson2StillingRolle;
            this.Kontaktperson2Epost = Kontaktperson2Epost;
            this.Kontaktperson2Mobil = Kontaktperson2Mobil;

            //Todo filename as input
            this.Filnavn = Filnavn;

            var document = Document.Create(container =>
            {
                
                container.Page(page =>
                {
                    page.MarginVertical(40);
                    page.MarginHorizontal(50);
                    page.Size(PageSizes.A4);
                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                });
            });
            document.GeneratePdf(Filnavn);
            
            //document.ShowInPreviewer(12345);
        }
        private void ComposeHeader(IContainer container)
        {
            container
                
                
                //.Padding(-30)
                
                .Column(column =>
                {
                    //ToDO
                    //Add functionality for company logo
                    //column.Item().AlignRight().Width(100).Height(50).Image(Placeholders.Image);
                
                });
        }
        //Todo generalize
        private void ComposeContent(IContainer container)
        {
            container
                .PaddingVertical(10)

                .Column(column =>
                {

                column.Item().Text(text =>
                {
                    text.Element().PaddingBottom(10).AlignCenter().Text(Prosjektnavn).FontColor(Colors.Blue.Darken4).Bold().FontSize(18);
                    text.ParagraphSpacing(30);
                    text.ParagraphSpacing(0);
                    text.Line("");
                    text.Line(BeskrivelseOppdragsgjevar);
                });
                column.Item().Text(text =>
                {
                    text.Element().AlignLeft().Text("Bakgrunn for prosjektet").FontColor(Colors.Blue.Darken2).Bold().FontSize(14);
                    text.ParagraphSpacing(30);
                    text.ParagraphSpacing(0);
                    text.Line("");
                    text.Line(BeskrivelseBakgrunn);
                });

                column.Item().Text(text =>
                {
                    text.Element().AlignLeft().Text("Beskrivelse av teknologier / metoder som er tenkt brukt i prosjektet").FontColor(Colors.Blue.Darken2).Bold().FontSize(14);
                    text.ParagraphSpacing(30);
                    text.ParagraphSpacing(0);
                    text.Line("");
                    text.Line(TeknologierMetoder);
                });
                column.Item().Text(text =>
                {
                    text.Element().AlignLeft().Text("Beskrivelse av arbeidsoppgaver for studentene").FontColor(Colors.Blue.Darken2).Bold().FontSize(14);
                    text.ParagraphSpacing(30);
                    text.ParagraphSpacing(0);
                    text.Line("");
                    text.Line(BeskrivelseArbeidsoppgaver);
                });
                column.Item().Text(text =>
                {
                    text.Element().AlignLeft().Text("Annet").FontColor(Colors.Blue.Darken2).Bold().FontSize(14);
                    text.ParagraphSpacing(30);
                    text.ParagraphSpacing(0);
                    text.Line("");
                    text.Line(Annet);
                    
                });
                    if (image != null)
                    {
                        column.Item().Row(row =>
                        {
                            row.RelativeItem()

                            .Padding(10)

                            .Image(image);

                            row.RelativeItem()

                            .Padding(10)

                            //Kontaktpersoner
                            .Text(text =>
                            {
                                text.Element().AlignLeft().Text("Kontaktpersoner").FontColor(Colors.Blue.Darken2).Bold().FontSize(14);
                                text.Line("");
                                text.Line("Aktuelle kontaktpersoner / veiledere: ");

                                text.Line("   \u2022" + "   " + Kontaktperson1Navn + "(" + Kontaktperson1StillingRolle + ")");
                                text.Line("   \u2022" + "   Epost: " + Kontaktperson1Epost);
                                text.Line("   \u2022" + "   Telefon: " + Kontaktperson1Mobil);

                            //Todo:
                            //Condition here...
                                text.Line("");
                                text.Line("   \u2022" + "   " + Kontaktperson2Navn + "(" + Kontaktperson2StillingRolle + ")");
                                text.Line("   \u2022" + "   Epost: " + Kontaktperson2Epost);
                                text.Line("   \u2022" + "   Telefon: " + Kontaktperson2Mobil);

                            });

                        });
                    }

                }); 

        }
        private void ForsideInnhold(IContainer container)
        {
            
            //Todo
            //generalize, object oriented, abstract class?

            string overskriftForside = "Bachelorprosjekt for Data og Informasjonsteknologi " + "Våren 2022";

            string s0 = "Institutt for datateknologi, elektroteknologi og realfag,";
            string s1 = "Fakultet for ingeniørutdanning,";
            string s2 = "Høgskulen på Vestlandet";
            string versjon = "Versjon \"alle valgte og tildelte\" aka \"skal veiledes\"";
            string dato = "06.01022";


            container

                .PaddingVertical(40)
                .PaddingHorizontal(20)
                
                .Text(text =>
                {
                    text.AlignCenter();
                    text.Line(overskriftForside).FontSize(50);     
                    text.Line(s0).FontSize(18).Italic();
                    text.Line(s1).FontSize(18).Italic();
                    text.Line(s2).FontSize(18).Italic();
                    text.Line("");
                    text.Line(versjon).FontSize(14).Bold();
                    text.Line(dato).FontSize(14).Bold();

                });
           
        }
        public PdfDocument GenererForside()
        {

            string name = "forside.pdf";

            //Todo...
            

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginVertical(40);
                    page.MarginHorizontal(50);
                    //page.Background().Image(Placeholders.Image);
                   // page.Header().MaxHeight(100).MaxWidth(200).AlignTop().AlignLeft().Image("hvlLogo.png");
                    page.Size(PageSizes.A4);
                    page.Content().Element(ForsideInnhold);
                    
                });
            });
            document.GeneratePdf(name);


            //Brukt til testing
            //document.ShowInPreviewer(12345);



            PdfDocument doc = new PdfDocument();
            doc = PdfReader.Open(name, PdfDocumentOpenMode.Import);
           

            return doc;
        }
        public void ProsjektkategoriInnhold(IContainer container)
        {
            

            container
                .Text(text =>
                {
                    text.AlignCenter();
                    
                    text.AlignLeft();
                    text.EmptyLine();
                    
                    foreach(string s in textLeft)
                    {
                        text.Line(s).FontColor(Colors.Blue.Darken3).FontSize(14);
                    }

                    text.Line("");
                    
                    text.Line("");
                    text.AlignCenter();
                    foreach (string s in textMiddle)
                    {
                        text.Line(s).FontColor(Colors.Red.Darken3).Bold().FontSize(14);
                    }

                });

        }
        public PdfDocument GenererProsjektkategoriForside(string name, string overskrift, List<string> textLeft, List<string> textMiddle)
        {

            //string name = "forside.pdf";

            //Todo...
            this.textLeft = textLeft;
            this.textMiddle = textMiddle;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginVertical(40);
                    page.MarginHorizontal(50);
                    //page.Background().Image(Placeholders.Image);
                    
                    page.Size(PageSizes.A4);
                    page.Header().Text(overskrift).FontColor(Colors.Blue.Darken4).FontSize(25).Bold();
                    page.Content().Element(ProsjektkategoriInnhold);

                });
            });
            document.GeneratePdf(name);

            //Brukt til testing
            //document.ShowInPreviewer(12345);

            PdfDocument doc = new PdfDocument();
            doc = PdfReader.Open(name, PdfDocumentOpenMode.Import);


            return doc;
        }
        //Todo
        //Generalize...
        //1 method for generating pdf, all other methods which generate a pdf use this?
        //TODO: tittel, forskjellig tekst. Eks. Prosjektkatalog for valgte vs godkjente.
        public PdfDocument GenererInnholdsfortegnelse(List<string> overskrifter/*,List<Prosjektbeskrivelse> prosjektbeskrivelsar*/)
        {
            //this.tempOverskrifter = overskrifter;
            //Todo
            //add links
            this.overskrifter = overskrifter;
            //Dummy data:
            
            //composition?
            


            string name = "innholdsfortegnelse.pdf";

            //Todo...


            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.MarginVertical(20);
                    page.MarginHorizontal(50);
                    //page.Background().Image(Placeholders.Image);
                    page.Size(PageSizes.A4);
                    page.Content().Element(InnholdsfortegnelseInnhold);

                });
            });
            document.GeneratePdf(name);
           

            //Brukt til testing
            //document.ShowInPreviewer(12345);



            PdfDocument doc = new PdfDocument();
            doc = PdfReader.Open(name, PdfDocumentOpenMode.Import);


            return doc;

        }
        private void InnholdsfortegnelseInnhold(IContainer container)
        {
            //List<string> overskrifter = new List<string>();
            List<int> sidetal = new List<int>();


            //List<string> tempOverskrifter = //new List<string>();
           
            

            container

                
                
                .Text(text =>
                {
                    text.AlignCenter();
                    text.Line("Innholdsfortegnelse").FontSize(25).FontColor(Colors.Blue.Darken4);
                    
                    text.AlignLeft();
                    text.EmptyLine();

                    
                    foreach (string s in tempOverskrifter)
                    {
                        text.Line(s); 
                        
                    }
                    text.Line("");
                    

                });
        }


        //use questpdf instead
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void GenerateProsjektKatalog(List<string> filer, string filnavn)
        {
            

            List<string> filesToDelete = new();
            

            //Todo
            List<string> prosjektTypar = new();
            prosjektTypar.Add("INTERNE PROSJEKT");
            prosjektTypar.Add("EKSTERNE PROSJEKT");
            prosjektTypar.Add("RESERVERTE PROSJEKT");

            PdfDocument doc;
            PdfDocument ProsjektkatalogPdf = new PdfDocument();
            
            //have to change this!
            //temporary solution
            PdfDocument forside = GenererForside();

            PdfDocument innholdsfortegnelse = GenererInnholdsfortegnelse(filer);

            KopierSider(forside, ProsjektkatalogPdf);

            KopierSider(innholdsfortegnelse, ProsjektkatalogPdf);

            //Todo
            //generalize
            //composition, object oriented
            List<string> interneProsjektForsideMiddle = new List<string>{ "Prosjekt med sete i Bergen er merket IB,", "prosjekt i Sunnfjord er merket IF,", "og prosjekt i Haugesund er merket IH." };
            List<string> interneProsjektForsideLeft =  new List<string>();

            List<string> eksterneProsjektForsideMiddle = new List<string> { "Prosjekt med sete i Bergen er merket EB,", "og prosjekt i Sunnfjord er merket EF." };
            List<string> eksterneProsjektForsideLeft = new List<string>();
            
            List<string> reserverteProsjektForsideMiddle = new List<string> { "Prosjekt med sete i Bergen er merket RB,", "og prosjekt i Sunnfjord er merket RF." };
            List<string> reserverteProsjektForsideLeft = new List<string> { "Prosjekt som er:", "- gitt av praksisbedrift til praksisstudenter", "- egendefinert av studenter", "- avtalt/hentet inn fra bedrift av studenter", "- reservert/tildelt student utenom åpent valg"};

            string interneProsjektForside = "interneProsjektForside.pdf";
            string eksterneProsjektForside = "eksterneProsjektForside.pdf";
            string reserverteProsjektForside = "reserverteProsjektForside.pdf";

            GenererProsjektkategoriForside(interneProsjektForside, "INTERNE PROSJEKT", interneProsjektForsideLeft, interneProsjektForsideMiddle);
            GenererProsjektkategoriForside(eksterneProsjektForside, "EKSTERNE PROSJEKT", eksterneProsjektForsideLeft, eksterneProsjektForsideMiddle);
            GenererProsjektkategoriForside(reserverteProsjektForside, "RESERVERTE PROSJEKT", reserverteProsjektForsideLeft, reserverteProsjektForsideMiddle);

            //TODO
            //dummy data, testing...
            //filer.Insert(3, interneProsjektForside);
            //filer.Insert(5, eksterneProsjektForside);
            //filer.Insert(7, reserverteProsjektForside);


            foreach (string s in filer)
            {
                bool cont = true;
                while (cont)
                {
                    try
                    {
                        doc = PdfReader.Open(s, PdfDocumentOpenMode.Import);
                        KopierSider(doc, ProsjektkatalogPdf);
                        cont = false;
                    }catch(Exception e) { }
                }

                
            }
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            ProsjektkatalogPdf.Save(filnavn + ".pdf");

            //Delete
            foreach (string s in filer)
            {
                File.Delete(s);
            }
            //File.Delete("eksterneProsjektForside.pdf");
            //File.Delete("forside.pdf");
            //File.Delete("innholdsfortegnelse.pdf");
            //File.Delete("interneProsjektForside.pdf");
            //File.Delete("reserverteProsjektForside.pdf");


        }
        private void KopierSider(PdfDocument source, PdfDocument destination)
        {
            for (int i = 0; i < source.PageCount; i++)
            {
                destination.AddPage(source.Pages[i]);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public byte[] GenerateProsjektKatalog(List<ProsjektDescription> prosjektDescriptions, string navn)
        {
            byte[] fileBytes = null;
            lock (_object)
            {
                List<string> filer = new();
                foreach (ProsjektDescription p in prosjektDescriptions)
                {
                    GenerateProsjektbeskrivelseTilKatalog(p);
                    string filnavn = p.Id + ".pdf";
                    filer.Add(filnavn);
                    tempOverskrifter.Add(p.IndeksString + "      " + p.Navn);
                }

                GenerateProsjektKatalog(filer, navn);

                string katalogNavn = navn + ".pdf";

                fileBytes = System.IO.File.ReadAllBytes(katalogNavn);

                //Delete
                File.Delete(katalogNavn);

            }

            return fileBytes;
            

        }

        
    }
}