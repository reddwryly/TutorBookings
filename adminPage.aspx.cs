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
                        s.FirstName + ' ' + s.LastName AS StudentName
                    FROM Appointment a
                    LEFT JOIN Student s ON a.StudentEmail = s.StudentEmail"
                    ).ToList();
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
                LoadAppointments();
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

                // DEBUG
                foreach (var appt in appointments)
                {
                    Response.Write($"{appt.CourseCode} <br>");
                }
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
                        TutorID = TutorInput.Text.Trim(),
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

            string tutorID = ((TextBox)row.Cells[2].Controls[0]).Text;
            string courseCode = ((TextBox)row.Cells[3].Controls[0]).Text;
            string date = ((TextBox)row.Cells[4].Controls[0]).Text;
            string time = ((TextBox)row.Cells[5].Controls[0]).Text;
            string studentEmail = ((TextBox)row.Cells[6].Controls[0]).Text.Trim();

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
                          OriginalStudentEmail = AppointmentsGrid.DataKeys[e.RowIndex].Values[3],
                          OriginalCourseCode = AppointmentsGrid.DataKeys[e.RowIndex].Values[4]
                    });

                // debug
                Response.Write("Rows affected: " + rows);
            }

            AppointmentsGrid.EditIndex = -1;
            LoadAppointments();
        }
    }
}