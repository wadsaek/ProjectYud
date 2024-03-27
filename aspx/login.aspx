<%@ Page Title="" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="login.aspx.cs" Inherits="aspx_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../styling_scripting/css/signUpStyles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="logInDiv">
        <form id="logInForm" name="logInForm" runat="server" method="post">
            <div class="SignUpWindow">
                <ul>
                    <li>
                        <b>username*:</b>
                        <input type="text" name="usernameInput" id="usernameInput" maxlength="25" placeholder="Your username" required="required" autocomplete="on" />
                    </li>
                    <li>
                        <b>password*:</b>
                        <input type="password" name="passwordInput" id="passwordInput" required="required" placeholder="Password" />
                    </li>
                    <li>
                        <input type="submit" value="Log in!!" name="SubmitInput" id="SubmitInput" />
                    </li>
                    <li>
                        <%=errorDisplay %>
                    </li>
                </ul>
            </div>
        </form>
        <div style="margin-top: 20px;">
            <b style="background-color: #0074b53d; border-radius: 4px;
    margin-top: 10px;"><a href="../aspx/SignUp.aspx" style="background-color: #4fafd8; border-radius: 4px; color: #050325; font-size: large; padding: 3px;">No account? register now!</a></b>
        </div>
    </div>
</asp:Content>

