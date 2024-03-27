using System;
using System.Data.SqlClient;
using System.Data;

/// <summary>
/// Summary description for RetrieveComments
/// </summary>
public partial class Connection
{
    /// <summary>
    /// unchecked
    /// </summary>
    /// <param name="RequesterId"></param>
    /// <param name="TrueIfPost"></param>
    /// <returns>all the commends of a User OR all the comments under a song</returns>
    public DataTable RetrieveComments(int RequesterId, bool TrueIfPost)
    {
        DataTable table = new DataTable();
        DataColumn column;
        //all the comment data
        column = new DataColumn("id", Type.GetType("System.Int32"));
        table.Columns.Add(column);
        column = new DataColumn("PostID", Type.GetType("System.Int32"));
        table.Columns.Add(column);
        column = new DataColumn("Text", Type.GetType("System.String"));
        table.Columns.Add(column);
        column = new DataColumn("Time", Type.GetType("System.DateTime"));
        table.Columns.Add(column);
        //all the associated user data
        column = new DataColumn("userId", Type.GetType("System.Int32"));
        table.Columns.Add(column);
        column = new DataColumn("pfp", Type.GetType("System.String"));
        table.Columns.Add(column);
        column = new DataColumn("username", Type.GetType("System.String"));
        table.Columns.Add(column);

        string type = TrueIfPost ? "PostID" : "AuthorID";
       
        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = string.Format(@"SELECT 
                                                Comments.Id as Id,
                                                Comments.CommentText as Text,
                                                Comments.Time as Time,
                                                Comments.PostID as Postid,
                                                users.username as username,
                                                users.pfp as pfp,
                                                users.id as userId
                                                FROM Comments
                                                INNER JOIN users 
                                                on Comments.AuthorID = users.id
                                                WHERE {0} = {1}
                                                ORDER BY Comments.Time desc", type, RequesterId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DataRow row = table.NewRow();
                            row["id"] = reader["Id"];
                            row["Text"] = reader["Text"];
                            row["Time"] = reader["Time"];
                            row["UserId"] = reader["userId"];
                            row["pfp"] = reader["pfp"];
                            row["username"] = reader["username"];
                            row["PostID"] = reader["Postid"];
                            table.Rows.Add(row);
                        }
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return table;
    }
}