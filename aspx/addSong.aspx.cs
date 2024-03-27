using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;

public partial class aspx_addSong : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Server.MapPath("../App_Data/admins.json");
        string jsonString = File.ReadAllText(fileName);
        int[] admins = JsonConvert.DeserializeObject<int[]>(jsonString);
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Redirect("../aspx/error.aspx?errorType=bruh");
        }
        if (!admins.Contains(((UserInfo)Session[Consts.SESSION_USER]).Id))
        {
            Response.Redirect("../aspx/error.aspx?errorType=bruh");
        }
        if (Request.Form["songInput"] == null)
        {
            return;
        }
        string link = Request.Form["songInput"];

        if (!(link.Contains("/track/") && link.Contains("?si")))
        {
            return;
        }
        if (link.Substring(link.IndexOf("/track") + 7).Length <= 25)
        {
            return;
        }
        Connection PageCon = new Connection(Server.MapPath("../App_Data/database.mdf"));
        PageCon.InputSongData(link.Substring(link.IndexOf("/track") + 7, 22), DateTime.Today);
        Response.Redirect((string)Session[Consts.SESSION_LAST_PAGE]);
    }
}