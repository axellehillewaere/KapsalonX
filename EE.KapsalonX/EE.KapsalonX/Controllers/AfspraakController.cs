﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EE.KapsalonX.Data;
using EE.KapsalonX.Domain.Boeken;
using EE.KapsalonX.Web.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace EE.KapsalonX.Web.Controllers
{
    public class AfspraakController : Controller
    {
        private ApplicationDbContext _context;
        public AfspraakController(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Opvullen van behandelinglijst
        List<BehandelingVm> BehandelingenDames = new List<BehandelingVm>
        {
            new BehandelingVm { Behandeling = "KORT HAAR - Knippen", Tijdsduur = new TimeSpan(00,30,00)},
            new BehandelingVm { Behandeling = "KORT HAAR - Kleuren", Tijdsduur = new TimeSpan(00,45,00)},
            new BehandelingVm { Behandeling = "KORT HAAR - Brushing", Tijdsduur = new TimeSpan(00,30,00)},
            new BehandelingVm { Behandeling = "KORT HAAR - Knippen + kleuren", Tijdsduur = new TimeSpan(01,15,00)},
            new BehandelingVm { Behandeling = "KORT HAAR - Knippen + kleuren + brushing", Tijdsduur = new TimeSpan(01,45,00)},

            new BehandelingVm { Behandeling = "LANG HAAR - Knippen", Tijdsduur = new TimeSpan(00,40,00)},
            new BehandelingVm { Behandeling = "LANG HAAR - Kleuren", Tijdsduur = new TimeSpan(01,00,00)},
            new BehandelingVm { Behandeling = "LANG HAAR - Brushing", Tijdsduur = new TimeSpan(00,40,00)},
            new BehandelingVm { Behandeling = "LANG HAAR - Knippen + kleuren", Tijdsduur = new TimeSpan(01,40,00)},
            new BehandelingVm { Behandeling = "LANG HAAR - Knippen + kleuren + brushing", Tijdsduur = new TimeSpan(02,20,00)}
        };
        List<BehandelingVm> BehandelingenHeren = new List<BehandelingVm>
        {
            new BehandelingVm { Behandeling = "Snit", Tijdsduur = new TimeSpan(00,30,00) },
            new BehandelingVm { Behandeling = "Tondeuse", Tijdsduur = new TimeSpan(00,30,00) },
            new BehandelingVm { Behandeling = "Knippen + kleuren", Tijdsduur = new TimeSpan(01,00,00)}
        };
        List<BehandelingVm> BehandelingenKinderen = new List<BehandelingVm>
        {
            new BehandelingVm { Behandeling = "Snit meisjes", Tijdsduur = new TimeSpan(00,30,00)},
            new BehandelingVm { Behandeling = "Snit jongens", Tijdsduur = new TimeSpan(00,30,00)}
        };
        #endregion

        [HttpGet]
        public IActionResult Index(int? stapId)
        {
            AfspraakVm viewModel = new AfspraakVm(stapId.GetValueOrDefault(1));
            viewModel.BehandelingenDames = BehandelingenDames;
            viewModel.BehandelingenHeren = BehandelingenHeren;
            viewModel.BehandelingenKinderen = BehandelingenKinderen;
            WaardenNaarViewModel(viewModel);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(AfspraakVm viewModel)
        {
            if (viewModel.Stap == 4)
            {
                if (ModelState.IsValid)
                {
                    viewModel.Stap++;
                    return RedirectToAction("Overzicht", viewModel);
                }
                else
                {
                    viewModel.BehandelingenDames = BehandelingenDames;
                    viewModel.BehandelingenHeren = BehandelingenHeren;
                    viewModel.BehandelingenKinderen = BehandelingenKinderen;
                    return View(viewModel);
                }
            }
            else
            {
                viewModel.Stap++;
            }
            WaardenNaarTempData(viewModel);
            return RedirectToAction("Index", new { stapId = viewModel.Stap, viewModel });
        }

        [HttpGet]
        public IActionResult Overzicht(int? stapId, AfspraakVm viewModel)
        {
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Overzicht(AfspraakVm viewModel)
        {
            var nieuweKlant = new Klant
            {
                Voornaam = viewModel.Voornaam,
                Achternaam = viewModel.Achternaam,
                Telefoonnummer = viewModel.Telefoonnummer,
                Emailadres = viewModel.Emailadres
            };
            _context.Add(nieuweKlant);
            _context.SaveChanges();

            var nieuweBehandeling = new Behandeling
            {
                Geslacht = viewModel.Geslacht,
                GekozenBehandeling = viewModel.Behandeling
            };
            _context.Add(nieuweBehandeling);
            _context.SaveChanges();

            var nieuweAfspraak = new Afspraak();
            nieuweAfspraak.KlantGegevens = nieuweKlant;
            nieuweAfspraak.BehandelingGegevens = nieuweBehandeling;
            nieuweAfspraak.Datum = viewModel.Datum;
            nieuweAfspraak.Tijdstip = viewModel.Tijdstip;
            nieuweAfspraak.Opmerking = viewModel.Opmerkingen;
            _context.Add(nieuweAfspraak);
            _context.SaveChanges();

            //HIER LATER VERSTUREN VAN MAIL NAAR KLANT MET GEGEVENS AFSPRAAK
            return new RedirectToActionResult("Bevestiging", "Afspraak", viewModel);
        }

        
        public IActionResult Bevestiging (AfspraakVm viewModel)
        {
            return View(viewModel);
        }



        private void WaardenNaarViewModel(AfspraakVm viewModel)
        {
            viewModel.Geslacht = TempData["Geslacht"]?.ToString();
            viewModel.Behandeling = TempData["Behandeling"]?.ToString();
            viewModel.Datum = TempData["Datum"]?.ToString();
            viewModel.Tijdstip = TempData["Tijdstip"]?.ToString();

            viewModel.Voornaam = TempData["Voornaam"]?.ToString();
            viewModel.Achternaam = TempData["Achternaam"]?.ToString();
            viewModel.Telefoonnummer = TempData["Telefoonnummer"]?.ToString();
            viewModel.Emailadres = TempData["Emailadres"]?.ToString();
            viewModel.Opmerkingen = TempData["Opmerkingen"]?.ToString();
        }

        private void WaardenNaarTempData(AfspraakVm viewModel)
        {
            TempData["Geslacht"] = viewModel.Geslacht;
            TempData["Behandeling"] = viewModel.Behandeling?.ToString();
            TempData["Datum"] = viewModel.Date.ToShortDateString();
            TempData["Tijdstip"] = viewModel.Time.ToShortTimeString();

            TempData["Voornaam"] = viewModel.Voornaam;
            TempData["Achternaam"] = viewModel.Achternaam;
            TempData["Telefoonnummer"] = viewModel.Telefoonnummer;
            TempData["Emailadres"] = viewModel.Emailadres;
            TempData["Opmerkingen"] = viewModel.Opmerkingen;

        }
    }
}