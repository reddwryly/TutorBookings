using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TutorBookings.Database_SQL
{
    public class Models
    {
        public class Tutor
        {
            public string TutorId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Bio { get; set; }
            public string Picture { get; set; }
        }

        public class Course
        {
            public string CourseCode { get; set; }
            public string Name { get; set; }
        }

        public class TutorCourse
        {
            public string TutorId { get; set; }
            public string CourseCode { get; set; }
        }

        public class TutorAvailability
        {
            public string TutorId { get; set; }
            public string Weekday { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
        }

        public class Student
        {
            public string StudentEmail { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Appointment
        {
            public string TutorId { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string StudentEmail { get; set; }
            public string CourseCode { get; set; }
        }
    }
}