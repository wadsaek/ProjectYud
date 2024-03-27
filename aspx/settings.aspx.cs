using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

public partial class _Default : System.Web.UI.Page
{

    public string errormsg;
    public string errorscript;
    Connection con;
    int userid;
    string prevpfp;

    private string getrandompfpname(string filepath)
    {
        string filename = Security.RandomString(8) + ".jpeg";
        if (File.Exists(filepath + "/" + filename))
        {
            //try again if such a pfp already exists
            return getrandompfpname(filepath);
        }
        else
        {
            //hooray
            return filename;
        }
    }

    protected void SetPfp(object sender, EventArgs e)
    {
        if (pfpinput.Value == "")
        {
            errormsg = "there is no file attached";
            return;
        }

        HttpPostedFile pfp = pfpinput.PostedFile;
        if (pfp.ContentLength > 4_194_304)//4mb
        {
            errormsg = "wow this is way too much. try limiting yourself to 4MiB";
            return;
        }

        string dir = Server.MapPath(Consts.IMAGES_DIR_LOCAL);
        string filename = getrandompfpname(dir);
        pfp.SaveAs(dir + "/" + filename);
        int returncode = con.EditPfp(userid, filename);
        switch (returncode)
        {
            case 1:
                errormsg = "everything went as supposed to";
                UserInfo tempuser = (UserInfo)Session[Consts.SESSION_USER];
                tempuser.PfpAdress = filename;
                Session[Consts.SESSION_USER] = tempuser;
                //i don't want to delete the defaul pfp
                if (prevpfp != Consts.DEFAULT_PNG)
                {
                    File.Delete(dir + "/" + prevpfp);
                }
                Response.Redirect(Request.RawUrl);
                break;
            case 0:
                errormsg = "the pfp didn't change in the db??";
                break;
            default:
                errormsg = "wow something got fucked up";
                break;

        }
    }

    private void GetUserId()
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Redirect("../aspx/login.aspx");
        }
        userid = ((UserInfo)Session[Consts.SESSION_USER]).Id;
    }
    private void GetUserPfp()
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Redirect("../aspx/login.aspx");
        }
        prevpfp = ((UserInfo)Session[Consts.SESSION_USER]).PfpAdress;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        GetUserId();
        GetUserPfp();
        con = new Connection(Server.MapPath(Consts.DB_PATH_LOCAL));
    }

    protected void DeleteUser(object sender, EventArgs e)
    {
        string path = Server.MapPath(Consts.adminPath);
        List<int> admins = Newtonsoft.Json.JsonConvert.DeserializeObject<List<int>>(File.ReadAllText(path));
        bool isAdmin = admins.Contains(userid);
        if(isAdmin && admins.Count == 1)
        {
            errorscript = @"<script>alert(""you're the only admin you can't do this"");</script>";
            return;
        }
        string dir = Server.MapPath(Consts.IMAGES_DIR_LOCAL);
        if (prevpfp != Consts.DEFAULT_PNG)
        {
            File.Delete(dir + "/" + prevpfp);
        }
        switch (con.DeleteUser(userid))
        {
            case 1:
                if (isAdmin)
                {
                    admins.Remove(userid);
                    File.WriteAllText(path, Newtonsoft.Json.JsonConvert.SerializeObject(admins));
                }
                errorscript = @"<script>alert('success');</script>";
                Session.Abandon();
                Response.Redirect("/aspx/homePage.aspx");
                break;
            case 0:
                errorscript = @"<script>alert('there was an error');</script>";
                break;
            default:
                errorscript = @"<script>alert('wow something went really wrong')</script>";
                break;
        }
    }
}