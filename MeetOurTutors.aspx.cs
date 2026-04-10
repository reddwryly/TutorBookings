using Dapper;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TutorBookings.Database_SQL;
using static TutorBookings.Database_SQL.Models;

namespace TutorBookings
{
    public partial class MeetOurTutors : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTutors();
            }
        }

        //explain this block VV

        protected void LoadTutors()
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sql = "SELECT t.TutorId, t.Picture, t.FirstName, t.LastName, t.Bio, tc.CourseCode, c.Name " +
                                "FROM Tutor as t " +
                                "INNER JOIN TutorCourse as tc ON t.TutorID = tc.TutorId " +
                                "INNER JOIN Course as c ON c.CourseCode = tc.CourseCode " +
                                "WHERE t.TutorId IS NOT NULL " +
                                "ORDER BY t.LastName, FirstName";

                var TutorList = new Dictionary<string, Models.Tutor>(); //Saves Tutors objects with their dictionary of courses

                //multi mapping
                var Tutor = db.Query<Models.Tutor, Models.Course, Models.Tutor>( //<Maps Tutor fields, maps course fields, return type 
                    sql,
                    (tutor, course) => //for each row in the result
                    {
                        if (!TutorList.TryGetValue(tutor.TutorId, out var currentTutor)) //checks if tutor is in the dictionary
                        {
                            currentTutor = tutor;
                            TutorList.Add(currentTutor.TutorId, currentTutor); //if not adds them 
                        }

                        if (course != null) //checks if tutor has courses
                            currentTutor.Courses.Add(course); //adds the couse if they do 

                        return currentTutor;
                    },
                    splitOn: "CourseCode").ToList(); //seperates the tutor field results from the course field results

                Tutors.DataSource = Tutors.DataSource = TutorList.Values.ToList(); //converts unique tutors into a list
                Tutors.DataBind(); //gives the data to the asp:Repeater
            }
        }

        protected void getTutorCourse(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var Tutor = (Models.Tutor)e.Item.DataItem; //the current tutor is set to the data already loaded
                var coursesRepeater = (Repeater)e.Item.FindControl("Courses"); //collects all the courses for the tutor
                coursesRepeater.DataSource = Tutor.Courses;
                coursesRepeater.DataBind(); //renders the courses to the course asp:Repeater
            }
        }
    }
}