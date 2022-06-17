#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Bachelorprosjekt.Data;
using Bachelorprosjekt.Models;
using Microsoft.AspNetCore.Authorization;
using GeneratePDF;

namespace Bachelorprosjekt.Controllers
{
    //autorisering
    [Authorize(Roles = "Fagansvarlig,Admin,Bedrift,Student")]
    public class ProsjektDescriptionsController : Controller
    {
        private readonly BachelorprosjektContext _context;

        //TODO dependency injection
        private IPDFGenerator IPDFGenerator { get; set; }
        public ProsjektDescriptionsController(BachelorprosjektContext context)
        {
            //TODO dependency injection
            //
           
            IPDFGenerator = new PDFGenerator();
            _context = context;
        }

        //GET
        //Generering og nedlasting av prosjektlatalog av alle godkjente prosjektforslag
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public FileResult ProsjektkatalogAlleGodkjente()
        {
            var projects = _context.ProsjektDescription.ToList();
            //TODO generalize status
            //list of projects with status != 2 (status 2 is "Fiks")
            var projectsValgte = projects.Where(p => p.status==4);
            string filnavn = "ProsjektkatalogAlleGodkjente";


            var projectsValgteAndSorted = projectsValgte.OrderBy(p => p.IndeksString);




            byte[] FileBytes = IPDFGenerator.GenerateProsjektKatalog(projectsValgteAndSorted.ToList(), filnavn);



            return File(FileBytes, "application/pdf");
        }

        // GET
        //Generering og nedlasting av prosjektlatalog av alle valgte prosjektforslag
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public FileResult ProsjektkatalogAlleValgte()
        {
            var projects =  _context.ProsjektDescription.ToList();
            //TODO generalize status
            //list of projects with status != 2 (status 2 is "Fiks")
            var projectsValgte= projects.Where(p => p.ErValgt);
            string filnavn = "ProsjektkatalogAlleValgte";


            var projectsValgteAndSorted = projectsValgte.OrderBy(p => p.IndeksString);

            byte[] FileBytes = IPDFGenerator.GenerateProsjektKatalog(projectsValgteAndSorted.ToList(), filnavn);


            

            return File(FileBytes, "application/pdf");
        }

        //GET
        //Få side der fagansvarleg skal kunne krysse av valgte prosjekt
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public async Task<IActionResult> OppgiValgteProsjekt()
        {
            var projects = await _context.ProsjektDescription.ToListAsync();
            //TODO status, tabell vs numbers
            List<ProsjektDescription> projectsGodkjente = projects.Where(p => p.status == 4).ToList();

            return View(projectsGodkjente);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public async Task<IActionResult> OppgiValgteProsjekt(int id)
        {
            //TODO
            
            if (!ProsjektDescriptionExists(id))
            {
                return NotFound();
            }
            var prosjektDescription = await _context.ProsjektDescription.FindAsync(id);

            prosjektDescription.ErValgt = true;

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Entry(prosjektDescription).State = EntityState.Detached;
                    _context.Update(prosjektDescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsjektDescriptionExists(prosjektDescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(OppgiValgteProsjekt));
            }
            return View(prosjektDescription);
        }







        // GET: ProsjektDescriptions
       
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProsjektDescription.ToListAsync());
        }

        // GET: ProsjektDescriptions klare for gokjnenning
        [Authorize(Roles = "Fagansvarlig,Admin")]
        public async Task<IActionResult> ProsjektKlarForGodkjenning()
        {

            var projects = await _context.ProsjektDescription.ToListAsync();


            //TODO generalize status
            //list of projects with status != 2 (status 2 is "Fiks") and not 4 (4 is "godkjent")
            List<ProsjektDescription> projectsNotFiks = projects.Where(p=>p.status!=2 && p.status!=4).ToList();
            
            return View(projectsNotFiks);
        }

        [Authorize(Roles = "Fagansvarlig,Admin")]
        public async Task<IActionResult> ProsjektVenterFiks()
        {
            var projects = await _context.ProsjektDescription.ToListAsync();
            //TODO generalize status
            //list of projects with status != 2 (status 2 is "Fiks")
            List<ProsjektDescription> projectsNotFiks = projects.Where(p => p.status == 2).ToList();

            return View(projectsNotFiks);
        }


        // GET
        // Vil vise prosjektforslag som er eigd av innlogga brukar
        [Authorize(Roles ="Bedrift,Student")]
        public async Task<IActionResult> GetMyProjects()
        {
            //Todo
            // TODO general method to find ID?
            string id = User.Claims.ToList().ElementAt(0).Value; 
            List<ProsjektDescription> ProsjektDescriptions = await _context.ProsjektDescription.ToListAsync();
            List<ProsjektDescription> a = ProsjektDescriptions.Where(p => p.IdentityUserID == id).ToList();

            return View(a);
        }


