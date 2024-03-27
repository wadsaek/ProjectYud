<%@ Page Title="Other Songs" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="otherSongs.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="../styling_scripting/css/otherSongs.css"/>
    <script src="../styling_scripting/js/deleteSong.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="display:flex;flex-direction:column;align-items:center; padding-top:5px;">
        <div class="song" id="songAdder" runat="server" visible="false">
            <h2 style="margin:0;">
                <a href="addSong.aspx">New Song!</a>
            </h2>
        </div>
        <asp:Repeater runat="server" ID="songsRepeater">
            <ItemTemplate>
                <div class="song overflow-div" id="SongBox<%#Eval("SongId") %>">
                    <b>
                        <a href="../aspx/Song.aspx?songId=<%#Eval("SongId") %>">
                            <%#Eval("SongDate").ToString().Substring(0,11).Replace(".","/")%>
                        </a>
                    </b>
                    <div style="display:flex;flex-direction:row;align-items:center;">
                        <iframe  style="border-radius: 12px;" src="https://open.spotify.com/embed/track/<%#Eval("SpotifyCode")%>?utm_source=generator"
                        frameborder="0" allow="autoplay; clipboard-write; encrypted-media; picture-in-picture" loading="lazy"></iframe>
                        <%if ((Session[Consts.SESSION_USER] != null) && (admins.Contains(((UserInfo)Session[Consts.SESSION_USER]).Id)))
                            { %>
                        <img id="delSong" src="../images/uiElements/bin.png" title="delete comment" onclick="deleteSong(<%#Eval("SongId")%>)" height="24" />
                        <%} %>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
         <ul class="pageChange">
             <li runat="server" id="prevSong" visible="true">
                 <a href="otherSongs.aspx?page=<%=pagenum-1%>"><div>
                     <%=pagenum-1%>
                   </div></a>
             </li>
             <li style="background-color:#0074B5"><b><%=pagenum%></b></li>
             <li>
                 <a href="otherSongs.aspx?page=<%=pagenum+1%>"><div>
                   <%=pagenum+1%>
                 </div></a>
             </li>
         </ul>
    </div>
</asp:Content>

