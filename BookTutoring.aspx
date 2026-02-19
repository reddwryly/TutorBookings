<%@ Page Title="Book Tutoring" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BookTutoring.aspx.cs" Inherits="TutorBookings._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div>
            <!--Dynamic Content-->
            <fieldset>
                <label for="tutors">Tutor:</label>
                <select id="tutors" name="tutors" required>
                    <option value="">Select</option>
                    <option value="placeholder1">placeholder1</option>
                    <option value="placeholder2">placeholder2</option>
                </select>

                <label for="course">Course:</label>
                <select id="course" name="course" required>
                    <option value="">Select</option>
                    <option value="placeholder1">placeholder1</option>
                    <option value="placeholder2">placeholder2</option>
                </select>

                <label for="date">Date:</label>
                <input type="date" id="date" name="date" required>

                <label for="time">Time:</label>
                <select id="time" name="time" required>
                    <option value="">Select</option>
                    <option value="placeholder1">placeholder1</option>
                    <option value="placeholder2">placeholder2</option>
                </select>
            </fieldset>

            <fieldset>
                <label for="name">Name:</label>
                <input type="text" id="name" placeholder="First Last" required/>

                <label for="email">Email:</label>
                <input type="email" id="email" placeholder="email@email.com" required/>

                <input type="submit" value="Book!">
            </fieldset>
        </div>
    </main>

</asp:Content>
