using System.Collections.Generic;
using System.Data.SqlClient;
public partial class Connection
{
    private static readonly Dictionary<int, string> _reacts = new Dictionary<int, string>
     {
        {1,"poop" },
        {2, "clown" },
        {3, "heart" },
        {4, "bomb" },
        {5, "fire" },
        {6, "whale" },
        {7, "skull" }
     };
    public Dictionary<string,int> RetrieveReactions(int SongId)
    {
        Dictionary<string, int> reactions = new Dictionary<string, int>()
        {
            {"poop",0},
            {"clown",0},
            {"heart",0},
            {"bomb",0},
            {"fire",0},
            {"whale",0},
            {"skull",0}
        };
        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"
                             SELECT COUNT(UserId) as ReactionCount, ReactionType
                             FROM Reactions
                             WHERE PostId=@SongId
                             GROUP BY ReactionType;
                             ";
                command.Parameters.AddWithValue("@SongID", SongId.ToString());
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int type = int.Parse(reader["ReactionType"].ToString());
                            int count = (int)reader["ReactionCount"];
                            reactions[_reacts[type]] = count;
                        }
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return reactions;
    }
    public string RetrieveReaction(int SongId, int UserId)
    {
        string result = "NoReaction";
        using (SqlConnection connection = new SqlConnection(path))
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Parameters.Clear();
                command.CommandText = @"SELECT ReactionType as rt
                                        FROM Reactions
                                        WHERE UserId = @UserID AND
                                        PostId = @PostId;";
                command.Parameters.AddWithValue("@PostID", SongId.ToString());
                command.Parameters.AddWithValue("@UserID", UserId.ToString());
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        int rt = int.Parse(reader["rt"].ToString());
                        result = _reacts[rt];
                    }
                    reader.Close();
                }
            }
            connection.Close();
        }
        return result;
        
    }
}