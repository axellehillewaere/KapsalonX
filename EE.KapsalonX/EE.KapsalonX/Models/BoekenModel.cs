﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EE.KapsalonX.Web.Models
{
    public class BoekenModel
    {
        public int Stap { get; set; }

        [Display(Name = "Behandeling voor: ")]
        public string Geslacht { get; set; }
        public string Behandeling { get; set; }
        public string Datum { get; set; }
        public string Tijdstip { get; set; }

        [Required]
        public List<SelectListItem> Behandelingen { get; set; }
        public List<BehandelingModel> BehandelingenDames { get; set; }
        public List<BehandelingModel> BehandelingenHeren { get; set; }
        public List<BehandelingModel> BehandelingenKinderen { get; set; }

        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Tijdstip")]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "Vul je voornaam in a.u.b.")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Vul je achternaam in a.u.b.")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Vul je telefoonnummer in a.u.b.")]
        public string Telefoonnummer { get; set; }

        [Required(ErrorMessage = "Vul je e-mailadres in a.u.b.")]
        public string Emailadres { get; set; }

        public string Opmerkingen { get; set; }


        public BoekenModel()
        {
        }

        public BoekenModel(int stap)
        {
            Stap = stap;
        }
    }
}
