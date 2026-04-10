<%@ Page Title="About Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AboutTutoring.aspx.cs" Inherits="TutorBookings.AboutTutoring" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="Main">
            <fieldset>
                <h3>Why get turoing?</h3>
                <p>Tutors can help with:</p>
                <ul>
                    <li>Deepening your understanding through practice, explenations, and examples</li>
                    <li>Assist with homework, projects, or studing for upcoming exams/quizes</li>
                    <li>Help build fundamental study skills such as note taking and memorization tricks</li>
                </ul>
                <h3>How it works!</h3> 
                <p>For free, students can book a tutoring apointment for a course with a student who has previously talken the class and excelled. Book a time that works for the both of you, meet up for an hour long session and get studying! Bring homework, practice exams, or a topic you need help with. Meet in the top floor of the library, check in, and your tutor will take it from there. Book your appintment today!</p>
                <a class="link" href="BookTutoring.aspx">Book Tutoring</a>

                <h3>Courses available for tutoring:</h3>

                <!--dynamic list from db-->
                <ul>
                    <!--ASP.NET server control displays all content like a loop-->
                    <asp:Repeater ID="CoursesList" runat="server">
                        <ItemTemplate>
                            <li><%# Eval("CourseCode") %> - <%# Eval("Name") %></li> <!--EVAL("Field name") pulls data from the databind-->
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>

                <h3>Any unanswered questions?</h3>
                <p>Contact our Tutoring Coordinator or Head Tutor!</p>
                <a class="link" href="Contact.aspx">Contact Us</a>
                <br />
            </fieldset>
        </div>
    </main>
</asp:Content>
