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
    public partial class AboutTutoring : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses() {
            using (var db = DatabaseHelper.Connect()) {
                var sql = "SELECT CourseCode, Name FROM Course";
                var Course = db.Query<Course>(sql).ToList();

                CoursesList.DataSource = Course;
                CoursesList.DataBind();
            }
        }
    }
}