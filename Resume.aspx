<%@ Page Title="Resume" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Resume.aspx.cs" Inherits="TutorBookings.Resume" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="Main">
             <iframe src="Resumefile.pdf" width="100%" height="1000px"></iframe>
        </div>
    </main>
</asp:Content>