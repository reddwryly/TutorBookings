<%@ Page Title="Book Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="BookTutoring.aspx.cs" Inherits="TutorBookings.BookTutoring" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="Main">
            <!--Dynamic Content-->
            <fieldset>
                <label for="tutors">Tutor:</label> <!--change labels to asp labels or html-->

                <asp:DropDownList runat="server" ID="TutorDDL" OnSelectedIndexChanged="TutorSelected" AutoPostBack="true">
                    <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                </asp:DropDownList>

                <label for="course">Course:</label> 
                 <asp:DropDownList runat="server" ID="CourseDDL" OnSelectedIndexChanged="CourseSelected" AutoPostBack="True" >
                     <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                 </asp:DropDownList>

                <br />

                <label for="date">Date:</label>
                <asp:Calendar runat="server" ID="Date" OnDayRender="LoadDates" OnSelectionChanged="DateSelected"></asp:Calendar>

                <label for="time">Time:</label>
                 <asp:DropDownList runat="server" ID="TimeDDL" AutoPostBack="true" Enabled="false">
                     <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                     <asp:ListItem Text="placeholder1" Value="1"></asp:ListItem>
                     <asp:ListItem Text="placeholder2" Value="2"></asp:ListItem>
                 </asp:DropDownList>
            </fieldset>

            <fieldset>
                <label for="Fname">First:</label>
                
                <asp:TextBox runat="server" ID="FName" MaxLength="50" TextMode="SingleLine"></asp:TextBox>
                <label for="Lname">Last:</label>
                <asp:TextBox runat="server" ID="LName" MaxLength="50" TextMode="SingleLine"></asp:TextBox>

                <label for="email">Email:</label>
                <asp:Textbox runat="server" ID="Email" MaxLength="50" TextMode="SingleLine"></asp:Textbox>

                <asp:Button runat="server" ID="Submit" Text="Book" OnClick="SubmitButton" />
            </fieldset>
            <br />
            <br />
        </div>
    </main>

</asp:Content>
