using System;

public partial class aspx_error : System.Web.UI.Page
{
    public string errormessage;
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (Request.QueryString["errorType"])
        {
            case "bruh":
                errormessage = "you aren't an admin to visit this page";
                break;
            case "no":
                errormessage = "what are you even trying to do";
                break;
            case "invalidQueryString":
                errormessage = "wow something got screwed up my buddy";
                break;
            case "UserDoesntExist":
                errormessage = "there is no user with such id";
                break;
            default:
                errormessage = "why are you here?";
                break;
        }
    }
}