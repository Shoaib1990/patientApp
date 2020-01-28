using System;
using Microsoft.AspNetCore.Mvc;
using patientApp.Models;


namespace patientApp.Controllers{

    public class LoginController: Controller{
        
        public IActionResult Index() {
            return View();
        }

        public IActionResult Submit(string myUsername, string myPassword) {
            WebLogin webLogin = new WebLogin("Server=localhost; Database=grades;Uid=shoaib;Pwd=password;SslMode=none;", HttpContext);
            webLogin.username = myUsername;
            webLogin.password = myPassword;

            // do I have access?
            if (webLogin.unlock()) {
                // access granted
                return RedirectToAction("Index","Home");
            } else {
                // access denied
                ViewData["feedback"] = "Incorrect login. Please try again...";
            }
           
            return View("Index");
        }


    }
}