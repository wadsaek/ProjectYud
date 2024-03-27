<%@ Page Title="Account" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="Account.aspx.cs" Inherits="aspx_Account"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styling_scripting/css/comments.css" rel="stylesheet" />
    <script src="../styling_scripting/js/comments.js"></script>
    <script src="../styling_scripting/js/makeAdmin.js"></script>
    <link href="../styling_scripting/css/account.css" rel="stylesheet" />
    <link href="../styling_scripting/css/homepage.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="HomePageDiv" style="margin-top:5%">
        <div class="notGreeting">
            <div class="songContent"  style="margin-right:60px">
                <h1 class="SongDate" id="SongDate"><%=PageUser.UserName%></h1>
                <img src="<%=$"{Consts.IMAGES_DIR_LOCAL}/{PageUser.PfpAdress}"%>" class="pfp-image" height="120" width="120"/>
                <div style="padding-top:30px;width:100%">
                    <a href="settings.aspx" class="theprettyblue" id="settingsanc" runat="server" visible="false">Settings</a>
                        <div runat="server" id="deleteUser" visible="false">
                            <form runat="server" name="deleteform">
                                <button name="deleteUser" class="theprettyblue" >Delete this user</button>
                                <b><%=error %></b>
                            </form>
                        </div>
                    <button runat="server" class="theprettyblue" id="makeAdminbut" visible="false">Make an admin</button>
                </div>
            </div>
            <div class="commentaryOuter" style="width:320px;overflow-x:unset">
                <div class="commentaryBlock" id="commentaryBlock">
                    <asp:Repeater runat="server" ID="CommentsRepeater">
                        <ItemTemplate>
                            <div class="comment" id="comment<%#Eval("Comment.Id") %>">
                                <div class="commentUserData">
                                    <img src="../images/pfps/<%#Eval("pfp")%>" class="pfp-image" height="22" width="22" title="<%#Eval("UserName")%>'s pfp" />
                                    <b style="padding: inherit;"><%#Eval("UserName") %> </b>
                                </div>
                                <div class="textanddelte">
                                    <div class="commentText" id="commenttext<%#Eval("Comment.Id") %>">
                                        <a href="Song.aspx?songId=<%#Eval("Comment.PostId") %>">
                                            <%#Eval("Comment.Text") %>
                                        </a>
                                    </div>
                                    <div class="delete" runat="server" visible='<%#Eval("CorrectStyleDeletion")%>'>
                                        <img src="../images/uiElements/bin.png" title="delete comment" onclick="deletecomment(<%#Eval("Comment.Id") %>)"
                                             height="24" />
                                    </div>
                                </div>
                                <i title="<%#Eval("Comment.CommentDate") %>"><%#Eval("FormatedDateTime") %></i>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

