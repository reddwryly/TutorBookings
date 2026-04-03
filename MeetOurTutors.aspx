<%@ Page Title="Meet Our Tutors" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="MeetOurTutors.aspx.cs" Inherits="TutorBookings.MeetOurTutors" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="Main">
            <!--Dynamic Content-->
            <asp:Repeater ID="Tutors" runat="server" OnItemDataBound="getTutorCourse">
                <ItemTemplate>
                    <div class="EmployeeDisplay">
                        <img src="/IMG/<%# Eval("Picture") %>" class="Headshot" />
                        <div class="Bio">
                            <h3><%# Eval("FirstName") %> <%# Eval("LastName") %></h3>
                            <p><%# Eval("Bio") %></p>
                            <ul>
                                <asp:Repeater ID="Courses" runat="server">
                                    <ItemTemplate>
                                        <li><%# Eval("CourseCode") %> - <%# Eval("Name") %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </ul>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </main>
</asp:Content>
