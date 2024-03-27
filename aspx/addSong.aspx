<%@ Page Title="add a new song!" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="addSong.aspx.cs" Inherits="aspx_addSong" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script src="../styling_scripting/js/validation.js"></script>
    <link href="../styling_scripting/css/signUpStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <form name="SongInputForm" id="SongInputForm" runat="server" method="post" onsubmit="return validateSong()">
        <div class="SignUpWindow" style="padding-bottom:4px;">
            <h4 style="margin:0;padding-bottom:4px;padding-top:5px;">Enter the link to the song you would like to add!</h4>
            <input type="text" name="songInput" id="songInput" autocomplete="off" size="45" style="padding-bottom:4px"/>
            <input type="submit"/>
        </div>
    </form>
</asp:Content>

