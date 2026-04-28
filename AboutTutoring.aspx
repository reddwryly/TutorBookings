<%@ Page Title="About Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="AboutTutoring.aspx.cs" Inherits="TutorBookings.AboutTutoring" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <div class="Main">
            <fieldset>
                <h3>Why get tutoring?</h3>
                <p>Tutors can help with:</p>
                <ul>
                    <li>Deepening your understanding through practice, explanations, and examples</li>
                    <li>Assist with homework, projects, or studying for upcoming exams/quizzes</li>
                    <li>Help build fundamental study skills such as note-taking and memorization tricks</li>
                </ul>
                <h3>How it works!</h3> 
                <p>For free, students can book a tutoring appointment for a course with a student who has previously taken the class and excelled. Book a time that works for both of you, meet up for an hour-long session, and get studying! Bring homework, practice exams, or a topic you need help with. Meet on the top floor of the library, check in on the iPad, and your tutor will take it from there. Book your appointment today!</p>
                <a class="link" href="BookTutoring.aspx">Book Tutoring</a>

                <h3>Courses available for tutoring: </h3>

                <!--dynamic list from db-->
                <ul>
                    <!--ASP.NET server control displays all content like a loop-->
                    <asp:Repeater ID="CoursesList" runat="server">
                        <ItemTemplate>
                            <li><%# Eval("CourseCode") %> - <%# Eval("Name") %></li> <!--EVAL("Field name") pulls data from the databind-->
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>

                <h4 style="text-align: center">Any unanswered questions? Contact our Tutoring Coordinator or Head Tutor!</h4>
                <a class="link" href="Contact.aspx">Contact Us</a>
                <br />
            </fieldset>
        </div>
    </main>
</asp:Content>
