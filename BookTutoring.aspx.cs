using Dapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

                if (TutorDDL.SelectedValue != "0")
                {
                    e.Day.IsSelectable = false;
                    e.Cell.ForeColor = System.Drawing.Color.Gray;

                    var TutorAvailabilityDays = ViewState["TutorAvailability"] as List<string>;
                    var DayOff = ViewState["TimeOff"] as List<string>;

                    if (TutorAvailabilityDays != null)
                    {
                        string currentCalendarDay = e.Day.Date.DayOfWeek.ToString().ToLower();

                        if (TutorAvailabilityDays.Contains(currentCalendarDay))
                        {
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
                }
                else if (CourseDDL.SelectedValue != "0")
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

                if (CourseDDL.SelectedValue == "0" && TutorDDL.SelectedValue == "0")
                { //load all times for day

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
                }
                else if (CourseDDL.SelectedValue == "0") //load based on tutor
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

                }
                else if (TutorDDL.SelectedValue == "0") //load based on course
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
                }
                else
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

            using (var db = DatabaseHelper.Connect())
            {

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

                if (Date.SelectedDate != DateTime.MinValue)
                {
                    bool hasTimeOff = DayOff.Any(d =>
                        DateTime.Parse(d).Date == Date.SelectedDate.Date);

                    if (hasTimeOff)
                    {
                        Date.SelectedDate = DateTime.MinValue;
                    }
                }

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

                if (CourseDDL.SelectedValue == "0")
                {
                    var sqlAvailability = $"SELECT ta.TutorId, t.FirstName, t.LastName " +
                                           $"FROM TutorAvailability ta INNER JOIN Tutor t ON ta.TutorId = t.TutorId " +
                                           $"WHERE Weekday = '{Date.SelectedDate.DayOfWeek.ToString().ToLower()}'";
                    var TutorAvailability = db.Query<Tutor>(sqlAvailability).ToList();

                    foreach (var t in TutorAvailability)
                    {
                        TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                    }
                }
                else
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
                }
                else
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

        protected void RepeatingAppointmentPlus(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex += 1;
        }
        protected void RepeatingAppointmentMinus(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex -= 1;
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

        protected void LoadDatesView2(object sender, DayRenderEventArgs e)
        {
            using (var db = DatabaseHelper.Connect())
            {
                View View2 = (View)MultiView1.FindControl("View2");

                if (View2 != null)
                {
                    CheckBox checkbox = (CheckBox)View2.FindControl("checkbox1");
                    System.Web.UI.WebControls.Calendar cal1 = (System.Web.UI.WebControls.Calendar)View2.FindControl("Calendar1");
                    var sqlSemester = "SELECT StartDate, EndDate FROM Semester WHERE Active = 1";
                    var Semester = db.QuerySingle<Semester>(sqlSemester);

                    DateTime StartDate = DateTime.Parse(Semester.StartDate).Date;
                    DateTime EndDate = DateTime.Parse(Semester.EndDate).Date;

                    var sqlA = $"SELECT Date FROM Appointment WHERE TutorId = '{TutorDDL.SelectedValue}' AND Time = '{TimeDDL.SelectedValue}'";
                    var Appointment = db.Query<Appointment>(sqlA).ToList();

                    var sqlAall = $"SELECT Date FROM Appointment WHERE Time = '{TimeDDL.SelectedValue}'";
                    var AppointmentAll = db.Query<Appointment>(sqlAall).ToList();

                    var sqlTO = $"SELECT Date FROM TimeOff WHERE TutorId = '{TutorDDL.SelectedValue}'";
                    var TimeOff = db.Query<Appointment>(sqlTO).ToList();
                    var sqlTOall = $"SELECT Date FROM TimeOff";
                    var TimeOffAll = db.Query<Appointment>(sqlTOall).ToList();

                    if (Semester != null && checkbox.Checked == false) //box unchecked = load all dates for chosen tutor from start to end of semester
                    {
                        if (e.Day.Date < Date.SelectedDate.AddDays(1) || e.Day.Date > DateTime.Today.AddDays(30) || StartDate > e.Day.Date || e.Day.Date > EndDate || e.Day.Date.DayOfWeek != Date.SelectedDate.DayOfWeek)
                        {
                            e.Day.IsSelectable = false;
                            e.Cell.ForeColor = System.Drawing.Color.Gray;
                        }

                        foreach (var a in Appointment)
                        {
                            DateTime AppointmentDate = DateTime.Parse(a.Date).Date;
                            if (AppointmentDate == e.Day.Date)
                            {
                                e.Day.IsSelectable = false;
                                e.Cell.ForeColor = System.Drawing.Color.Gray;
                            }
                        }

                        foreach (var t in TimeOff)
                        {
                            DateTime TimeOffDate = DateTime.Parse(t.Date).Date;
                            if (TimeOffDate == e.Day.Date)
                            {
                                e.Day.IsSelectable = false;
                                e.Cell.ForeColor = System.Drawing.Color.Gray;
                            }
                        }
                    }
                    else if (checkbox.Checked == true) //box checked = load all dates available for any tutor until end of semester (choose a tutor thats available on dates the other is available)
                    {
                        foreach (var a in AppointmentAll)
                        {
                            DateTime AppointmentDate = DateTime.Parse(a.Date).Date;
                            if (AppointmentDate != e.Day.Date & (StartDate <= e.Day.Date || e.Day.Date <= EndDate || e.Day.Date.DayOfWeek == Date.SelectedDate.DayOfWeek))
                            {
                                e.Day.IsSelectable = true;
                                e.Cell.ForeColor = System.Drawing.Color.Black;
                            }
                        }

                        foreach (var t in TimeOffAll)
                        {
                            DateTime TimeOffDate = DateTime.Parse(t.Date).Date;
                            if (TimeOffDate != e.Day.Date)
                            {
                                e.Day.IsSelectable = true;
                                e.Cell.ForeColor = System.Drawing.Color.Black;
                            }
                        }

                        if (e.Day.Date < Date.SelectedDate.AddDays(1) || e.Day.Date > DateTime.Today.AddDays(30) || StartDate > e.Day.Date || e.Day.Date > EndDate || e.Day.Date.DayOfWeek != Date.SelectedDate.DayOfWeek)
                        {
                            e.Day.IsSelectable = false;
                            e.Cell.ForeColor = System.Drawing.Color.Gray;
                        }
                    }

                    if (MultiView1.ActiveViewIndex == 3 && e.Day.Date > cal1.SelectedDate)
                    {
                        e.Day.IsSelectable = false;
                        e.Cell.ForeColor = System.Drawing.Color.Gray;
                    }

                    List<DateTime> disabledDates = ViewState["DisabledDates"] as List<DateTime>;

                    if (disabledDates != null)
                    {
                        foreach (var d in disabledDates)
                        {
                            DateTime disable = d;
                            if (e.Day.Date == disable)
                            {
                                e.Day.IsSelectable = false;
                                e.Cell.ForeColor = System.Drawing.Color.Gray;
                            }
                        }
                    }
                }
            }
        }

        protected void DateSelectedtoview2(object source, EventArgs e)
        {
            View View2 = (View)MultiView1.FindControl("View2");

            if (View2 != null)
            {
                Button button = (Button)View2.FindControl("Button2");
                if (button != null)
                {
                    button.Enabled = true;
                }
            }
        }

        protected void DateSelectedtoRemove(object source, EventArgs e)
        {
            View View3 = (View)MultiView1.FindControl("View3");

            if (View3 != null)
            {
                Button Remove = (Button)View3.FindControl("RemoveButton");
                if (Remove != null)
                {
                    Remove.Enabled = true;
                }
            }
        }

        protected void Remove(object source, EventArgs e)
        {
            View View3 = (View)MultiView1.FindControl("View3");

            if (View3 != null)
            {
                System.Web.UI.WebControls.Calendar cal2 = (System.Web.UI.WebControls.Calendar)View3.FindControl("Calendar2");
                if (cal2.SelectedDate != DateTime.MinValue)
                {
                    List<DateTime> disabledDates = ViewState["DisabledDates"] as List<DateTime> ?? new List<DateTime>();
                    DateTime selected = cal2.SelectedDate.Date;

                    if (!disabledDates.Contains(selected))
                    {
                        disabledDates.Add(selected);
                    }

                    ViewState["DisabledDates"] = disabledDates;
                    cal2.SelectedDate = DateTime.MinValue;
                }
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

                        //sql to find students if student exists, return (this is why it says theres an error if the student isnt unique)
                        var sql3 = $"SELECT StudentEmail FROM Student WHERE StudentEmail = '{email}'";
                        if (db.ExecuteScalar<bool>("SELECT count(1) FROM Student where StudentEmail = @StudentEmail", new { StudentEmail = email }))
                        {
                            var sql2 = "INSERT INTO Student (StudentEmail, FirstName, LastName) VALUES (@StudentEmail, @FirstName, @LastName)";
                            {
                                var insert = new { StudentEmail = $"{email}", FirstName = $"{fName}", LastName = $"{lName}" };
                                var rowsAffected = db.Execute(sql2, insert);
                                Console.WriteLine($"{rowsAffected} row(s) inserted.");
                            }
                        }
                    }
                    confirmed.Text = $"Tutoring with {TutorDDL.SelectedItem} for {CourseDDL.SelectedItem} on {date} at {TimeDDL.SelectedItem} is Scheduled!"; //after selection error once it doesnt do this when non selection error
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
                TimeDDL.EnableViewState = false;
                TutorDDL.Items.Clear();
                TutorDDL.Items.Add(new ListItem("Select", "0"));
                LoadTutors();
                CourseDDL.Items.Clear();
                CourseDDL.Items.Add(new ListItem("Select", "0"));
                LoadCourses();
            }
        }

        private List<DateTime> GetAvailableDates()
        {
            List<DateTime> availableDates = new List<DateTime>();
            availableDates.Add(Date.SelectedDate);

            using (var db = DatabaseHelper.Connect())
            {
                var sqlSemester = "SELECT StartDate, EndDate FROM Semester WHERE Active = 1";
                var Semester = db.QuerySingle<Semester>(sqlSemester);

                DateTime StartDate = DateTime.Parse(Semester.StartDate).Date;
                DateTime EndDate = DateTime.Parse(Semester.EndDate).Date;

                var sqlA = $"SELECT Date FROM Appointment WHERE TutorId = '{TutorDDL.SelectedValue}' AND Time = '{TimeDDL.SelectedValue}'";
                var Appointment = db.Query<Appointment>(sqlA).ToList();

                var sqlTO = $"SELECT Date FROM TimeOff WHERE TutorId = '{TutorDDL.SelectedValue}'";
                var TimeOff = db.Query<TimeOff>(sqlTO).ToList();

                var sqlAll = $""; 
                var TutorsAll = db.Query<Appointment>(sqlTO).ToList();

                List<DateTime> disabledDates = ViewState["DisabledDates"] as List<DateTime> ?? new List<DateTime>();

                DayOfWeek targetDay = Date.SelectedDate.DayOfWeek;

                View View2 = (View)MultiView1.FindControl("View2");
                CheckBox checkbox = (CheckBox)View2.FindControl("checkbox1");
                bool isBlocked = true;

                for (DateTime d = Date.SelectedDate.AddDays(1); d <= EndDate; d = d.AddDays(1))
                {
                    if (d <= DateTime.Today.AddDays(30) && d.DayOfWeek == targetDay)
                    {
                        if (!checkbox.Checked) {
                            isBlocked = Appointment.Any(a => DateTime.Parse(a.Date).Date == d) ||
                                             TimeOff.Any(t => DateTime.Parse(t.Date).Date == d) ||
                                             disabledDates.Contains(d);
                        } else 
                        {
                            //all available dates for all tutors that tutor that course
                        }

                        if (!isBlocked)
                        {
                            availableDates.Add(d);
                        }
                    }
                }
            }
            return availableDates;
        }

        protected void SubmitButton2(object sender, EventArgs e)
        {

            //needs to check if the checkbox is checked and then add tutors accordingly 
            List<DateTime> datesToBook = GetAvailableDates();

            using (var db = DatabaseHelper.Connect())
            {
                foreach (DateTime apptDate in datesToBook)
                {
                    var sql = "INSERT INTO Appointment (TutorID, Date, Time, StudentEmail, CourseCode) VALUES (@TutorID, @Date, @Time, @StudentEmail, @CourseCode)";
                    var insert = new
                    {
                        TutorID = $"{TutorDDL.SelectedValue}",
                        Date = apptDate.ToString("yyyy-MM-dd"),
                        Time = $"{TimeDDL.SelectedValue}",
                        StudentEmail = $"{Email.Text}",
                        CourseCode = $"{CourseDDL.SelectedValue}"
                    };
                    db.Execute(sql, insert);
                }

                if (!db.ExecuteScalar<bool>("SELECT count(1) FROM Student where StudentEmail = @StudentEmail", new { StudentEmail = Email.Text }))
                {
                    var sql2 = "INSERT INTO Student (StudentEmail, FirstName, LastName) VALUES (@StudentEmail, @FirstName, @LastName)";
                    var insertStudent = new { StudentEmail = $"{Email.Text}", FirstName = $"{FName.Text}", LastName = $"{LName.Text}" };
                    db.Execute(sql2, insertStudent);
                }

                var sqlStudent = "SELECT FirstName, LastName FROM Student WHERE StudentEmail = @StudentEmail";
                var students = db.QuerySingle<Student>(sqlStudent, new { StudentEmail = Email.Text });

                var sqlAppointment = "SELECT * " +
                                      "FROM AppointmentView " +
                                      "WHERE StudentEmail = @StudentEmail";
                var appointments = db.Query<AppointmentView>(sqlAppointment, new { StudentEmail = Email.Text });

                View View3 = (View)MultiView1.FindControl("View3");
                Repeater AppointmentList = (Repeater)View3.FindControl("AppointmentList");

                if (View3 != null)
                {
                    System.Web.UI.WebControls.Label confirmed = (System.Web.UI.WebControls.Label)View3.FindControl("Label1");
                    if (confirmed != null)
                    {
                        confirmed.Text = $"Upcomming appointments for {students.FirstName} {students.LastName}:";
                    }
                }

                AppointmentList.DataSource = appointments;
                AppointmentList.DataBind();

                MultiView1.ActiveViewIndex = 3;
            }
        }

        protected void BackToBooking(object sender, EventArgs e)
        {
            Email.Text = "";
            FName.Text = "";
            LName.Text = "";
            TimeDDL.SelectedValue = "0";
            Date.SelectedDate = DateTime.MinValue;
            TutorDDL.SelectedValue = "0";
            CourseDDL.SelectedValue = "0";
            TimeDDL.EnableViewState = false;
            MultiView1.ActiveViewIndex = 0;
            TutorDDL.Items.Clear();
            TutorDDL.Items.Add(new ListItem("Select", "0"));
            LoadTutors();
            CourseDDL.Items.Clear();
            CourseDDL.Items.Add(new ListItem("Select", "0"));
            LoadCourses();
        }
    }
}