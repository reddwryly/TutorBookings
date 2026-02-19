<%@ Page Title="About Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AboutTutoring.aspx.cs" Inherits="TutorBookings.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
            <div>
                <h3>Why get turoing?</h3>
                <p>Tutors can help with:</p>
                <ul>
                    <li>Deepening your understanding through practice, explenations, and examples</li>
                    <li>Help you with homework, projects, or studing for upcoming exams/quizes</li>
                    <li>Help you build fundamental study skills such as note taking and memorization tricks</li>
                </ul>
                <h3>How it works!</h3> 
                <p>For no cost, students can book a tutoring apointment for a course with a student who has previously talken the class and excelled. Book a time that works for the both of you, meet up for an hour long session and get studying! Bring homework, practice exams, or a topic you need help with. Meet in the library, check in, and your tutor will take it from there. Book your appintment today!</p>
                <a class="link" href="BookTutoring.aspx">Book Tutoring</a>
                <h3>Want to become a tutor?</h3>
                <p>You must have:</p>
                <ul>
                    <li>A cumulative GPA of 3.0 or higher</li>
                    <li>An A or B in the course(s) you want to tutor</li>
                    <li>A letter of recomendation from a professor</li>
                </ul>
                <p>If you think you would be a good fit or have other tutoring questions please contact our tutoring coordinator Ms.Tutor at MTutor@email.com !</p>
                <br />
                <h3>Courses Available for Tutoring</h3>
                <!--populate from DB-->
                <p>*place holder</p>
            </div>
    </main>
</asp:Content>
