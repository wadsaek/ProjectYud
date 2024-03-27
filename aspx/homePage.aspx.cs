using System;

public partial class _Default : System.Web.UI.Page
{
    public string greeting;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Consts.SESSION_USER] != null)
        {
            greeting = $"<h1 class=greeting> hii {((UserInfo)Session[Consts.SESSION_USER]).UserName}!!!</h1>";
        }
    }
}