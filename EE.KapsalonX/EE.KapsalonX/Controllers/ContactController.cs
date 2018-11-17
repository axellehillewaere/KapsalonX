﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EE.KapsalonX.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EE.KapsalonX.Web.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            var viewModel = new ContactIndexVm();
            return View(viewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(ContactIndexVm viewModel)
        {
            if (ModelState.IsValid)
            {
                TempData[Constants.SuccessMessage] = $@"<p>Bedankt voor uw bericht, heer/mevrouw {viewModel.Achternaam}.<br /><br />
                                                      We trachten zo snel mogelijk een antwoord terug te sturen.<br /><br />
                                                      Het KapsalonX-team";
                return new RedirectToActionResult("Index", "Contact", null);
            }
            else
            {
                return View(viewModel);
            }
        }
    }
}