using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jello.Models;
using MvcApplication1.Services;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Security;

namespace Jello.Controllers
{
    public class UserController : Controller
    {
        UserRepository _usersRepository;

        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /User/Login

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Jello.Models.UserModel model)
        {
            // instantiate user repository
            //call method from UP  to validate user

            _usersRepository = new UserRepository();
            try
            {               
                if (ModelState.IsValid)
                {
                    var isLoginValid = _usersRepository.IsLoginValid(model.Email, model.Password);
                    if (isLoginValid)
                    {
                        var currentUser = _usersRepository.GetUserByEmail(model.Email);
                        //FormsAuthentication.SetAuthCookie(u.Username, false);
                        Session["User"] = currentUser;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        throw new Exception("Incorrect Password");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View(model);
        }

        public ActionResult LogOff()
        {
            Session.Abandon();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Jello.Models.RegisterViewModel model)
        {
            _usersRepository = new UserRepository();

            var crypto = new SimpleCrypto.PBKDF2();
            var encryptPass = crypto.Compute(model.Password);

            if(ModelState.IsValid)
            {
                _usersRepository.CreateNewAccount(model.UserName, model.FirstName, model.LastName, model.Email, encryptPass, crypto.Salt);
                return RedirectToAction("Login", "User");
            }
            return View(model);
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return PartialView("_ExternalLoginListPartial", OAuthWebSecurity.RegisteredClientData);
        }

    }
}
