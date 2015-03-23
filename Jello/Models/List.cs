using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jello.Models
{
    public class List
    {
        public List()
        {

        }

        public int ListID { get; set; }
        public int BoardID { get; set; }
        public string Title { get; set; }
        public int ScreenPosition { get; set; }
        public int CreatorID { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsArchived { get; set; }
        public ICollection<Card> CardList { get; set; }
    }

    public class CreateListModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        public int BoardID { get; set; }
    }
}