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

        //
        // GET: /List/

        [HttpGet]
        public ActionResult Index(int boardID)
        {
            _listRepository = new ListRepository();

            var model = _listRepository.GetNonArchivedList(boardID).AsEnumerable().Select(row => new List
            {
                ListID = row.ListID,
                Title = row.Title,
                CreationDate = row.CreationDate
            }).ToList();
            return View(model);
        }

    }
}
