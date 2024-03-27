using System;

public partial class aspx_Default : System.Web.UI.Page
{

    public string errorDisplay = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        Connection PageCon = new Connection(Server.MapPath(Consts.DB_PATH_LOCAL));
        //Request.Form.Clear();
        if (Request.Form.Count != 0)
        {
            if ((Request.Form["usernameInput"]=="")||(Request.Form["passwordInput"] == ""))
            {
                errorDisplay += "you didn't fill all of the fields";
                return;
            }
            UserInfo log = PageCon.Login(Request.Form["usernameInput"], Request.Form["passwordInput"],out int opCode);
            switch (opCode)
            {
                case -2:
                    errorDisplay += @"there isn't a user with this username. try <a href=""../aspx/SignUp.aspx"">registration</a>";
                    break;
                case -3:
                    errorDisplay += "incorrect password";
                    break;
                case 1:
                    Session[Consts.SESSION_USER] = log;
                    Response.Redirect(Session[Consts.SESSION_LAST_PAGE]==null?"/aspx/homePage.aspx":(string)Session[Consts.SESSION_LAST_PAGE]);;
                    break;


            }
        }

    }
}