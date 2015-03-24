using Jello.Models;
using Jello.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jello.Controllers
{
    public class ListController : Controller
    {
        ListRepository _listRepository;
        CardRepository _cardRepository;

        //
        // GET: /List/

        [HttpGet]
        public ActionResult Index(int boardID)
        {
            _listRepository = new ListRepository();
            _cardRepository = new CardRepository();

            boardID = Convert.ToInt32(Request["boardID"]);
            Session["BoardID"] = boardID;

            var model = _listRepository.GetNonArchivedList(boardID).AsEnumerable().Select(row => new List
            {
                BoardID = row.BoardID,
                ListID = row.ListID,
                Title = row.Title,
                CreationDate = row.CreationDate,
                CardList = _cardRepository.GetCardByListID(row.ListID).AsEnumerable().Select(rowCard => new Card
                {
                    CardID = rowCard.CardID,
                    BoardID = boardID,
                    CreatorName = rowCard.CreatorName,
                    Description = rowCard.Description,
                    CreationDate = rowCard.CreationDate,
                    UpdatedDate = rowCard.UpdatedDate                    
                }).ToList()
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult CreateList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateList(CreateListModel model)
        {
            _listRepository = new ListRepository();
            var user = (User)Session["User"];

            if(ModelState.IsValid)
            {
                int boardID = Convert.ToInt32(Session["BoardID"]);
                string title = model.Title;
                int screenPosition = _listRepository.GetListNumber(boardID);
                int creatorID = user.UserID;
                bool isArchived = false;
                DateTime creationDate = System.DateTime.Now;

                _listRepository.CreateNewList(boardID, title, screenPosition, creatorID, isArchived, creationDate);

                return RedirectToAction("Index", "List", new { boardid = boardID });
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult ArchiveList(int listID, int boardID)
        {
            _listRepository = new ListRepository();
            _listRepository.ArchiveListByListID(listID);
            return RedirectToAction("Index", "List", new { boardid = boardID });
        }

    }
}
