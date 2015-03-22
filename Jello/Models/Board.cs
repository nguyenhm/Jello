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
}