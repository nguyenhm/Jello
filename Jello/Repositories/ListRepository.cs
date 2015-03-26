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
    public class ListRepository
    {
        string _connStr = ConfigurationManager.ConnectionStrings["JelloConnectionString"].ConnectionString;

        public List<List> GetNonArchivedList(int boardID)
        {
            List<List> lists = new List<List>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "NonArchivedListGetByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@BoardID", SqlDbType.Int).Value = boardID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lists.Add(FillModel(dr));
                    }
                }
            }
            return lists;
        }

        public void CreateNewList(int boardID, string title, int screenPosition, int creatorID, bool isArchived, DateTime creationDate)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "ListCreation",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("BoardID", boardID);
                command.Parameters.AddWithValue("Title", title);
                command.Parameters.AddWithValue("ScreenPosition", screenPosition);
                command.Parameters.AddWithValue("CreatorID", creatorID);
                command.Parameters.AddWithValue("IsArchived", isArchived);
                command.Parameters.AddWithValue("CreationDate", creationDate);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public int GetListNumber(int boardID)
        {
            int numList;

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "ListNumberGetByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@BoardID", SqlDbType.Int).Value = boardID;

                connection.Open();
                numList = Convert.ToInt32(command.ExecuteScalar()) + 1;
                return numList;
            }
        }

        public void ArchiveListByListID(int listID)
        {
            using(var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "ListArchiveByListID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@ListID", SqlDbType.Int).Value = listID;

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        private List FillModel(IDataReader dr)
        {
            var list = new List
            {
                BoardID = (int)dr["BoardID"],
                ListID = (int)dr["ListID"],
                BoardTitle = (string)dr["BoardTitle"],
                Title = dr["Title"] != DBNull.Value ? dr["Title"].ToString() : null,
                CreationDate = dr["CreationDate"] != DBNull.Value ? (DateTime)dr["CreationDate"] : DateTime.MinValue
            };
            return list;
        }
    }
}