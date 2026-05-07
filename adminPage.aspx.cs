using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using TutorBookings.Database_SQL;
using System.Web.Caching;
using Dapper;

namespace TutorBookings
{
    public partial class adminPage : System.Web.UI.Page
    {
        
        public class Appointment
        {
            public int Id { get; set; }
            public string StudentName { get; set; }
            public string TutorName { get; set; }
            public string TutorID { get; set; }
            public string CourseCode { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string StudentEmail { get; set; }
        }

        public class Tutor
        {
            public string TutorID { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string FullName
            {
                get {  return FirstName + " " + LastName; }
            }
        }

        private void LoadTutors()
        {
            using (var connection = DatabaseHelper.Connect())
            {
                var tutors = connection.Query<Tutor>("SELECT TutorID, FirstName, LastName FROM Tutor").ToList();
                TutorDropdown.DataSource = tutors;
                TutorDropdown.DataTextField = "FullName";
                TutorDropdown.DataValueField = "TutorID";

                TutorDropdown.DataBind();
            }
        }

        // helper function to retrieve appointment info
        private List<Appointment> GetAppointments()
        {
            using (var connection = DatabaseHelper.Connect())
            {
                return connection.Query<Appointment>(
                    @"SELECT
                        a.TutorID, 
                        a.Date, 
                        a.Time, 
                        a.StudentEmail, 
                        a.CourseCode,
                        s.FirstName || ' ' || s.LastName AS StudentName,
                        t.FirstName || ' ' || t.LastName AS TutorName
                    FROM Appointment a
                    LEFT JOIN Student s ON a.StudentEmail = s.StudentEmail
                    LEFT JOIN Tutor t ON a.TutorID = t.TutorID"
                    ).ToList();
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                LoadAppointments();
                LoadTutors();
            }
        }

        // THE MAIN function for loading/refreshing appointment list
        private void LoadAppointments()
        {
            using (var connection = DatabaseHelper.Connect())
            {
                var appointments = GetAppointments();

                AppointmentsGrid.DataSource = appointments;
                AppointmentsGrid.DataBind();

            }
        }

        
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var button = (System.Web.UI.WebControls.Button)sender;
            int id = int.Parse(button.CommandArgument);

            
            var appt = GetAppointments().FirstOrDefault(a => a.Id == id);
            if (appt != null)
            {
                GetAppointments().Remove(appt);
            }

            
            LoadAppointments();
        }

        protected void EditButton_Click(object sender, EventArgs e)
        {
            var button = (System.Web.UI.WebControls.Button)sender;
            int id = int.Parse(button.CommandArgument);

        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            using (var connection = DatabaseHelper.Connect())
            {
                connection.Execute(
                    @"INSERT INTO Appointment (TutorID, Date, Time, StudentEmail, CourseCode)
                   VALUES (@TutorID, @Date, @Time, @StudentEmail, @CourseCode)",

                    new
                    {
                        TutorID = TutorDropdown.SelectedValue,
                        Date = DateInput.Text.Trim(),
                        Time = TimeInput.Text.Trim(),
                        StudentEmail = StudentEmailInput.Text.Trim(),
                        CourseCode = CourseCodeInput.Text.Trim()
                    });
            }
            LoadAppointments();
        }

        protected void AppointmentsGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            AppointmentsGrid.EditIndex = e.NewEditIndex;
            LoadAppointments();
        }

        protected void AppointmentsGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            AppointmentsGrid.EditIndex = -1;
            LoadAppointments();
        }

        // handles updating appointments, one row at a time
        protected void AppointmentsGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // gets the new updated values
            GridViewRow row = AppointmentsGrid.Rows[e.RowIndex];

            DropDownList tutorDropdown = (DropDownList)row.FindControl("TutorDropdownEdit");
            string tutorID = tutorDropdown.SelectedValue;

            TextBox courseCodeTextbox = (TextBox)row.FindControl("CourseCodeTextbox");
            string courseCode = courseCodeTextbox.Text.Trim();

            TextBox dateTextBox = (TextBox)row.FindControl("DateTextBox");
            string date = dateTextBox.Text.Trim();

            TextBox timeTextBox = (TextBox)row.FindControl("TimeTextBox");
            string time = timeTextBox.Text.Trim();

            TextBox studentEmailTextBox = (TextBox)row.FindControl("StudentEmailTextBox");
            string studentEmail = studentEmailTextBox.Text.Trim();

            // stores the original field values
            var keys = AppointmentsGrid.DataKeys[e.RowIndex];
            string originalTutorID = keys.Values["TutorID"].ToString();
            string originalDate = keys.Values["Date"].ToString();
            string originalTime = keys.Values["Time"].ToString();
            string originalStudentEmail = keys.Values["StudentEmail"].ToString();
            string originalCourseCode = keys.Values["CourseCode"].ToString();

            using (var connection = DatabaseHelper.Connect())
            {
                var exists = connection.ExecuteScalar<int>(
                    "SELECT COUNT(1) FROM Student WHERE StudentEmail = @Email",
                    new { Email = studentEmail });

                // executes if email is invalid (not found in Student table)
                if (exists == 0)
                {
                    Response.Write("<script>alert('Student email invalid. Please enter a valid email.');</script>");

                    AppointmentsGrid.EditIndex = -1;
                    LoadAppointments();
                    return;
                }



                int rows = connection.Execute(
                    @"UPDATE Appointment
                        SET TutorID = @TutorID, CourseCode = @CourseCode, Date = @Date, Time = @Time, StudentEmail = @StudentEmail
                        WHERE TutorID = @OriginalTutorID AND Date = @OriginalDate AND Time = @OriginalTime",

                    new { TutorID = tutorID, 
                          CourseCode = courseCode,
                          Date = date,
                          Time = time,
                          StudentEmail = studentEmail,
                    
                          // original values for WHERE
                          OriginalTutorID = AppointmentsGrid.DataKeys[e.RowIndex].Values[0],
                          OriginalDate = AppointmentsGrid.DataKeys[e.RowIndex].Values[1],
                          OriginalTime = AppointmentsGrid.DataKeys[e.RowIndex].Values[2],
                    });

            }

            AppointmentsGrid.EditIndex = -1;
            LoadAppointments();
        }

        protected void AppointmentsGrid_RowDataBound(
            object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState & DataControlRowState.Edit) > 0)
            {
                DropDownList tutorDropdown = (DropDownList)e.Row.FindControl("TutorDropdownEdit");

                if (tutorDropdown != null)
                {
                    using (var connection = DatabaseHelper.Connect())
                    {
                        var tutors = connection.Query<Tutor>(
                            @"Select TutorID, FirstName, LastName FROM Tutor").ToList();

                        tutorDropdown.DataSource = tutors;
                        tutorDropdown.DataTextField = "FullName";
                        tutorDropdown.DataValueField = "TutorID";

                        tutorDropdown.DataBind();
                    }
                }
            }
        }
    }
}