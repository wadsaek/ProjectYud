using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public partial class aspx_processer : System.Web.UI.Page
{
    private void DeleteSong(Connection con)
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            writeNotAdminity();
            return;
        }

        string jsonString = File.ReadAllText(Server.MapPath(Consts.adminPath));
        List<int> admins = JsonConvert.DeserializeObject<List<int>>(jsonString);

        if (!admins.Contains(((UserInfo)Session[Consts.SESSION_USER]).Id))
        {
            writeNotAdminity();
            return;
        }
        if (int.TryParse(Request.QueryString["song"], out int songId))
        {
            if (con.DeleteSong(songId) > 0)
            {
                Response.Write("success");
                Response.End();
            }
            else
            {
                Response.Write("something went wrong sorry");
                Response.End();
            }
        }
        else{
            Response.Write("invalid song id");
            Response.End();
        }
    }

    private void writeResponse(string a)
    {
        string[] response = { a };
        Response.Write(JsonConvert.SerializeObject(response));
        Response.End();
    }

    private void writeNotAdminity()
    {
        Response.Write("notAnAdmin");
        Response.End();
    }
    private void MakeAnAdmin()
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            writeNotAdminity();
            return;
        }

        string jsonString = File.ReadAllText(Server.MapPath(Consts.adminPath));
        List<int> admins = JsonConvert.DeserializeObject<List<int>>(jsonString);

        if (!admins.Contains(((UserInfo)Session[Consts.SESSION_USER]).Id))
        {
            writeNotAdminity();
            return;
        }
        if (int.TryParse(Request.QueryString["victim"], out int victimId))
        {
            if (admins.Contains(victimId))
            {
                Response.Write("the user is already an admin");
                Response.End();
                return;
            }
            admins.Add(victimId);
            jsonString = JsonConvert.SerializeObject(admins);
            File.WriteAllText(Server.MapPath(Consts.adminPath), jsonString);
            Response.Write("success");
            Response.End();
        }
        else
        {
            Response.Write("invalid user id");
            Response.End();
        }
    }
    void ReactionFunc(Connection con)
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Write("not logged in");
            Response.End();
            return;
        }
        if(int.TryParse(Request.QueryString["song"], out int songId))
        {
            if(int.TryParse(Request.QueryString["reactType"], out int reaction) && reaction>0 && reaction<8)
            {
                int returnCode = con.AddReaction(songId, reaction, ((UserInfo)Session[Consts.SESSION_USER]).Id);
                switch (returnCode)
                {
                    case 1:
                        Response.Write("success");
                        break;
                    case -1:
                        Response.Write("the user has already put a reaction");
                        break;
                    default:
                        Response.Write("something strange happened");
                        break;
                }
            }
            else
            {
                Response.Write("f u");
            }

        }
        else
        {
            Response.Write("f u");
        }
        
        
        Response.End();
    }
    void GetreactionFunc(Connection con)
    {
        const string noreaction = "NoReaction";
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Write(noreaction);
            Response.End();
            return;
        }
        if (int.TryParse(Request.QueryString["song"], out int songId))
        {
            string reaction = con.RetrieveReaction(songId, ((UserInfo)Session[Consts.SESSION_USER]).Id);
            switch (reaction)
            {
                case noreaction:
                    Response.Write(noreaction);
                    break;
                case "poop":
                    Response.Write("1");
                    break;
                case "clown":
                    Response.Write("2");
                    break;
                case "heart":
                    Response.Write("3");
                    break;
                case "bomb": 
                    Response.Write("4");
                    break;
                case "fire":
                    Response.Write("5");
                    break;
                case "whale":
                    Response.Write("6");
                    break;
                case "skull":
                    Response.Write("7");
                    break;                
            }
            Response.End();
        }
        else
        {
            Response.Write("f u");
        }
    }
    void DeleteCommentFunc(Connection con)
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Write("NoComment");
            Response.End();
            return;
        }
        if (int.TryParse(Request.QueryString["comment"], out int commentId))
        {
            switch (con.DeleteComment(commentId, ((UserInfo)Session[Consts.SESSION_USER]).Id, Server.MapPath(Consts.adminPath)))
            {
                case 1:
                    Response.Write("success");
                    break;
                case 0:
                    Response.Write("NoComment");
                    break;
                default:
                    Response.Write("Oh no");
                    break;
            }
            Response.End();

        }
        else
        {
            Response.Write("go to hell");
            Response.Write(Request.QueryString["comment"]);
            Response.End();
        }
    }
    void DeleteReactFunc(Connection con)
    {
        if (Session[Consts.SESSION_USER] == null)
        {
            Response.Write("not logged in");
            Response.End();
        }
        if (int.TryParse(Request.QueryString["song"], out int songId))
        {
            if (con.DeleteReaction(songId, ((UserInfo)Session[Consts.SESSION_USER]).Id) == 1)
                Response.Write("success");
            else Response.Write("oh no");
            Response.End();
        }

    }
    private void AddComment(Connection con)
    {
        
        if (Session[Consts.SESSION_USER] == null)
        {

            writeResponse("not logged in");
            return;
        }
        int userId= ((UserInfo)Session[Consts.SESSION_USER]).Id;
        string commentText = Request.QueryString["CommText"].Trim();
        if(commentText.Length > 250)
        {
            writeResponse("This is way too big. Try limiting yourself to 250 characters");
            return;
        }
        if(commentText.Length == 0)
        {
            writeResponse("try writing something");
            return;
        }
        if (int.TryParse(Request.QueryString["CommId"], out int CommId))
        {
            if(CommId == -1)
            {
                if (int.TryParse(Request.QueryString["songId"], out int postId))
                {
                    (int, int) ConResponse = con.AddComment(postId, commentText, userId, DateTime.Now);
                    if (ConResponse.Item1 != 1)
                    {
                        writeResponse("an error occured while adding the comment");
                        return;
                    }
                    string[] response = {
                        "added",
                        ((UserInfo)Session[Consts.SESSION_USER]).UserName,
                        ConResponse.Item2.ToString(),
                        ((UserInfo)Session[Consts.SESSION_USER]).Id.ToString() };
                    Response.Write(JsonConvert.SerializeObject(response));
                    Response.End();
                    return;
                }
                else
                {
                    writeResponse("non numeric song id");
                    return;
                }
            }
            if(CommId >= 0)
            {
                if(con.EditComment(CommId, commentText, userId) == 1)
                {
                    writeResponse("edited");
                    return;
                }
                writeResponse("an error occured while editing the comment");
                return;

            }
            else
            {
                writeResponse("what are you trying to do");
                return;
            }
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if(Request.QueryString==null || Request.QueryString.Count<=0) return;
        if (Request.QueryString["action"] == null)
        {
            Response.Write("no action specified");
            Response.End();
            return;
        }
        Connection con = new Connection(Server.MapPath(@"../App_Data/database.mdf"));
        switch (Request.QueryString["action"])
        {
            case "deletecomment":
                DeleteCommentFunc(con);
                break;
            case "putreaction":
                ReactionFunc(con);
                break;
            case "getreaction":
                GetreactionFunc(con);
                break;
            case "deleteReaction":
                DeleteReactFunc(con);
                break;
            case "addComment":
                AddComment(con);
                break;
            case "makeAnAdmin":
                MakeAnAdmin();
                break;
            case "deleteSong":
                DeleteSong(con);
                break;
            default:
                Response.Write("Command not found");
                Response.End();
                break;

        }
    }   
}