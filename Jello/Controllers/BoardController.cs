using Jello.Models;
using Jello.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Jello.Controllers
{
    public class BoardController : Controller
    {
        //
        // GET: /Board/

        BoardRepository _boardRepository;
        UserRepository _userRepository;

        [HttpGet]
        public ActionResult Index()
        {
            _boardRepository = new BoardRepository();
            var user = (User)Session["User"];

            if(user != null)
            {
                var model = _boardRepository.GetCurrentBoards(user.UserID).AsEnumerable().Select(row => new Board
                {
                    BoardID = row.BoardID,
                    Title = row.Title,
                    Description = row.Description,
                    FullName = row.FullName
                }).ToList();
                return View(model);
            }
            return View();         
        }

        [HttpGet]
        public ActionResult CreateBoard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBoard(CreateBoardModel model)
        {
            _boardRepository = new BoardRepository();
            var user = (User)Session["User"];

            if(ModelState.IsValid)
            {
                _boardRepository.CreateNewBoard(model.Title, model.Description, user.UserID, model.IsPublic);
                return RedirectToAction("Index", "Board");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult EditBoard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditBoard(EditBoardModel model)
        {
            _boardRepository = new BoardRepository();
            int boardID = Convert.ToInt32(Request["boardID"]);

            if(ModelState.IsValid)
            {
                _boardRepository.UpdateBoardByBoardID(boardID, model.Title, model.Description, model.IsPublic);
                return RedirectToAction("Index", "Board");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult CloseBoard()
        {
            _boardRepository = new BoardRepository();
            int boardID = Convert.ToInt32(Request["boardID"]);
            _boardRepository.DeleteBoardByBoardID(boardID);
            return RedirectToAction("Index", "Board");
        }

        [HttpGet]
        public ActionResult ViewMembers()
        {
            _userRepository = new UserRepository();
            int boardID = Convert.ToInt32(Session["BoardID"]);

            var model = _userRepository.GetBoardMemberByBoardID(boardID).AsEnumerable().Select(row => new User
            {
                BoardID = row.BoardID,
                RoleID = row.RoleID,
                RoleDescription = row.RoleDescription,
                FullName = row.FullName
            }).ToList();
            return View(model);
        }

    }
}
