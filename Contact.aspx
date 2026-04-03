<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TutorBookings.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
            <div class="Main">
                <h2>Tutoring Coordinator</h2>
                <div class="EmployeeDisplay">
                    <img src="/IMG/Default.png" class="Headshot"/>

                    <div class="Bio">
                        <h3>Ms. Tutor</h3>
                        <p>Work Hours: M-F 9:00AM-4:30PM</p>
                        <p>MTutor@email.com | 123-456-78910 | Office:F456</p>
                    </div>
                </div>
                <h2>Head Tutor</h2>
                <div class="EmployeeDisplay">
                    <img src="/IMG/Default.png" class="Headshot"/>

                    <div class="Bio">
                        <h3>Jane Doe</h3>
                        <p>Work Hours: Tuesday 3:00PM-6:00PM | Wednesday 9:00AM-12:00PM | Thursday 3:00PM-6:00PM</p>
                        <p>JDoe02@email.com | Office:Tutoring Center L845</p>
                    </div>
                </div>
            </div>
    </main>
</asp:Content>
