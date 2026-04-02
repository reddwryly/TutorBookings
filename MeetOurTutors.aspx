<%@ Page Title="Meet Our Tutors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MeetOurTutors.aspx.cs" Inherits="TutorBookings.MeetOurTutors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="Main">
            <!--Dynamic Content-->
            <asp:Repeater ID="Tutors" runat="server">
                <ItemTemplate>
                    <div class="MeetTutors">
                        <img src="/IMG/<%# Eval("Picture") %>" class="TutorImg"/>

                        <div class="TutorBio">
                            <h3><%# Eval("FirstName") %> <%# Eval("LastName") %></h3>
                            <p><%# Eval("Bio") %></p>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>
</asp:Content>
