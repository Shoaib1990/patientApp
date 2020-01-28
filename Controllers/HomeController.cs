using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using patientApp.Models;

namespace patientApp.Controllers{
    public class HomeController : Controller{
        
        public IActionResult Index(){
            if (HttpContext.Session.GetString("auth") != "true") {
                return RedirectToAction("Index","Login");
            }
            return View();
        }

        public IActionResult Diary(){
            patientManager patientManager = new patientManager();
            patientManager.viewPatientDiary();
            return View(patientManager);
        }

        public IActionResult Add() {
            Patient patient = new Patient();
            return View(patient);
        }

        [HttpPost]
        public IActionResult AddSubmit(Patient patient){
            // data sanitization
            if (!ModelState.IsValid) return RedirectToAction("Index");
            patient.create();
            return Redirect("Index");
        }


        public IActionResult Delete() {
            patientManager patientManager = new patientManager();
            return View(patientManager);
        }

        public IActionResult DeleteSubmit(int OPD) {
            patientManager patientManager = new patientManager();
            Patient patient = patientManager.viewPatientDiary(OPD);
            patient.delete();
            return RedirectToAction("Index");
        }


    }
}
