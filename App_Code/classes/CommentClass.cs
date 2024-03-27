using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

/// <summary>
/// Summary description for CommentClass
/// </summary>

public struct CommentUserPair
{
    public string UserName { get; }
    public int userid { get; }
    public string pfp { get; }
    public CommentClass Comment { get; }
    public string FormatedDateTime
    {
        get
        {
            DateTime commDate = Comment.CommentDate;
            return commDate.Date == DateTime.Now.Date
            ? string.Format("{0:D2}:{1:D2}", commDate.Hour, commDate.Minute)
            : string.Format("{0:D2}/{1:D2}/{2:D4}", commDate.Day, commDate.Month, commDate.Year);
        }
    }
    public  bool CorrectStyleDeletion { get;}
    public bool CorrectStyleEditing { get;}

    public CommentUserPair(DataRow i, int[] admins, int curuserid)
    {
        UserName = (string)i["username"];
        userid = (int)i["UserId"];
        pfp = (string)i["pfp"];
        Comment = new CommentClass((int)i["Id"], (string)i["Text"], (DateTime)i["Time"], (int)i["PostID"]);
        CorrectStyleDeletion = (userid == curuserid) || admins.Contains(curuserid);
        CorrectStyleEditing = userid == curuserid;
    }
}
public class CommentClass
{
    public int PostId { get; set; }
    public int Id { get; } 
    public string Text { get; set; }
    public DateTime CommentDate { get; }
    public CommentClass(int CommentId, string CommentText, DateTime Time,int postId)
    {
        Id = CommentId;
        Text = CommentText;
        CommentDate = Time;
        PostId = postId;
    }
    public static List<CommentUserPair> GetComments(DataTable dt, int userid, int[] admins)
    {
        List<CommentUserPair> comments = new List<CommentUserPair>();
        foreach (DataRow i in dt.Rows)
        {
            CommentUserPair pair = new CommentUserPair(i,admins,userid);
            
            comments.Add(pair);

        }
        return comments;
    }
}