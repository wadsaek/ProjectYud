using System;

public partial class MasterPage : System.Web.UI.MasterPage
{
    public string logup;
    public string pfp = "default.png";
    protected void Page_Load(object sender, EventArgs e)
    {
        string url = Request.RawUrl;
        if (!(url.StartsWith("/aspx/SignUp.aspx")
            ||url.StartsWith("/aspx/login.aspx")
            ||url.StartsWith("/aspx/addSong.aspx")
            ||url.StartsWith("/aspx/settings.aspx")))
        {
            Session["LastPage"] = url;
        }
        if (Session["user"] == null)
        {
            logup = @"<ul id=""LogUp"" style=""list-style:none; display:none;background-color:rgb(55 131 191);border-radius:10px;margin-right:8px"">
                                    <li>
                                        <a href=""../aspx/login.aspx"" style=""color:#050325;font-size:25px"">LOGIN</a>
                                    </li>
                                    <li>
                                        <a href=""../aspx/SignUp.aspx""style=""color:#050325;font-size:25px"">SIGNUP</a>
                                    </li>
                                </ul>";
            
        }
        else
        {
            logup = @"<ul id=""LogUp"" style=""list-style:none; display:none;background-color:rgb(55 131 191);border-radius:10px;margin-right:8px"">
                                    <li>
                                        <a href=""../aspx/Account.aspx"" style=""color:#050325;font-size:25px"">ACCOUNT</a>
                                    </li>
                                    <li>
                                        <a href=""../aspx/signout.aspx""style=""color:#050325;font-size:25px"">SIGN OUT</a>
                                    </li>
                                </ul>";
            pfp = ((UserInfo)Session["user"]).PfpAdress;

        }
    }


}
