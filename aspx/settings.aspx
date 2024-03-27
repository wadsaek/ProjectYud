<%@ Page Title="Settings" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="settings.aspx.cs" Inherits="_Default" runat="server" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styling_scripting/css/settings.css" rel="stylesheet" />
    <script src="../styling_scripting/js/settings.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="outer">
        <form id="pfpform" name="pfpform" runat="server" enctype="multipart/form-data">
            <div class="settings">
                <p>Set image!</p>
                <hr />
                <input type="file" name="pfpinput" id="pfpinput" runat="server" placeholder="Choose File" class ="fileupload"/>
                <hr />
                <button runat="server" onserverclick="SetPfp">
                    Submit!
                </button>
                
                <b id="errormsg">
                    <%=errormsg%>
                </b>
            </div>
            <div>
                <button onclick="showDeletion()" type="button" class="deletebutton" runat="server" validationgroup="userDeletion"> delete yourself</button>
                <div id="confirmation" style="display:none;" class="deletebutton">
                    are you sure?<br/>
                    <asp:Button ID="deleteUserButton" runat="server" Text="YES" OnClick="DeleteUser" ValidationGroup="userDeletion" />
                    <%=errorscript %>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

