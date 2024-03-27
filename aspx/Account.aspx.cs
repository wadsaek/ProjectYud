using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls.WebParts;

public partial class aspx_Account : System.Web.UI.Page
{
    public UserInfo PageUser;
    public string error;
    private int getId()
    {
        if (Request.QueryString["id"] == null)
        {
            if (Session[Consts.SESSION_USER] != null)
            {
                Response.Redirect(Request.RawUrl + $"?id={((UserInfo)Session[Consts.SESSION_USER]).Id}");
                return -1;
            }
            else
            {
                Response.Redirect("../aspx/homePage.aspx");
                return -1;
            }
        }
        else
        {
            if (int.TryParse(Request.QueryString["id"], out int Id))
            {
                return Id;
            }
            else
            {
                Response.Redirect("../aspx/error.aspx?errorType=invalidQueryString");
                return -1;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        int PageId = getId();
        Connection con = new Connection(Server.MapPath(Consts.DB_PATH_LOCAL));
        PageUser = con.RetriveUserData(PageId);
        if(PageUser.Id == -1)
        {
            Response.Redirect("../aspx/error.aspx?errorType=UserDoesntExist");
        }
        string mappedpath = Server.MapPath(Consts.adminPath);
        string jsonString = File.ReadAllText(mappedpath);
        List<int> admins = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(jsonString);
        DataTable dt = con.RetrieveComments(PageUser.Id,false);
        List<CommentUserPair> comments = CommentClass.GetComments(dt, Session[Consts.SESSION_USER] != null 
            ? ((UserInfo)Session[Consts.SESSION_USER]).Id : -1, admins.ToArray());
        CommentsRepeater.DataSource = comments;
        CommentsRepeater.DataBind();

        if (Session[Consts.SESSION_USER] != null)
        {
            int curUser = ((UserInfo)Session[Consts.SESSION_USER]).Id;
            if(PageUser.Id == curUser)
            {
                settingsanc.Visible = true;
            }
            else
            {
                if (admins.Contains(curUser))
                {
                    deleteUser.Visible=true;
                    bool isAdminPage = admins.Contains(PageUser.Id);
                    if (!isAdminPage)
                    {
                        makeAdminbut.Visible = true;
                        makeAdminbut.Attributes.Add("onclick", $"makeAdmin(this,{PageUser.Id})");
                    }
                    if (Request.Form.Count > 0)
                    {

                        switch(con.DeleteUser(PageUser.Id, curUser, admins))
                        {
                            case 1:
                                if (PageUser.PfpAdress != Consts.DEFAULT_PNG)
                                {
                                    File.Delete(Server.MapPath(Consts.IMAGES_DIR_LOCAL) + "/" + PageUser.PfpAdress);
                                }
                                if (isAdminPage)
                                {
                                    admins.Remove(PageUser.Id);
                                    File.WriteAllText(mappedpath, Newtonsoft.Json.JsonConvert.SerializeObject(admins));
                                }
                                Response.Redirect("../aspx/homePage.aspx");
                                break;
                            case -1:
                                error = "you aren't an admin";
                                break;
                            case 0:
                                error = "something went wrong. noone got deleted";
                                break;
                            default:
                                error = "oh wow. oh no.";
                                break;
                        }
                        
                    }
                }
            }
        }
    }
}