using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    public int[] admins = { };
    public int pagenum;
    protected void Page_Load(object sender, EventArgs e)
    {
        string fileName = Server.MapPath(Consts.adminPath);
        string jsonString = File.ReadAllText(fileName);
        admins = Newtonsoft.Json.JsonConvert.DeserializeObject<int[]>(jsonString);
        if (Session[Consts.SESSION_USER] != null)
        {
            if (admins.Contains(((UserInfo)Session[Consts.SESSION_USER]).Id))
            {
                songAdder.Visible=true;
            }
        }

        string page= Request.QueryString["page"]==null? "1" : Request.QueryString["page"];
        if (int.TryParse(page, out int pageNum))
        {
            pagenum = pageNum;
            Connection PageCon = new Connection(Server.MapPath(Consts.DB_PATH_LOCAL));
            DataTable dt =PageCon.RetrieveSongs(pageNum);
            SongData[] songs = new SongData[dt.Rows.Count];
            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow curRow = dt.Rows[i];
                SongData current = new SongData((int)curRow["id"], curRow["code"].ToString(), DateTime.Parse(curRow["date"].ToString()));
                songs[i] = current;
            }
            songsRepeater.DataSource=songs;
            songsRepeater.DataBind();
        }
        
    }
}