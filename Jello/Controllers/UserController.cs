using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jello.Models;
using MvcApplication1.Services;

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
                        Session["User"] = currentUser;
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message);
            }
            return View(model);
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

    }
}
