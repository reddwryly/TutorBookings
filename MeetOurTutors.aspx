<%@ Page Title="Meet Our Tutors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetOurTutors.aspx.cs" Inherits="TutorBookings.MeetOurTutors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div>
            <!--Dynamic Content-->
            <!--tutor pic, name, bio-->

            <!--somthing is wrong dependednt on label for some reaosn (extra thing small syntax thing)-->
            <asp:Label ID="debug" runat="server" Text=""/>
            <asp:Repeater ID="Tutors" runat="server">
                <ItemTemplate>
                    <img src="\IMG\<%# Eval("Picture") %>"/>
                    <h3><%# Eval("FirstName") %> <%# Eval("LastName") %></h3>
                    <p><%# Eval("Bio") %></p>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>
</asp:Content>
