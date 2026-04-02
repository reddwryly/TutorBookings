using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TutorBookings
{
    public partial class adminPage : System.Web.UI.Page
    {
        
        public class Appointment
        {
            public int Id { get; set; }
            public string StudentName { get; set; }
            public string TutorName { get; set; }
            public string CourseCode { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string StudentEmail { get; set; }
        }

        
        private static List<Appointment> appointments = new List<Appointment>
        {
            new Appointment { Id = 1, StudentName = "Moon", TutorName = "Webs", CourseCode = "PSY2100", Date = "3/20", Time = "2PM" },
            new Appointment { Id = 2, StudentName = "Winter", TutorName = "Starflight", Date = "3/21", Time = "10AM" },
            new Appointment { Id = 3, StudentName = "Peril", TutorName = "Clay", Date = "3/22", Time = "1PM", StudentEmail = "peril@jma.edu" }
        };

        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadAppointments();
            }
        }

        
        private void LoadAppointments()
        {
            AppointmentsGrid.DataSource = appointments;
            AppointmentsGrid.DataBind();
        }

        
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            var button = (System.Web.UI.WebControls.Button)sender;
            int id = int.Parse(button.CommandArgument);

            
            var appt = appointments.FirstOrDefault(a => a.Id == id);
            if (appt != null)
            {
                appointments.Remove(appt);
            }

            
            LoadAppointments();
        }
    }
}