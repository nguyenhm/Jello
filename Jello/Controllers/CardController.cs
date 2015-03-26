using Jello.Models;
using Jello.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jello.Controllers
{
    public class CardController : Controller
    {
        CardRepository _cardRepository;

        //
        // GET: /Card/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateCard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCard(CreateCardModel model)
        {
            _cardRepository = new CardRepository();
            if(ModelState.IsValid)
            {
                var user = new User();
                user = (User)Session["User"];

                int listID = Convert.ToInt32(Request["listID"]);
                string description = model.Description;
                int creatorID = user.UserID;
                DateTime creationDate = System.DateTime.Now;
                DateTime updatedDate = System.DateTime.Now;
                _cardRepository.CreateNewCard(listID, description, creatorID, creationDate, updatedDate);
                return RedirectToAction("Index", "List", new { boardID = Convert.ToInt32(Request["boardID"]) });
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateListID(int finalListID, int cardID)
        {
            _cardRepository = new CardRepository();
            _cardRepository.UpdateListID(finalListID, cardID);
            return Content("");
        }
    }
}
