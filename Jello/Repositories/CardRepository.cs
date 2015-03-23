using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Jello.Repositories
{
    public class CardRepository
    {
        string _connStr = ConfigurationManager.ConnectionStrings["JelloConnectionString"].ConnectionString;

        //public List<Card> GetCardByListID()
        //{

        //}
    }
}