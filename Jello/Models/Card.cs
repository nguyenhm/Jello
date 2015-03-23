using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Jello.Models
{
    public class Card
    {
        public Card()
        {
            WorkerName = "";
        }
        public int BoardID { get; set; }
        public int CardID { get; set; }
        public int ListID { get; set; }


        [Required]
        public string Description { get; set; }
        public string WorkerName { get; set; }
        public string CreatorName { get; set; }

        public int Priority { get; set; }
        public int CreatorID { get; set; }
        public int WorkerID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}