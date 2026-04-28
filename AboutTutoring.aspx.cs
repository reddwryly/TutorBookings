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
            if (!IsPostBack) //Makes sure it only runs when the page loads for the first time
            {
                LoadCourses();
            }
        }

        private void LoadCourses() {
            using (var db = DatabaseHelper.Connect()) { //uses the connection from the DatabaseHelper file in the Database_SQL folder
                var sql = "SELECT CourseCode, Name FROM Course ORDER BY CourseCode"; //SQLite Query 
                var Course = db.Query<Course>(sql).ToList(); //Results of Query

                CoursesList.DataSource = Course; //CourseList is the ID for my ASP.NET server control in the html this sets the data
                CoursesList.DataBind(); //Lines 30 and 31 put the data into the server control so it can display
            }
        }
    }
}