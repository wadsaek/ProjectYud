using System;

public partial class aspx_SignUp : System.Web.UI.Page
{
    public string errorDisplay="";
    protected void Page_Load(object sender, EventArgs e)
    {
        Connection PageCon = new Connection(Server.MapPath(Consts.DB_PATH_LOCAL));
        //Request.Form.Clear();
        if (Request.Form.Count != 0)
        {
            switch (PageCon.AddUser(Request.Form["usernameInput"], Request.Form["passwordInput"], Request.Form["emailInput"])){
                case 1:
                    int id= PageCon.RetriveUserIdByUserName(Request.Form["usernameInput"]);
                    Session[Consts.SESSION_USER] = new UserInfo(id, Request.Form["usernameInput"], Request.Form["emailInput"], Consts.DEFAULT_PNG);
                    Response.Redirect(Session[Consts.SESSION_LAST_PAGE]!=null? (string)Session[Consts.SESSION_LAST_PAGE]: "/aspx/homePage.aspx");
                    break;
                case -2:
                    errorDisplay += "there is a user with this mail already.";
                    break;
                case -3:
                    errorDisplay += "there is a user with this username already";
                    break;
                case -5:
                    errorDisplay += "there is a user with this username and this mail already";
                    break;
                default:
                    errorDisplay += "wtf";
                    break;

            }
        }
        
    }
}