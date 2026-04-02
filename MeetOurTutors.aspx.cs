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

        //not yet working
        protected void LoadTutors()
        {
            using (var db = DatabaseHelper.Connect())
            {
                var sql = "SELECT Picture, FirstName, LastName, Bio from Tutor";
                var Tutor = db.Query<Tutor>(sql).ToList();

                Tutors.DataSource = Tutor;
                Tutors.DataBind();
            }
        }
    }
}