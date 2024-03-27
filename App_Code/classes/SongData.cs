using System;

/// <summary>
/// Summary description for SongData
/// </summary>
public class SongData
{
    public static string SpotifyIdCutOut(string link)
    {
        int pFrom=0, pTo=0;
        if (link != null) { 
        pFrom = link.IndexOf("/track/") + "/track/".Length;
        }

        return pTo!=0 ?link.Substring(pFrom, 7):"Bad Link";
    }
    public bool Validity { get; }
    public int SongId { get;}
    public string SpotifyCode { get;}
    public DateTime SongDate { get;}
    public SongData(int songId, string spotifyId, DateTime songDate)
    {
        SongId = songId;
        SpotifyCode = spotifyId;
        SongDate = songDate;
        Validity = spotifyId.Length==22;
    }
}