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
                var isLoginValid = _usersRepository.IsLoginValid(model.Email, model.Password);
                var currentUser = _usersRepository.GetUserByEmail(model.Email);

                if (ModelState.IsValid)
                {
                    if (isLoginValid)
                    {
                        Session["UserID"] = currentUser.UserID;
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

            _usersRepository.CreateNewAccount(model.UserName, model.FirstName, model.LastName, model.Email, encryptPass, crypto.Salt);


            if(ModelState.IsValid)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

    }
}
