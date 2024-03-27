using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;

public partial class masters_SongPage : System.Web.UI.MasterPage
{
    List<CommentUserPair> comments;

    public Dictionary<string, int> reactions = new Dictionary<string, int>()
    {
        {"poop",0},
        {"clown",0},
        {"heart",0},
        {"bomb",0},
        {"fire",0},
        {"whale",0},
        {"skull",0}
    };
    public SongData Song;
    protected void Page_Load(object sender, EventArgs e)
    {
        Connection PageCon = new Connection(Server.MapPath(@"../App_Data/database.mdf"));
        if (Request.RawUrl.StartsWith("/aspx/homePage.aspx"))
        {
            Song = PageCon.RetrieveLastSong();
        }
        else if (Request.RawUrl.StartsWith("/aspx/Song.aspx"))
        {
            if (Request.QueryString["songId"] == null)
            {
                Response.Redirect("../aspx/otherSongs.aspx");
            }
            if (int.TryParse(Request.QueryString["songId"], out int result))
            {
                Song = PageCon.RetrieveSongData(result);
                if(Song.SpotifyCode=="")
                {
                    Response.Redirect("../aspx/otherSongs.aspx");
                }
            }
        }
        else { Response.Redirect("../aspx/otherSongs.aspx"); }
        bool loggedIn = Session["user"] != null;
        string jsonString = File.ReadAllText(Server.MapPath("../App_Data/admins.json"));
        int[] admins = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(jsonString);
        if(Song== null)
        {
            Response.Redirect("../aspx/otherSongs.aspx");
        }
        DataTable dt = PageCon.RetrieveComments(Song.SongId, true);
        comments = CommentClass.GetComments(dt, loggedIn ? ((UserInfo)Session["user"]).Id : -1, admins);
        CommentsRepeater.DataSource = comments;
        CommentsRepeater.DataBind();
        reactions = PageCon.RetrieveReactions(Song.SongId);
        newCommentDiv.Visible = loggedIn;
    }
}
