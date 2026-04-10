using Dapper;
using Microsoft.Ajax.Utilities;
using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TutorBookings.Database_SQL;
using static TutorBookings.Database_SQL.Models;

namespace TutorBookings
{
    public partial class BookTutoring : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
                LoadTutors();
            }
        }

        protected void LoadTutors()
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sqlTutors = "SELECT FirstName, LastName, TutorID FROM Tutor";
                var Tutor = db.Query<Tutor>(sqlTutors).ToList();

                foreach (var t in Tutor)
                {
                    TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                }
            }
        }

        protected void LoadCourses()
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sqlCourses = "SELECT CourseCode, Name FROM Course";
                var Course = db.Query<Course>(sqlCourses).ToList();

                foreach (var c in Course)
                {
                    CourseDDL.Items.Add(new ListItem($"{c.CourseCode} - {c.Name}", c.CourseCode));
                }
            }
        }

        protected void LoadDates(object sender, DayRenderEventArgs e)
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sqlSemester = "SELECT StartDate, EndDate FROM Semester WHERE Active = 1";
                var Semester = db.QuerySingle<Semester>(sqlSemester);

                DateTime StartDate = DateTime.Parse(Semester.StartDate).Date;
                DateTime EndDate = DateTime.Parse(Semester.EndDate).Date;

                if (Semester != null && TutorDDL.SelectedValue == "0" && CourseDDL.SelectedValue == "0")
                {
                    if (e.Day.Date < DateTime.Today.AddDays(1) || e.Day.Date > DateTime.Today.AddDays(30) || StartDate > e.Day.Date || e.Day.Date > EndDate)
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                    return;
                }

                e.Day.IsSelectable = false;
                e.Cell.ForeColor = System.Drawing.Color.Gray;

                var TutorAvailabilityDays = ViewState["TutorAvailability"] as List<string>;
                var DayOff = ViewState["TimeOff"] as List<string>;

                if (TutorAvailabilityDays != null && TutorDDL.SelectedValue != "0")
                {
                    string currentCalendarDay = e.Day.Date.DayOfWeek.ToString().ToLower();

                    if (TutorAvailabilityDays.Contains(currentCalendarDay)) {
                        e.Day.IsSelectable = true;
                        e.Cell.ForeColor = System.Drawing.Color.Black;
                    }
                }

                foreach (var d in DayOff)
                {
                    DateTime date = DateTime.Parse(d).Date;

                    if (date == e.Day.Date) {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                }

                if (Semester != null)
                {
                    if (e.Day.Date < DateTime.Today.AddDays(1) || e.Day.Date > DateTime.Today.AddDays(30) || StartDate > e.Day.Date || e.Day.Date > EndDate)
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }
                }
            }
        }

        protected void TutorSelected(object sender, EventArgs e)
        {
            var currentCourse = CourseDDL.SelectedValue;
            CourseDDL.Items.Clear();
            CourseDDL.Items.Add(new ListItem("select", "0"));

            if (TutorDDL.SelectedValue == "0")
            {
                LoadCourses();
            }

            using (var db = DatabaseHelper.Connect()) {

                var sqlJoinTutorCourseCourse = $"SELECT tc.CourseCode, c.Name " +
                                                $"FROM TutorCourse as tc " +
                                                $"INNER JOIN Course c ON tc.CourseCode = c.CourseCode " +
                                                $"WHERE TutorId= '{TutorDDL.SelectedValue}'";
                var TutorCourseCourseTutorCourse = db.Query<Course>(sqlJoinTutorCourseCourse).ToList();

                foreach (var tc in TutorCourseCourseTutorCourse)
                {
                    CourseDDL.Items.Add(new ListItem($"{tc.CourseCode} - {tc.Name}", tc.CourseCode));
                }

                if (CourseDDL.Items.FindByValue(currentCourse) != null)
                {
                    CourseDDL.SelectedValue = currentCourse;
                }

                //date 
                var sqlAvailability = $"SELECT Weekday FROM TutorAvailability WHERE TutorId = '{TutorDDL.SelectedValue}'";
                var TutorAvailability = db.Query<TutorAvailability>(sqlAvailability).ToList();

                var sqlApointment = $"SELECT Date FROM Appointment WHERE TutorId = '{TutorDDL.SelectedValue}'";
                var Appointment = db.Query<Appointment>(sqlApointment).ToList();

                var sqlTimeOff = $"SELECT Date FROM TimeOff WHERE TutorId = '{TutorDDL.SelectedValue}'";
                var TimeOff = db.Query<TimeOff>(sqlTimeOff).ToList();

                var TutorAvailabilityDays = TutorAvailability.Select(a => a.Weekday.ToLower()).ToList();
                ViewState["TutorAvailability"] = TutorAvailabilityDays;

                var DayOff = TimeOff.Select(a => a.Date).ToList();
                ViewState["TimeOff"] = DayOff;
            }
        }

        protected void CourseSelected(object sender, EventArgs e)
        {
            var currentTutor = TutorDDL.SelectedValue;
            TutorDDL.Items.Clear();
            TutorDDL.Items.Add(new ListItem("select", "0"));

            if (CourseDDL.SelectedValue == "0")
            {
               
                LoadTutors();
            }

            using (var db = DatabaseHelper.Connect())
            {
                var sqlJoinTutorCourseTutor = $"SELECT t.FirstName, t.LastName, tc.TutorId " +
                                               $"FROM TutorCourse as tc INNER JOIN Tutor as t ON tc.TutorID = t.TutorId " +
                                               $"WHERE CourseCode = '{CourseDDL.SelectedValue}'";
                var TutorCourseCourseTutorTutor = db.Query<Tutor>(sqlJoinTutorCourseTutor).ToList();

                foreach (var t in TutorCourseCourseTutorTutor)
                {
                    TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                }

                if (TutorDDL.Items.FindByValue(currentTutor) != null)
                {
                    TutorDDL.SelectedValue = currentTutor;
                }
            }

            //date 
        }

        protected void DateSelected(object sender, EventArgs e)
        {
            TimeDDL.Enabled = true;

            //tutor 

            //course

            //time 

        }

        protected void EnableTime()
        {
           
        }

        protected void SubmitButton(object sender, EventArgs e)
        {
            //insert statement
            //clear form 
        }
    }
}