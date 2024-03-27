<%@ Page Title="" Language="C#" MasterPageFile="~/masters/MasterPage.master" AutoEventWireup="true" CodeFile="error.aspx.cs" Inherits="aspx_error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div style="margin:11%;display:flex;flex-direction:column;align-items:center;color:#D9D9D9;">
        <h1>
            <%=errormessage %>
        </h1>
        <h2>
            <a href="homePage.aspx" style="text-decoration:underline; color :#D9D9D9">Return to homepage?</a>
        </h2>
    </div>
</asp:Content>

