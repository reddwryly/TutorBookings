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

                var TutorList = new Dictionary<string, Models.Tutor>();
                var Tutor = db.Query<Models.Tutor, Models.Course, Models.Tutor>(
                    sql,
                    (tutor, course) =>
                    {
                        if (!TutorList.TryGetValue(tutor.TutorId, out var currentTutor))
                        {
                            currentTutor = tutor;
                            TutorList.Add(currentTutor.TutorId, currentTutor);
                        }

                        if (course != null)
                            currentTutor.Courses.Add(course);

                        return currentTutor;
                    },
                    splitOn: "CourseCode").ToList();

                Tutors.DataSource = Tutors.DataSource = TutorList.Values.ToList();
                ;
                Tutors.DataBind();
            }
        }

        protected void getTutorCourse(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var Tutor = (Models.Tutor)e.Item.DataItem;
                var coursesRepeater = (Repeater)e.Item.FindControl("Courses");
                coursesRepeater.DataSource = Tutor.Courses;
                coursesRepeater.DataBind();
            }
        }
    }
}