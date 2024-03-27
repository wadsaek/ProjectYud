using System;
using System.Data;
using System.Data.SqlClient;


public partial class Connection
{
    /// <summary>
    ///working method, works as needed 
    /// </summary>
    /// <param name="id"></param>
    /// <returns>the Song with the specified id</returns>
    public SongData RetrieveSongData(int id)
    {
        string code = "";
        DateTime datee = new DateTime();
        using (SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = @"
                             SELECT TOP 1 Date,Code FROM SONGS
                             WHERE
                             Id=@SongID;
                             ";
            command.Parameters.AddWithValue("@SongID", id.ToString());
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    code = reader["Code"].ToString();
                    datee = DateTime.Parse(reader["Date"].ToString());
                }
            }
            reader.Close();
            connection.Close();
            return new SongData(id, code, datee);
        }
    }

    /// <summary>
    /// working method, works as needed
    /// </summary>
    /// <returns>the last song in the DataBase</returns>
    public SongData RetrieveLastSong()
    {
        int id = 0;
        string code = "";
        DateTime datee = new DateTime();
        using (SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.Parameters.Clear();
            command.CommandText = @"
                             SELECT TOP 1 * FROM SONGS
                             ORDER BY
                             Date desc;
                             ";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    id = int.Parse(reader["id"].ToString());
                    code = reader["Code"].ToString();
                    datee = DateTime.Parse(reader["Date"].ToString());
                }
            }
            reader.Close();
            connection.Close();
            return new SongData(id, code, datee);
        }
    }

    /// <summary>
    /// columns: "id", "code", "date"
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    public DataTable RetrieveSongs(int page)
    {
        page--;
        using (SqlConnection connection = new SqlConnection(path))
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = string.Format(@"SELECT * FROM Songs
                                              ORDER BY Date DESC 
                                              OFFSET @offs ROWS FETCH NEXT {0} ROWS ONLY;
                                              ", Consts.SONGS_ON_A_PAGE);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@offs", page * Consts.SONGS_ON_A_PAGE);
            DataTable table = new DataTable();
            DataColumn column;
            column = new DataColumn("id", Type.GetType("System.Int32"));
            table.Columns.Add(column);
            column = new DataColumn("code", Type.GetType("System.String"));
            table.Columns.Add(column);
            column = new DataColumn("date", Type.GetType("System.DateTime"));
            table.Columns.Add(column);
            using (connection)
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DataRow row = table.NewRow();
                        row["id"] = reader["Id"];
                        row["code"] = reader["Code"];
                        row["date"] = reader["Date"];
                        table.Rows.Add(row);
                    }
                }
                reader.Close();

                connection.Close();
            }
            return table;
        }
    }
}