        // GET: ProsjektDescriptions/ShowAsPDF/5
        [Authorize]
        public async Task<IActionResult> ShowAsPDF(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription
                .FirstOrDefaultAsync(m => m.Id == id);


            if (prosjektDescription == null)
            {
                return NotFound();
            }
            
            

            return View(prosjektDescription);
        }

        // GET: ProsjektDescriptions/ShowAsPDFGodkjennFiks/5
        //TODO: Kan generalisere
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> ShowAsPDFGodkjennFiks(int? id, string sentFrom)

        {
            if (sentFrom == null)
            {
                return NotFound();
            }
            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (prosjektDescription == null)
            {
                return NotFound();
            }


            ViewData["sentFrom"] = sentFrom;
            return View(prosjektDescription);
        }


        public FileResult GetReport(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var prosjektDescription =  _context.ProsjektDescription
                .FirstOrDefault(m => m.Id == id);

            if (prosjektDescription == null)
            {
                return null;
            }
            
            byte[] FileBytes = IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription, prosjektDescription.Id + ".pdf");

            return File(FileBytes, "application/pdf");
        }

        // GET: ProsjektDescriptions/Create
        [Authorize(Roles = "Student,Bedrift")]
        public IActionResult Create()
        {
            if (!User.IsInRole("Bedrift") && !User.IsInRole("Student")){
                return BadRequest();
            }
            DbSet<ProjectType> ProjectTypes = _context.ProjectType;
            ProjectTypes.Load();
            ViewData["ProjectTypes"] = ProjectTypes.Local;

            DbSet<Domain> Domains = _context.Domain;
            Domains.Load();
            ViewData["Domains"] = Domains.Local;

            return View("Create");
        }

        // POST: ProsjektDescriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Student, Bedrift")]
        public async Task<IActionResult> Create(string submit,string lokasjon,[Bind("Id,Navn,Prosjektgiver,Webadresse,IntroOppdragsgiver,BagrunnProsjekt,TeknologiMetoder,Arbeidsoppgaver,Annet,status")] ProsjektDescription prosjektDescription)
        {

            //get indeks

            var lokasjoner = await _context.Lokasjon.ToListAsync();

            Lokasjon l = lokasjoner.ToList().Where(l => l.Name.Equals(lokasjon)).First();

            if (l == null)
            {
                return NotFound();
            }
            prosjektDescription.Lokasjon = l.Id;


            //TODO remove
            prosjektDescription.Indeks = l.Antall + 1;
            prosjektDescription.IndeksString = l.NormalizedName + "-" + prosjektDescription.Indeks;
            l.Antall++;
            

            //TODO
            if (submit.Equals("Last ned PDF"))
            {
                byte[] FileBytes = IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription, prosjektDescription.Id + "_temp.pdf");


                return File(FileBytes, "application/pdf");

            }

            _context.Update(l);
            IFormFile f = null;
            if (Request.Form.Files.Count == 1)
            {
                 f = Request.Form.Files.First();
                f = Request.Form.Where(a => a.Key.Equals("Image")).Select(a => a.Value) as IFormFile;
                var b = Request.Form.ToList();
                var c = b.Where(a => a.Key == "Image");
                var d = c;
                var ms = new MemoryStream();
                Request.Form.Files[0].CopyTo(ms);
                var bytes = ms.ToArray();
                prosjektDescription.Image = bytes;

            }
            
            
            prosjektDescription.ErValgt = false;

            
            
            prosjektDescription.status = 1;
            
            List<ProjectStatus> ProjectStatusList = await _context.ProjectStatus.ToListAsync();
            
            string id = User.Claims.ToList().ElementAt(0).Value;
            prosjektDescription.IdentityUserID = id;
            
            List<ProjectStatus> a = new();
            
           
            ViewData["ProjectStatuses"] = ProjectStatusList.GetEnumerator(); 

            if (ModelState.IsValid)
            {
                _context.Add(prosjektDescription);
                await _context.SaveChangesAsync();
                //TODO
                if (User.IsInRole("Bedrift") || User.IsInRole("Student"))
                {
                    return Redirect("getmyprojects");
                }
                else if (User.IsInRole("Fagansvarlig"))
                {
                    //TODO logging av feil
                    //Fagansvarleg kan ikkje legge inn prosjekt
                }

                //TODO
                return RedirectToAction(nameof(Index));
            }
            return View(prosjektDescription);
        }

        // GET: ProsjektDescriptions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription.FindAsync(id);
            if (prosjektDescription == null)
            {
                return NotFound();
            }
            if (prosjektDescription.status == 4)
            {
                //Todo send med feilmelding
                return RedirectToAction(nameof(GetMyProjects));
            }
            return View(prosjektDescription);
        }



        // POST: ProsjektDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,string submit, [Bind("Id,Navn,Prosjektgiver,Webadresse,IntroOppdragsgiver,BagrunnProsjekt,TeknologiMetoder,Arbeidsoppgaver,Annet,status")] ProsjektDescription prosjektDescription)
        {



            if (submit.Equals("Last ned PDF"))
            {
                
                byte[] FileBytes = IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription, prosjektDescription.Id + "_temp.pdf");

                

                return File(FileBytes, "application/pdf");

            }


           
            //TODO
            string id2 = User.Claims.ToList().ElementAt(0).Value;
            prosjektDescription.IdentityUserID = id2;
            
            if (!ProsjektDescriptionExists(id))
            {
                return NotFound();
            }
            
            if (id != prosjektDescription.Id)
            {
                return NotFound();
            }

            ProsjektDescription old = await _context.ProsjektDescription.FindAsync(id);
            if (old.status == 4)
            {
                return View();
            }
            if (old.Id != prosjektDescription.Id)
            {
                return NotFound();
            }

            old.Id = prosjektDescription.Id;
            old.Navn = prosjektDescription.Navn;
            old.Prosjektgiver = prosjektDescription.Prosjektgiver;
            old.Webadresse = prosjektDescription.Webadresse;
            old.IntroOppdragsgiver = prosjektDescription.IntroOppdragsgiver;
            old.BagrunnProsjekt = prosjektDescription.BagrunnProsjekt;
            old.TeknologiMetoder = prosjektDescription.TeknologiMetoder;
            old.Arbeidsoppgaver = prosjektDescription.Arbeidsoppgaver;
            old.Annet = prosjektDescription.Annet;
            
            old.status = 3;


            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Entry(prosjektDescription).State = EntityState.Detached;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsjektDescriptionExists(prosjektDescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (User.IsInRole("Bedrift") || User.IsInRole("Student"))
                {
                    return RedirectToAction(nameof(GetMyProjects));
                }

                return RedirectToAction(nameof(Index));



            }
            return View(prosjektDescription);
        }




        //TODO: Kan generalisere
        // GET: ProsjektDescriptions/Edit/5
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> EditGodkjenn(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription.FindAsync(id);
            if (prosjektDescription == null)
            {
                return NotFound();
            }
          
            return View(prosjektDescription);
        }


        //TODO: Kan generalisere
        // POST: ProsjektDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> EditGodkjenn(int id, string submit, [Bind("Id,Navn,Prosjektgiver,Webadresse,IntroOppdragsgiver,BagrunnProsjekt,TeknologiMetoder,Arbeidsoppgaver,Annet,status")] ProsjektDescription prosjektDescription)
        {
            //TODO
            if (submit.Equals("Last ned pdf"))
            {
                //IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription.Navn, prosjektDescription.Prosjektgiver, "", prosjektDescription.IntroOppdragsgiver, prosjektDescription.BagrunnProsjekt, prosjektDescription.TeknologiMetoder, prosjektDescription.Arbeidsoppgaver, prosjektDescription.Annet, "", "", "", "", "", "", "", "", prosjektDescription.Id + "_temp.pdf", prosjektDescription.Image, prosjektDescription.IndeksString);
                //TODO - remove - generalize
                byte[] FileBytes = IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription, prosjektDescription.Id + ".pdf");
               

                return File(FileBytes, "application/pdf");

            }
            
            
            if (!ProsjektDescriptionExists(id))
            {
                return NotFound();
            }
            //ProsjektDescription old = await _context.ProsjektDescription.FindAsync(id);
            //prosjektDescription.Id = id;
            if (id != prosjektDescription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ProsjektDescription old = await _context.ProsjektDescription.FindAsync(id);
                    old.Id = prosjektDescription.Id;
                    old.Navn = prosjektDescription.Navn;
                    old.Prosjektgiver = prosjektDescription.Prosjektgiver;
                    old.Webadresse = prosjektDescription.Webadresse;
                    old.IntroOppdragsgiver = prosjektDescription.IntroOppdragsgiver;
                    old.BagrunnProsjekt = prosjektDescription.BagrunnProsjekt;
                    old.TeknologiMetoder = prosjektDescription.TeknologiMetoder;
                    old.Arbeidsoppgaver = prosjektDescription.Arbeidsoppgaver;
                    old.Annet = prosjektDescription.Annet;
                    old.status = 3;


                    //_context.Entry(prosjektDescription).State = EntityState.Detached;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsjektDescriptionExists(prosjektDescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProsjektKlarForGodkjenning));
            }
            return View(prosjektDescription);
        }

       

        //TODO: Kan generalisere
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> EditFiks(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription.FindAsync(id);
            if (prosjektDescription == null)
            {
                return NotFound();
            }
            return View(prosjektDescription);
        }



        //TODO: Kan generalisere
        // POST: ProsjektDescriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> EditFiks(int id, string submit, [Bind("Id,Navn,Prosjektgiver,Webadresse,IntroOppdragsgiver,BagrunnProsjekt,TeknologiMetoder,Arbeidsoppgaver,Annet,status")] ProsjektDescription prosjektDescription)
        {
            //TODO
            if (submit.Equals("Last ned pdf"))
            {
                //IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription.Navn, prosjektDescription.Prosjektgiver, "", prosjektDescription.IntroOppdragsgiver, prosjektDescription.BagrunnProsjekt, prosjektDescription.TeknologiMetoder, prosjektDescription.Arbeidsoppgaver, prosjektDescription.Annet, "", "", "", "", "", "", "", "", prosjektDescription.Id + "_temp.pdf", prosjektDescription.Image, prosjektDescription.IndeksString);
                byte[] FileBytes = IPDFGenerator.GenerateProsjektbeskrivelse(prosjektDescription, prosjektDescription.Id + "_temp.pdf");

                

                return File(FileBytes, "application/pdf");

            }


            if (!ProsjektDescriptionExists(id))
            {
                return NotFound();
            }
            //ProsjektDescription old = await _context.ProsjektDescription.FindAsync(id);
            //prosjektDescription.Id = id;
            if (id != prosjektDescription.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    ProsjektDescription old = await _context.ProsjektDescription.FindAsync(id);
                    old.Id = prosjektDescription.Id;
                    old.Navn = prosjektDescription.Navn;
                    old.Prosjektgiver = prosjektDescription.Prosjektgiver;
                    old.Webadresse = prosjektDescription.Webadresse;
                    old.IntroOppdragsgiver = prosjektDescription.IntroOppdragsgiver;
                    old.BagrunnProsjekt = prosjektDescription.BagrunnProsjekt;
                    old.TeknologiMetoder = prosjektDescription.TeknologiMetoder;
                    old.Arbeidsoppgaver = prosjektDescription.Arbeidsoppgaver;
                    old.Annet = prosjektDescription.Annet;
                    old.status = 3;
                    

                    //_context.Entry(prosjektDescription).State = EntityState.Detached;
                    _context.Update(old);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsjektDescriptionExists(prosjektDescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProsjektVenterFiks));
            }
            return View(prosjektDescription);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Fagansvarlig, Administrator")]
        public async Task<IActionResult> Godkjenn(int id, string sentFrom)
        {
            if (!ProsjektDescriptionExists(id))
            {
                return NotFound();
            }

            //TODO
            ProsjektDescription prosjektDescription = await _context.ProsjektDescription.FindAsync(id);
            //TODO - status. 4 er godkjent

            prosjektDescription.status = 4;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prosjektDescription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProsjektDescriptionExists(prosjektDescription.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }


                switch (sentFrom)
                {
                    case "ProsjektKlarForGodkjenning":
                        return RedirectToAction(nameof(ProsjektKlarForGodkjenning));
                        break;

                    case "ProsjektVenterFiks":
                        return RedirectToAction(nameof(ProsjektVenterFiks));
                        break;
                }

                return NotFound();
            }
            return NotFound();
        }
        // GET: ProsjektDescriptions/Delete/5
        
        public async Task<IActionResult> Delete(int? id)
        {
            //TODO
            //bedrifter og studentar: can only delete with correct id. tsame with edit...
            if (id == null)
            {
                return NotFound();
            }

            var prosjektDescription = await _context.ProsjektDescription
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prosjektDescription == null)
            {
                return NotFound();
            }
            if (User.IsInRole("Fagansvarlig"))
            {
                ViewData["Tilbake"] = "Index";
            }else if (User.IsInRole("Bedrift"))
            {
                ViewData["Tilbake"] = "GetMyProjects";
            }
            else if (User.IsInRole("Student"))
            {
                ViewData["Tilbake"] = "GetMyProjects";
            }
            return View(prosjektDescription);
        }

        // POST: ProsjektDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prosjektDescription = await _context.ProsjektDescription.FindAsync(id);
            _context.ProsjektDescription.Remove(prosjektDescription);
            await _context.SaveChangesAsync();


            if (User.IsInRole("Bedrift") || User.IsInRole("Student")) { 
                return RedirectToAction(nameof(GetMyProjects));
            }
            return RedirectToAction(nameof(Index));


        }

        private bool ProsjektDescriptionExists(int id)
        {
            return _context.ProsjektDescription.Any(e => e.Id == id);
        }
    }
}
