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
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
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
                confirmed.Text = "";
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
                var sqlCourses = "SELECT CourseCode, Name FROM Course ORDER BY CourseCode";
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

                if (TutorDDL.SelectedValue != "0") {
                    e.Day.IsSelectable = false;
                    e.Cell.ForeColor = System.Drawing.Color.Gray;

                    var TutorAvailabilityDays = ViewState["TutorAvailability"] as List<string>;
                    var DayOff = ViewState["TimeOff"] as List<string>;

                    if (TutorAvailabilityDays != null)
                    {
                        string currentCalendarDay = e.Day.Date.DayOfWeek.ToString().ToLower();

                        if (TutorAvailabilityDays.Contains(currentCalendarDay)) {
                            e.Day.IsSelectable = true;
                            e.Cell.ForeColor = System.Drawing.Color.Black;
                        }
                    }

                    if (DayOff != null)
                    {
                        foreach (var d in DayOff)
                        {
                            DateTime date = DateTime.Parse(d).Date;

                            if (date == e.Day.Date)
                            {
                                e.Day.IsSelectable = false;
                                e.Cell.ForeColor = System.Drawing.Color.Gray;
                            }
                        }
                    }
                } else if (CourseDDL.SelectedValue != "0")
                {
                    e.Day.IsSelectable = false;
                    e.Cell.ForeColor = System.Drawing.Color.Gray;

                    var CourseAvailabilityDays = ViewState["CourseAvailability"] as List<string>;

                    if (CourseAvailabilityDays != null)
                    {
                        string currentCalendarDay = e.Day.Date.DayOfWeek.ToString().ToLower();

                        if (CourseAvailabilityDays.Contains(currentCalendarDay))
                        {
                            e.Day.IsSelectable = true;
                            e.Cell.ForeColor = System.Drawing.Color.Black;
                        }
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

        protected void LoadTimes() //change hashset to list of datetimes, collect all times, then display all formatted times (will fix ordering issue)**
        {
            using (var db = DatabaseHelper.Connect())
            {
                var addedTimes = new HashSet<string>();
                var times = new List<(string display, string value)>();
                var currenttime = TimeDDL.SelectedValue;
                TimeDDL.Items.Clear();
                TimeDDL.Items.Add(new ListItem("select", "0"));

                var sqlAppointmentTimes = $"SELECT time " +
                                            $"FROM Appointment " +
                                            $"WHERE date = '{Date.SelectedDate.ToString("yyyy-MM-dd")}' AND TutorId = '{TutorDDL.SelectedValue}'";
                var AppointmentTimes = db.Query<Appointment>(sqlAppointmentTimes).ToList();

                if (CourseDDL.SelectedValue == "0" && TutorDDL.SelectedValue == "0") { //load all times for day

                    var sqltutortime = $"SELECT DISTINCT StartTime, EndTime " +
                                            $"FROM TutorAvailability " +
                                            $"WHERE Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}'" +
                                            $"ORDER BY StartTime DESC";
                    var tutorTime = db.Query<TutorAvailability>(sqltutortime).ToList();

                    foreach (var t in tutorTime)
                    {
                        var sTime = DateTime.ParseExact(t.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                        var eTime = DateTime.ParseExact(t.EndTime, "HH:mm", CultureInfo.InvariantCulture);

                        while (sTime < eTime)
                        {
                            var time = (sTime).ToString("h:mm tt");
                            var dbTime = sTime.ToString("HH:mm");
                            if (addedTimes.Add(time) && !AppointmentTimes.Any(s => s.Time == dbTime))
                            {
                                times.Add((time, dbTime));
                            }
                            sTime = sTime.AddHours(1);
                        }
                    }
                } else if (CourseDDL.SelectedValue == "0") //load based on tutor
                {

                    var sqltutortime = $"SELECT DISTINCT StartTime, EndTime " +
                                                $"FROM TutorAvailability ta INNER JOIN Tutor t ON t.TutorId = ta.TutorId " +
                                                $"WHERE Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}' AND ta.TutorId = '{TutorDDL.SelectedValue}'" +
                                                $"ORDER BY StartTime DESC";
                    var tutorTime = db.Query<TutorAvailability>(sqltutortime).ToList();

                    foreach (var t in tutorTime)
                    {
                        var sTime = DateTime.ParseExact(t.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                        var eTime = DateTime.ParseExact(t.EndTime, "HH:mm", CultureInfo.InvariantCulture);

                        while (sTime < eTime)
                        {
                            var time = (sTime).ToString("h:mm tt");
                            var dbTime = sTime.ToString("HH:mm");
                            if (addedTimes.Add(time) && !AppointmentTimes.Any(s => s.Time == dbTime))
                            {
                                times.Add((time, dbTime));
                            }
                            sTime = sTime.AddHours(1);
                        }
                    }

                } else if (TutorDDL.SelectedValue == "0") //load based on course
                {
                    var sqltutortime = $"SELECT DISTINCT ta.StartTime, ta.EndTime " +
                                                $"FROM TutorAvailability ta INNER JOIN Course c ON c.CourseCode = tc.CourseCode " +
                                                $"INNER JOIN TutorCourse tc ON ta.TutorId = tc.TutorId " +
                                                $"WHERE ta.Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}' AND tc.CourseCode = '{CourseDDL.SelectedValue}' " +
                                                $"ORDER BY StartTime DESC";
                    var tutorTime = db.Query<TutorAvailability>(sqltutortime).ToList();

                    foreach (var t in tutorTime)
                    {
                        var sTime = DateTime.ParseExact(t.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                        var eTime = DateTime.ParseExact(t.EndTime, "HH:mm", CultureInfo.InvariantCulture);

                        while (sTime < eTime)
                        {
                            var time = (sTime).ToString("h:mm tt");
                            var dbTime = sTime.ToString("HH:mm");
                            if (addedTimes.Add(time) && !AppointmentTimes.Any(s => s.Time == dbTime))
                            {
                                times.Add((time, dbTime));
                            }
                            sTime = sTime.AddHours(1);
                        }
                    }
                } else 
                {
                    var sqltutortime = $"SELECT DISTINCT ta.StartTime, ta.EndTime " +
                                                $"FROM TutorAvailability ta INNER JOIN Course c ON c.CourseCode = tc.CourseCode " +
                                                $"INNER JOIN TutorCourse tc ON ta.TutorId = tc.TutorId " +
                                                $"WHERE ta.Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}' AND tc.CourseCode = '{CourseDDL.SelectedValue}' AND ta.TutorId = '{TutorDDL.SelectedValue}'" +
                                                $"ORDER BY StartTime DESC";
                    var tutorTime = db.Query<TutorAvailability>(sqltutortime).ToList();

                    foreach (var t in tutorTime)
                    {
                        var sTime = DateTime.ParseExact(t.StartTime, "HH:mm", CultureInfo.InvariantCulture);
                        var eTime = DateTime.ParseExact(t.EndTime, "HH:mm", CultureInfo.InvariantCulture);

                        while (sTime < eTime)
                        {
                            var time = (sTime).ToString("h:mm tt");
                            var dbTime = sTime.ToString("HH:mm");
                            if (addedTimes.Add(time) && !AppointmentTimes.Any(s => s.Time == dbTime))
                            {
                                times.Add((time, dbTime));
                            }
                            sTime = sTime.AddHours(1);
                        }
                    }
                }

                foreach (var t in times.OrderBy(t => DateTime.ParseExact(t.value, "HH:mm", CultureInfo.InvariantCulture)))
                {
                    TimeDDL.Items.Add(new ListItem(t.display, t.value));
                }

                if (TimeDDL.Items.FindByValue(currenttime) != null)
                {
                    TimeDDL.SelectedValue = currenttime;
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

                //course
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

                var sqlTimeOff = $"SELECT Date FROM TimeOff WHERE TutorId = '{TutorDDL.SelectedValue}'";
                var TimeOff = db.Query<TimeOff>(sqlTimeOff).ToList();

                var TutorAvailabilityDays = TutorAvailability.Select(a => a.Weekday.ToLower()).ToList();
                ViewState["TutorAvailability"] = TutorAvailabilityDays;

                var DayOff = TimeOff.Select(a => a.Date).ToList();
                ViewState["TimeOff"] = DayOff;

                //time
                LoadTimes();
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
                LoadTimes();
            }

            using (var db = DatabaseHelper.Connect())
            {
                //tutor
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

                //date 
                var sqlAvailability = $"SELECT ta.Weekday " +
                                        $"FROM TutorAvailability ta INNER JOIN TutorCourse as tc ON ta.TutorID = tc.TutorId " +
                                        $"WHERE CourseCode = '{CourseDDL.SelectedValue}'";
                var TutorAvailability = db.Query<TutorAvailability>(sqlAvailability).ToList();

                var CourseAvailabilityDays = TutorAvailability.Select(a => a.Weekday.ToLower()).ToList();
                ViewState["CourseAvailability"] = CourseAvailabilityDays;

                //time
                LoadTimes();
            }
        }

        protected void DateSelected(object sender, EventArgs e) 
        {
            TimeDDL.Enabled = true;

            using (var db = DatabaseHelper.Connect())
            {
                //tutor
                var currentTutor = TutorDDL.SelectedValue;
                TutorDDL.Items.Clear();
                TutorDDL.Items.Add(new ListItem("select", "0"));

                if (CourseDDL.SelectedValue == "0") {
                    var sqlAvailability = $"SELECT ta.TutorId, t.FirstName, t.LastName " +
                                           $"FROM TutorAvailability ta INNER JOIN Tutor t ON ta.TutorId = t.TutorId " +
                                           $"WHERE Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}'";
                    var TutorAvailability = db.Query<Tutor>(sqlAvailability).ToList();

                    foreach (var t in TutorAvailability)
                    {
                        TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                    }
                } else
                {
                    var sqlAvailability = $"SELECT ta.TutorId, t.FirstName, t.LastName " +
                                           $"FROM TutorAvailability ta INNER JOIN Tutor t ON ta.TutorId = t.TutorId " +
                                           $"INNER JOIN TutorCourse tc ON ta.TutorId = tc.TutorId " +
                                           $"WHERE Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}' AND tc.CourseCode = '{CourseDDL.SelectedValue}'";
                    var TutorAvailability = db.Query<Tutor>(sqlAvailability).ToList();

                    foreach (var t in TutorAvailability)
                    {
                        TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                    }
                }

                if (TutorDDL.Items.FindByValue(currentTutor) != null)
                {
                    TutorDDL.SelectedValue = currentTutor;
                }

                //courses
                var currentCourse = CourseDDL.SelectedValue;
                CourseDDL.Items.Clear();
                CourseDDL.Items.Add(new ListItem("select", "0"));

                if (TutorDDL.SelectedValue == "0")
                {
                    var sqlCourseAvailability = $"SELECT DISTINCT c.CourseCode, c.Name " +
                                                $"FROM Course c INNER JOIN TutorCourse tc ON c.CourseCode = tc.CourseCode " +
                                                $"INNER JOIN TutorAvailability ta ON ta.TutorId = tc.TutorId " +
                                                $"WHERE ta.Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}'";
                    var CourseAvailability = db.Query<Course>(sqlCourseAvailability).ToList();

                    foreach (var c in CourseAvailability)
                    {
                        CourseDDL.Items.Add(new ListItem($"{c.CourseCode} - {c.Name}", c.CourseCode));
                    }
                } else
                {
                    var sqlCourseAvailability = $"SELECT DISTINCT c.CourseCode, c.Name " +
                                                $"FROM Course c INNER JOIN TutorCourse tc ON c.CourseCode = tc.CourseCode " +
                                                $"INNER JOIN TutorAvailability ta ON ta.TutorId = tc.TutorId " +
                                                $"WHERE ta.Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}' AND ta.TutorId = '{TutorDDL.SelectedValue}'";
                    var CourseAvailability = db.Query<Course>(sqlCourseAvailability).ToList();

                    foreach (var c in CourseAvailability)
                    {
                        CourseDDL.Items.Add(new ListItem($"{c.CourseCode} - {c.Name}", c.CourseCode));
                    }
                }

                if (CourseDDL.Items.FindByValue(currentCourse) != null)
                {
                    CourseDDL.SelectedValue = currentCourse;
                }

                //time
                LoadTimes();
            }
        }

        protected void cvDate_Validation(object source, ServerValidateEventArgs args)
        {
            if (Date.SelectedDate == DateTime.MinValue)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }

        }

        protected void cvTutor_Validation(object source, ServerValidateEventArgs args)
        {
            if (TutorDDL.SelectedValue == "0")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cvCourse_Validation(object source, ServerValidateEventArgs args)
        {
            if (CourseDDL.SelectedValue == "0")
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void cvTime_Validation(object source, ServerValidateEventArgs args)
        {
            if (TimeDDL.SelectedValue == "0" || TimeDDL.Enabled == false)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
        }

        protected void SubmitButton(object sender, EventArgs e)
        {

            if (Page.IsValid)
            {
                var email = Email.Text;
                var fName = FName.Text;
                var lName = LName.Text;
                var time = TimeDDL.SelectedValue;
                var date = Date.SelectedDate.ToString("yyyy-MM-dd");
                var tutor = TutorDDL.SelectedValue;
                var course = CourseDDL.SelectedValue;
                confirmed.Text = $"Tutoring with {TutorDDL.SelectedItem} for {CourseDDL.SelectedItem} on {date} at {TimeDDL.SelectedItem} is Scheduled!";

                try
                {
                    using (var db = DatabaseHelper.Connect())
                    {
                        var sql = "INSERT INTO Appointment (TutorID, Date, Time, StudentEmail, CourseCode) VALUES (@TutorID, @Date, @Time, @StudentEmail, @CourseCode)";
                        {
                            var insert = new { TutorID = $"{tutor}", Date = $"{date}", Time = $"{time}", StudentEmail = $"{email}", CourseCode = $"{course}" };
                            var rowsAffected = db.Execute(sql, insert);
                            Console.WriteLine($"{rowsAffected} row(s) inserted.");
                        }
                        var sql2 = "INSERT INTO Student (StudentEmail, FirstName, LastName) VALUES (@StudentEmail, @FirstName, @LastName)";
                        {
                            var insert = new { StudentEmail = $"{email}", FirstName = $"{fName}", LastName = $"{lName}" };
                            var rowsAffected = db.Execute(sql2, insert);
                            Console.WriteLine($"{rowsAffected} row(s) inserted.");
                        }
                    }
                }
                catch
                {
                    confirmed.Text = "Selection Error";
                }


                Email.Text = "";
                FName.Text = "";
                LName.Text = "";
                TimeDDL.SelectedValue = "0";
                Date.SelectedDate = DateTime.MinValue;
                TutorDDL.SelectedValue = "0";
                CourseDDL.SelectedValue = "0";
            }
        }
    }
}