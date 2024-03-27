using System.Data.SqlClient;


public partial class Connection
{
    /// <summary>
    /// working method, retrieves as needed
    /// </summary>
    /// <param name="passedId"></param>
    /// <returns> the User with the specified id</returns>
    public UserInfo RetriveUserData(int passedId)
    {
        SqlConnection connection = new SqlConnection(path);
        int id = -1;
        string username = "", email = "", pfpAdress = Consts.DEFAULT_PNG;
        SqlCommand command = new SqlCommand(
            $"SELECT TOP 1 pfp,id,username,email FROM users WHERE id=@userid AND username!='{Consts.DELETED_USERNAME}';",
            connection);
        command.Parameters.Clear();
        command.Parameters.AddWithValue("@userid", passedId);
        using (connection)
        {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    pfpAdress = reader["pfp"].ToString();
                    id = (int)reader["Id"];
                    username = (string)reader["username"];
                    email = (string)reader["email"];
                }
                reader.Close();
                connection.Close();
                return new UserInfo(id, username, email, pfpAdress);
            }
            else
            {
                reader.Close();
                connection.Close();

                return UserInfo.Empty;
            }


        }
    }

    public bool ExistsUserWithUserName(string username)
    {
        if (username == Consts.DELETED_USERNAME) return true; 
        bool returnvalue;
        using(SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = @"SELECT COUNT(Id) as count from users WHERE username=@username";
            command.Parameters.AddWithValue("@username", username);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            returnvalue = (int)reader["count"] != 0;
            reader.Close();
            connection.Close();
        }
        return returnvalue;
    }
    public bool ExistsUserWithEmail(string email)
    {
        bool returnvalue;
        using (SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = @"SELECT COUNT(Id) as count from users WHERE email=@email";
            command.Parameters.AddWithValue("@email", email);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            returnvalue = (int)reader["count"] != 0;
            reader.Close();
            connection.Close();
        }
        return returnvalue;
    }
    public int RetriveUserIdByUserName(string passedusername)
    {
        if(passedusername==Consts.DELETED_USERNAME) return -1;
        int id = -1;
        using (SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = @"SELECT TOP 1 id FROM users WHERE username=@userr;";
            command.Parameters.AddWithValue("@userr", passedusername);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = (int)reader["Id"];
                }
                reader.Close();
                connection.Close();
                return id;

            }
            else
            {
                reader.Close();
                connection.Close();

                return 0;
            }


        }
    }
}