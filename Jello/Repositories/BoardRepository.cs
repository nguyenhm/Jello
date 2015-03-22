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
    public class BoardRepository
    {
        string _connStr = ConfigurationManager.ConnectionStrings["JelloConnectionString"].ConnectionString;

        public List<Board> GetCurrentBoards(int userID)
        {
            List<Board> boards = new List<Board>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardGetByUserID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        boards.Add(FillModel(dr));
                    }
                }
            }
            return boards;
        }

        public void CreateNewBoard(string title, string descrition, int creatorID, bool isPublic)
        {
            int boardID;

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardCreation",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("Title", title);
                command.Parameters.AddWithValue("Description", descrition);
                command.Parameters.AddWithValue("CreatorID", creatorID);
                command.Parameters.AddWithValue("isPublic", isPublic);

                connection.Open();
                //command.ExecuteNonQuery();
                boardID = Convert.ToInt32(command.ExecuteScalar());

                string[] titleList = new string[] { "To Do", "In Progress", "Complete" };
                ListRepository _listRepository = new ListRepository();
                for (int i = 0; i < titleList.Length; i++)
                {
                    string listTitle = titleList[i];
                    int screenPosition = i + 1;
                    bool isArchived = false;
                    DateTime creationDate = System.DateTime.Now;
                    _listRepository.CreateNewList(boardID, listTitle, screenPosition, creatorID, isArchived, creationDate);
                }
            }
        }

        private Board FillModel(IDataReader dr)
        {
            var board = new Board
            {
                BoardID = (int)dr["BoardID"],
                Title = dr["Title"] != DBNull.Value ? dr["Title"].ToString() : null,
                FullName = dr["FullName"] != DBNull.Value ? dr["FullName"].ToString() : null,
                Description = (string)dr["Description"],
                IsPublic = (bool)dr["IsPublic"]
            };
            return board;
        }
    }
}