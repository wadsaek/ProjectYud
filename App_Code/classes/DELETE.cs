using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Summary description for DELETE
/// </summary>
public partial class Connection
{
    public int DeleteReaction(int PostId, int UserId)
    {
        int returnCode;
        using (SqlConnection con = new SqlConnection(path))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = @"DELETE FROM Reactions
                          WHERE UserId = @usID
                          AND PostId = @psID";
                cmd.Parameters.AddWithValue("@usID", UserId);
                cmd.Parameters.AddWithValue("@psId", PostId);
                con.Open();
                returnCode = cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        return returnCode;
    }

    public int DeleteComment(int CommentId, int UserId, string mappedAdminPath)
    {
        int returnCode;
        using (SqlConnection con = new SqlConnection(path))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.Parameters.Clear();

                string jsonString = File.ReadAllText(mappedAdminPath);
                int[] admins = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(jsonString);
                cmd.CommandText = @"DELETE FROM Comments 
                            WHERE Id = @CommId";
                cmd.Parameters.AddWithValue("@CommId", CommentId);
                if (!admins.Contains(UserId))
                {
                    cmd.CommandText += @"
                                AND AuthorID = @AuthID";
                    cmd.Parameters.AddWithValue("@AuthID", UserId);
                }
                con.Open();
                returnCode = cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        return returnCode;
        
    }
    public int DeleteSong(int songId)
    {
        int returnCode;
        using (SqlConnection con = new SqlConnection(path))
        {
            using(SqlCommand  cmd = con.CreateCommand())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = @"DELETE FROM Comments
                                    WHERE PostID = @songid;
                                    DELETE FROM Reactions
                                    WHERE PostId = @songid;
                                    DELETE FROM Songs
                                    WHERE Id = @songid";
                cmd.Parameters.AddWithValue("@songid", songId);
                con.Open();
                returnCode = cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        return returnCode;
    }

    /// <summary>
    /// user deletion for admins
    /// </summary>
    /// <param name="userId">the id of the person to be deleted</param>
    /// <param name="senderId">the id of the person who sent the request</param>
    /// <param name="admins">the List containing all the admins</param>
    /// <returns>-1 if the sender is not an admin.
    /// otherwise the numbers of rows affected</returns>
    public int DeleteUser(int userId, int senderId, List<int> admins)
    {
        if (!admins.Contains(senderId))
        {
            return -1;
        }
        return DeleteUser(userId);
    }
    /// <summary>
    /// soft deletion
    /// </summary>
    /// <param name="userId"></param>
    /// <returns> number of raws affected.
    /// should be 1.</returns>
    public int DeleteUser(int userId)
    {
        int returnCode;
        using (SqlConnection con = new SqlConnection(path))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.Parameters.Clear();

                cmd.CommandText = $@"UPDATE users
                                SET pfp = 'default.png',
                                    password = '',
                                    username = '{Consts.DELETED_USERNAME}'
                                WHERE id = @userid";
                cmd.Parameters.AddWithValue("@userid", userId);
                con.Open();
                returnCode = cmd.ExecuteNonQuery();
            }
            con.Close();
        }
        return returnCode;
    }
}