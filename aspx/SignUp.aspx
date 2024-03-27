<%@ Page Title="Registration" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="SignUp.aspx.cs" Inherits="aspx_SignUp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="../styling_scripting/css/signUpStyles.css" rel="stylesheet" />
    <script src="../styling_scripting/js/validation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="SignUpDiv">
        <form id="registrationForm" name="registrationForm" runat="server" onsubmit="return validateRegistration()" method="post">
            <div class="SignUpWindow">
                <ul>
                    <li>
                        <b>username*:</b>
                        <input type="text" name="usernameInput" id="usernameInput" maxlength="25" placeholder="Your username" required="required"/>
                    </li>
                    <li>
                        <b>password*:</b>
                         <input type="password" name="passwordInput" id="passwordInput" required="required" placeholder="Password"/>
                    </li>
                    <li>
                        <b>confirm password*:</b>
                        <input type="password" name="passwordInputConfirm" id="passwordInputConfirm" required="required" placeholder="Confirm Password"/>
                    </li>
                    <li>
                        <b>email*:</b>
                        <input type="email" name="emailInput" id="emailInput" required="required" placeholder="your email"/>
                    </li>
                    <li>
                        <input type="submit" value="Register!!" name="SubmitInput" id="SubmitInput" />
                    </li>
                </ul>
                <%=errorDisplay %>
            </div>
        </form>
    </div>
</asp:Content>

