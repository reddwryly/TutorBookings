using Dapper;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TutorBookings.Database_SQL;
using static TutorBookings.Database_SQL.Models;

//Database connection not working yet** 

namespace TutorBookings
{
    public partial class About : Page
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
           
                var courses = db.Query<Course>("SELECT CourseCode, Name FROM Course").ToList();

                rptCourse.DataSource = courses;
                rptCourse.DataBind();
            }
        }
    }
}