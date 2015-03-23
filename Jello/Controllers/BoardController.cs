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
            _boardRepository = new BoardRepository();
            int boardID = Convert.ToInt32(Session["BoardID"]);

            var model = _boardRepository.GetBoardMemberByBoardID(boardID).AsEnumerable().Select(row => new User
            {
                BoardID = row.BoardID,
                RoleID = row.RoleID,
                RoleDescription = row.RoleDescription,
                FullName = row.FullName
            }).ToList();
            return View(model);
        }

        [HttpGet]
        public ActionResult AssignMember()
        {
            _userRepository = new UserRepository();
            _boardRepository = new BoardRepository();

            int boardID = Convert.ToInt32(Request["boardID"]);

            List<SelectListItem> NonMemberList = _userRepository.GetNonMemberByBoardID(boardID).AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.UserID.ToString(),
                Text = row.FName +" "+ row.LName
            }).ToList();

            List<SelectListItem> RoleList = _boardRepository.GetAllRole().AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.RoleID.ToString(),
                Text = row.RoleDescription
            }).ToList();

            SelectList sl = new SelectList(NonMemberList, "Value", "Text");
            ViewBag.NonMemberList = sl;

            SelectList slR = new SelectList(RoleList, "Value", "Text");
            ViewBag.RoleList = slR;

            return View();
        }

        [HttpPost]
        public ActionResult AssignMember(AssignMemberModel model, FormCollection form)
        {
            int boardID = Convert.ToInt32(Request["boardID"]);
            _boardRepository = new BoardRepository();
            _userRepository = new UserRepository();

            try
            {
                if (ModelState.IsValid)
                {
                    model.UserID = Convert.ToInt32(form["NonMemberList"]);
                    model.RoleID = Convert.ToInt32(form["RoleList"]);
                    _boardRepository.AssignMember(boardID, model.UserID, model.RoleID);
                    return RedirectToAction("ViewMembers", "Board", new { boardid = boardID });
                }
            }
            catch
            {
                ModelState.AddModelError("Error", "Please select a Member and assign Role");
            }
            

            List<SelectListItem> NonMemberList = _userRepository.GetNonMemberByBoardID(boardID).AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.UserID.ToString(),
                Text = row.FName + " " + row.LName
            }).ToList();

            List<SelectListItem> RoleList = _boardRepository.GetAllRole().AsEnumerable().Select(row => new SelectListItem
            {
                Value = row.RoleID.ToString(),
                Text = row.RoleDescription
            }).ToList();

            SelectList sl = new SelectList(NonMemberList, "Value", "Text");
            ViewBag.NonMemberList = sl;

            SelectList slR = new SelectList(RoleList, "Value", "Text");
            ViewBag.RoleList = slR;

            return View(model);
        }

    }
}
