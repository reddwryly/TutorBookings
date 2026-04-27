<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="TutorBookings.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
            <div class="Main">
                <fieldset>
                    <h2 style="text-align:center;">Tutoring Coordinator</h2>
                    <div class="EmployeeDisplay">
                        <img src="/IMG/MsTutor.jpg" class="Headshot"/>

                        <div class="Bio">
                            <h3>Tracy Tutor</h3>
                            <p>Work Hours: Monday-Friday 9:00AM-4:30PM</p>
                            <p>TTutor@email.com | 123-456-78910 | Office:O123</p>
                        </div>
                    </div>
                </fieldset>
                <fieldset>
                    <h2 style="text-align:center;">Head Tutor</h2>
                    <div class="EmployeeDisplay">
                        <img src="/IMG/JaneDoe.jpg" class="Headshot"/>

                        <div class="Bio">
                            <h3>Jane Doe</h3>
                            <p>Work Hours: Tuesday 3:00PM-6:00PM | Thursday 3:00PM-6:00PM</p>
                            <p>JDoe02@email.com | Office:Tutoring Center L123</p>
                        </div>
                    </div>
                </fieldset>
            </div>
    </main>
</asp:Content>
