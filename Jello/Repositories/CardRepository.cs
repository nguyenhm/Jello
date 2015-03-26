using Jello.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Jello.Repositories
{
    public class CardRepository
    {
        string _connStr = ConfigurationManager.ConnectionStrings["JelloConnectionString"].ConnectionString;

        public List<Card> GetCardByListID(int listID)
        {
            List<Card> cards = new List<Card>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "CardGetByListID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        cards.Add(FillModel(dr));
                    }
                }
            }
            return cards;
        }

        public void CreateNewCard(int listID, string description, int creatorID, DateTime creationDate, DateTime updatedDate)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "CardCreation",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("ListID", listID);
                command.Parameters.AddWithValue("Description", description);
                command.Parameters.AddWithValue("CreatorID", creatorID);
                command.Parameters.AddWithValue("CreationDate", creationDate);
                command.Parameters.AddWithValue("UpdatedDate", updatedDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateListID(int finalListID, int cardID)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "CardListIDUpdateByCardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("ListID", finalListID);
                command.Parameters.AddWithValue("CardID", cardID);                
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public Card FillModel(IDataReader dr)
        {
            var card = new Card 
            {
                CardID = (int)dr["CardID"],
                Description = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : null,
                CreatorName = dr["CreatorName"] != DBNull.Value ? dr["CreatorName"].ToString() : null,
                Priority = dr["Priority"] != DBNull.Value ? (int)dr["Priority"] : 0,
                CreatorID = (int)dr["CreatorID"],
                CreationDate = (DateTime)dr["CreationDate"],
                UpdatedDate = (DateTime)dr["UpdatedDate"]
            };
            return card;
        }
    }
}