using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jello.Models
{
    public class Board
    {
        public Board()
        {
        }

        public int BoardID { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
    }

    public class CreateBoardModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "IsPublic")]
        public bool IsPublic { get; set; }
    }

    public class EditBoardModel
    {
        public int BoardID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "IsPublic")]
        public bool IsPublic { get; set; }
    }

    //public class BoardMember
    //{
    //    public BoardMember()
    //    {
    //    }

    //    public BoardMember(int boardID, int userID)
    //    {
    //        BoardID = boardID;
    //        UserID = userID;
    //    }

    //    public int BoardID { get; set; }
    //    [Required]
    //    public int UserID { get; set; }
    //    [Required]
    //    public int RoleID { get; set; }
    //    public string RoleDescription { get; set; }
    //    public string FullName { get; set; }

    //}
}