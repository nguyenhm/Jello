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

        public Board GetBoardByBoardID(int boardID)
        {
            Board board = new Board();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardGetByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@BoardID", SqlDbType.Int).Value = boardID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        board = FillModel(dr);
                    }
                }
            }
            return board;
        }

        public int CreateNewBoard(string title, string descrition, int creatorID, bool isPublic)
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

            return boardID;
        }

        public void UpdateBoardByBoardID(int boardID, string title, string description, bool isPublic)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardUpdateByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BoardID", boardID);
                command.Parameters.AddWithValue("@Title", title);
                command.Parameters.AddWithValue("@Description", description);
                command.Parameters.AddWithValue("@isPublic", isPublic);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteBoardByBoardID(int boardID)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardDeleteByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BoardID", boardID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void RemoveMemberByBoardIDUserID(int boardID, int userID)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "MemberRemoveByBoardIDUserID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("@BoardID", boardID);
                command.Parameters.AddWithValue("@UserID", userID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<User> GetBoardMemberByBoardID(int boardID)
        {
            List<User> boardMembers = new List<User>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "BoardMemberGetByBoardID",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add("@BoardID", SqlDbType.Int).Value = boardID;

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        boardMembers.Add(FillBoardMemberModel(dr));
                    }
                }
            }
            return boardMembers;
        }

        public List<Role> GetAllRole()
        {
            List<Role> roles = new List<Role>();

            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "RoleGetAll",
                    CommandType = CommandType.StoredProcedure
                };

                connection.Open();
                using (IDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        roles.Add(FillRoleModel(dr));
                    }
                }
            }
            return roles;
        }

        public void AssignMember(int boardID, int userID, int roleID)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandText = "MemberAssignment",
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.AddWithValue("BoardID", boardID);
                command.Parameters.AddWithValue("UserID", userID);
                command.Parameters.AddWithValue("RoleID", roleID);

                connection.Open();
                command.ExecuteNonQuery();
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

        private User FillBoardMemberModel(IDataReader dr)
        {
            var boardMembers = new User
            {
                BoardID = (int)dr["BoardID"],
                UserID = (int)dr["UserID"],
                RoleDescription = (string)dr["RoleDesciption"],
                FullName = (string)dr["FullName"]
            };
            return boardMembers;
        }

        private Role FillRoleModel(IDataReader dr)
        {
            var role = new Role
            {
                RoleID = (int)dr["RoleID"],
                RoleDescription = (string)dr["RoleDesciption"]
            };
            return role;
        }
    }
}