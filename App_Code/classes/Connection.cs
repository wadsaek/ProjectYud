using System;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Connection
/// </summary>
public partial class Connection
{
    
    readonly private string path = 
    "Data Source=(LocalDB)\\MSSQLLocalDB;"+
    "AttachDbFilename=";

    public Connection(string MappedPath)
    {
        path += MappedPath;
    }

    /// <summary>
    /// uploads a song
    /// </summary>
    /// <param name="link">the 22 symbols from spotify</param>
    /// <param name="date">self-explanatory</param>
    /// <returns>the number of Songs uploaded(should be 1 if worked, 0 if didn't)</returns>
    public int InputSongData(string link, DateTime date)
    {
        int code;

        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"
                             INSERT INTO Songs
                             (Date,Code)
                             VALUES
                             (@songDate,@SongLink)
                             ";
                command.Parameters.AddWithValue("@songDate", date);
                command.Parameters.AddWithValue("@SongLink", link);
                connection.Open();
                code = command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return code;

    }
    
    /// <summary>
    /// Registration
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <param name="mail"></param>
    /// <returns>1 if all went good, -2 if a user with the same mail exists, -3 if a user with the same username exists, -5 if both</returns>
    public int AddUser(string username, string password, string mail)
    {
        int resultCode = 0;
        if(ExistsUserWithEmail(mail))
        {
            resultCode -=2;
        }
        if(ExistsUserWithUserName(username))
        {
            resultCode -=3;
        }
        if (resultCode != 0)
        {
            return resultCode;
        }
        string salt = Security.RandomString(32);
        string salted = Security.EncryptPassword(password, salt);

        using (SqlConnection connection = new SqlConnection(path))
        {

            using (SqlCommand cmd = connection.CreateCommand())
            {
                cmd.Parameters.Clear();
                cmd.CommandText = @"INSERT INTO users (username,password,salt,email) VALUES
                                    (@username, @password, @salt, @mail );
                                    ";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", salted);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@mail", mail);
                connection.Open();
                resultCode = cmd.ExecuteNonQuery();
            }
            connection.Close();
            
        }
        return resultCode;
    }

    /// <summary>
    /// Error codes:
    /// id -2 no such user
    /// id -3 incorrect password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns>the user</returns>
    public UserInfo Login(string username, string password, out int successCode)
    {
        string salt ="";
        int id;
        string email, pfpAdress;
        using (SqlConnection con = new SqlConnection(path))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = @"SELECT salt FROM users 
                                    WHERE username= @usern";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@usern", username);
                con.Open();
                using (SqlDataReader saltreader = cmd.ExecuteReader())
                {
                    if (!saltreader.HasRows)
                    {
                        con.Close();
                        UserInfo error = UserInfo.Empty;
                        successCode = -2;//user not registred
                        return error;
                    }
                    while (saltreader.Read())
                    {
                        salt = saltreader["salt"].ToString();
                    }
                    saltreader.Close();
                }
                cmd.CommandText = @"SELECT id,username,email,pfp FROM users
                                    WHERE username=@usern AND password = @passw";
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@passw", Security.EncryptPassword(password, salt));
                cmd.Parameters.AddWithValue("@usern", username);
                
                using (SqlDataReader userReader = cmd.ExecuteReader())
                {
                    if (!userReader.HasRows)
                    {
                        userReader.Close();
                        con.Close();
                        UserInfo error = UserInfo.Empty;
                        successCode = -3;//password doesn't match
                        return error;
                    }
                    //exists a user with this username and this password
                    userReader.Read();
                    pfpAdress = userReader["pfp"].ToString();
                    id = (int)userReader["Id"];
                    username = (string)userReader["username"];
                    email = (string)userReader["email"];
                    userReader.Close();
                }
            }
            con.Close();
        }
        successCode = 1;
        return new UserInfo(id, username, email, pfpAdress);
    }
    
    /// <summary>
    /// adds a comment
    /// </summary>
    /// <param name="SongId">under which song is the comment located</param>
    /// <param name="Text">comment text</param>
    /// <param name="UserId">who added the comment</param>
    /// <param name="timeOfInput"> when was the comment added</param>
    /// <returns></returns>
    public (int,int) AddComment(int SongId, string Text, int UserId, DateTime timeOfInput)
    {
        using (SqlConnection connection = new SqlConnection(path))
        {
            //should be refactored into one SqlCommand
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"
                             INSERT INTO Comments
                             (CommentText,AuthorID,PostID,Time)
                             VALUES
                             (@text,@authID, @postID, @time)
                             ";
                command.Parameters.AddWithValue("@time", timeOfInput);
                command.Parameters.AddWithValue("@authID", UserId);
                command.Parameters.AddWithValue("@postID", SongId);
                command.Parameters.AddWithValue("@text", Text);
                connection.Open();
                int code = command.ExecuteNonQuery();
                if (code == 0)
                {
                    connection.Close();
                    return (0, -1);
                }
                command.CommandText = @"
                                     SELECT TOP 1 Id FROM Comments
                                     WHERE PostID = @postID 
                                     AND AuthorId = @authID
                                     AND time = @time";
                int commentId = -1;
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        commentId = (int)reader["Id"];
                    }
                }
                connection.Close();
                return (code, commentId);
            }
        }
    }

    public int AddReaction(int SongId, int reactionNumber, int UserId)
    {
        if (RetrieveReaction(SongId, UserId) != "NoReaction")
        {
            return -1;
        }
        int code;
        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"
                             INSERT INTO Reactions
                             (ReactionType, UserId, PostId)
                             VALUES
                             (@type,@UserID, @postID)
                             ";
                command.Parameters.AddWithValue("@UserID", UserId);
                command.Parameters.AddWithValue("@postID", SongId);
                command.Parameters.AddWithValue("@type", reactionNumber);
                connection.Open();
                code = command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return code;
    }

    /// <summary>
    /// edits comment
    /// </summary>
    /// <param name="commId"> Id of the edited comment</param>
    /// <param name="text">Comment Text</param>
    /// <param name="id">Id of the user that is trying to edit the comment</param>
    /// <returns></returns>
    public int EditComment(int commId, string text, int id)
    {
        int code;
        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"UPDATE Comments
                                SET CommentText = @text
                                WHERE AuthorID = @userid AND Id = @commId";
                command.Parameters.AddWithValue("@text", text);
                command.Parameters.AddWithValue("@userid", id);
                command.Parameters.AddWithValue("@commId", commId);
                connection.Open();
                code = command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return code;
    }

    public int EditPfp(int userid, string filename)
    {
        int code;
        using(SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"UPDATE users
                                  SET pfp = @filename
                                  WHERE id = @userid;";
                command.Parameters.AddWithValue("@filename", filename);
                command.Parameters.AddWithValue("@userid", userid);
                connection.Open();
                code = command.ExecuteNonQuery();
            }
            connection.Close();
        }
        return code;
    }
}