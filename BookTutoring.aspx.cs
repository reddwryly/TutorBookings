using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TutorBookings.Database_SQL;
using static TutorBookings.Database_SQL.Models;
using Dapper;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.Ajax.Utilities;

namespace TutorBookings
{
    public partial class BookTutoring : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadOptions();
            }
        }

        protected void LoadOptions()
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sqlCourses = "SELECT CourseCode, Name FROM Course";
                var Course = db.Query<Course>(sqlCourses).ToList();

                foreach(var c in Course)
                {
                    CourseDDL.Items.Add(new ListItem($"{c.CourseCode} - {c.Name}", c.CourseCode));
                }

                var sqlTutors = "SELECT FirstName, LastName, TutorID FROM Tutor";
                var Tutor = db.Query<Tutor>(sqlTutors).ToList();

                foreach (var t in Tutor)
                {
                    TutorDDL.Items.Add(new ListItem($"{t.FirstName} {t.LastName}", t.TutorId));
                }
            }
        }

        protected void TutorSelected(object sender, EventArgs e)
        {

            if (TutorDDL.SelectedValue == "0")
            {
                //to let the user "change their mind"
                //if the value still makes sence for the new selection set value to the old value before clear
            }

            CourseDDL.Items.Clear();
            CourseDDL.Items.Add(new ListItem("select", "0"));
            using (var db = DatabaseHelper.Connect()) { 
                var sqlJoinTutorCourseCourse = $"SELECT tc.CourseCode, c.Name " +
                                                $"FROM TutorCourse as tc INNER JOIN Course c ON tc.CourseCode = c.CourseCode " +
                                                $"WHERE TutorId= '{TutorDDL.SelectedValue}'";
                var TutorCourseCourseTutorCourse = db.Query<Course>(sqlJoinTutorCourseCourse).ToList();

                foreach (var tc in TutorCourseCourseTutorCourse)
                {
                    CourseDDL.Items.Add(new ListItem($"{tc.CourseCode} - {tc.Name}", tc.CourseCode));
                }

            }

            //issues with duel dependent dropdowns still 
            //also if the user empties courses i want all unlselected options to reload with all options (may need to seperate LoadOptionsO into different functions for each)

            if (Date.SelectedDate == DateTime.MinValue)
            {
                //research how to choose dates that apear in the calandar control 
            }

            //WIP
            //if (TimeDDL.SelectedValue == "0")
            //{
            //    TimeDDL.Items.Clear();
            //    TimeDDL.Items.Add(new ListItem("select", "0"));
            //    using (var db = DatabaseHelper.Connect())
            //    {
            //        var sqlTutorAvailability = $"SELECT StartTime, EndTime FROM TutorAvailability WHERE TutorId = '{TutorDDL.SelectedValue}'";
            //        var TutorAvailability = db.Query<TutorAvailability>(sqlTutorAvailability).ToList();

            //        foreach (var tc in TutorCourseCourseTutorCourse)
            //        {
            //            CourseDDL.Items.Add(new ListItem($"{tc.CourseCode} - {tc.Name}", tc.CourseCode));
            //        }
            //    }
            //}
        }

        protected void CourseSelected(object sender, EventArgs e)
        {
 

            if (TutorDDL.SelectedValue == "0")
            {
                TutorDDL.Items.Clear();
                TutorDDL.Items.Add(new ListItem("select", "0"));
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
                }
            }

            if (Date.SelectedDate == DateTime.MinValue)
            {
                
            }

            if (TimeDDL.SelectedValue == "0")
            {

            }
        }

        protected void DateSelected(object sender, EventArgs e)
        {
            if (TutorDDL.SelectedValue == "0")
            {

            }

            if (CourseDDL.SelectedValue == "0")
            {

            }

            if (TimeDDL.SelectedValue == "0")
            {
                
            }
        }

        protected void TimeSelected()
        {
            if (TutorDDL.SelectedValue == "0")
            {

            }

            if (CourseDDL.SelectedValue == "0")
            {

            }

            if (Date.SelectedDate == DateTime.MinValue)
            {
                
            }
        }

        protected void SubmitButton(object sender, EventArgs e)
        {
            //insert statement
            //clear form 
        }
    }
}