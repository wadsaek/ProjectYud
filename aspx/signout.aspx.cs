using System;

public partial class aspx_signout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string link = Session[Consts.SESSION_LAST_PAGE]==null? "/aspx/homePage.aspx":(string)Session[Consts.SESSION_LAST_PAGE];
        Session.Abandon();
        Response.Redirect(link);
    }
}