/*Tutor*/

/*TutorID must be unique and 10 digits*/
/*Bio can be left out*/
/*Leave PicturePath empty unless you send me a coorisponding named file of the same name (jpg, png, or jpeg)*/
/*Syntax (order matters): 
INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture) **remove the columns you dont add values for**
INSERT INTO Tutor VALUES("TutorID", "FirstName", "LastName", "Bio", "PicturePath.png");
*/
INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture)
VALUES("1234567890", "Steve", "Jones", "I am an Engineering major and have been tutoring for two years. I love to play Minecraft, and I have two cats, Solar and Luna. In my free time, I’m a poet and pianist. ", "SteveJones.jpg");

INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture)
VALUES("0987654321", "Jane", "Doe", "I am studying Biology with a minor in English, and I am the head tutor! I’m who you come to with any scheduling or tutoring-related questions! I’ve been a tutor for almost three years, and I aspire to work in academia. ", "JaneDoe.jpg");

INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture)
VALUES("0000000002", "John", "Doe", "I study Physics with a minor in Computer Science. I aspire to work as a Quant Software Engineer. I love animals and volunteer at the zoo in my spare time. ", "JohnDoe.jpg");

INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture)
VALUES("0000000003", "Sally", "Sue", "I’m a Social Science major and aspiring lawyer. I’m on the tennis team, and I play singles. I have three dogs, and they love tennis too. ", "SallySue.jpg");

INSERT INTO Tutor (TutorID, FirstName, LastName, Bio, Picture)
VALUES("0000000004", "June", "Rose", "I study mathematics and aspire to be a math professor! In my free time, I’m a painter, and I love incorporating nature into my work.", "JuneRose.jpg");

/*Course*/
/*CourseCode must be unique and less than 8 characters*/
/*Syntax (order matters): INSERT INTO Course VALUES("CourseCode", "CourseName");*/
INSERT INTO Course VALUES("EGR2000", "Engineering Communication");
INSERT INTO Course VALUES("PH1300", "Physics Lecture 1");
INSERT INTO Course VALUES("PH1310", "Physics Laboratory 1");
INSERT INTO Course VALUES("IS2350", "Web Development");
INSERT INTO Course VALUES("CS3810", "Intro to AI");
INSERT INTO Course VALUES("MA1210", "Calculus 2");
INSERT INTO Course VALUES("MA2150", "Linear Algebra");
INSERT INTO Course VALUES("SS1110", "United States Government");
INSERT INTO Course VALUES("IME2110", "Six Sigma");
INSERT INTO Course VALUES("MA3200", "Graph Theory");
INSERT INTO Course VALUES("MA1200", "Calculus 1");
INSERT INTO Course VALUES("PH2300", "Physics Lecture 2");
INSERT INTO Course VALUES("PH2310", "Physics Laboratory 2");
INSERT INTO Course VALUES("ENG1252", "Argumentative writing");
INSERT INTO Course VALUES("BIO1110", "Anatomy & Physiology");
INSERT INTO Course VALUES("BIO1210", "Human Disease");
INSERT INTO Course VALUES("BIO1000", "Introductory Biology");
INSERT INTO Course VALUES("HUM2000", "Intro to Humanities");
INSERT INTO Course VALUES("ENG1272", "Analytical Writing");
INSERT INTO Course VALUES("CJ1100", "Intro to Criminal Justice");
INSERT INTO Course VALUES("LS1100", "Intro to Law Studies");
INSERT INTO Course VALUES("SS2800", "Introduction to Sociology");

/*TutorCourse*/
/*TutorID must already exist in the tutor table*/
/*CourseCode must already exist in the course table*/
/*Syntax (order matters): INSERT INTO TutorCourse VALUES("TutorID", "CourseCode");*/
INSERT INTO TutorCourse VALUES("1234567890", "EGR2000");
INSERT INTO TutorCourse VALUES("1234567890", "PH1300");
INSERT INTO TutorCourse VALUES("1234567890", "MA1210");
INSERT INTO TutorCourse VALUES("1234567890", "MA1200");
INSERT INTO TutorCourse VALUES("1234567890", "ENG1272");

INSERT INTO TutorCourse VALUES("0987654321", "EGR2000");
INSERT INTO TutorCourse VALUES("0987654321", "PH1300");
INSERT INTO TutorCourse VALUES("0987654321", "PH1310");
INSERT INTO TutorCourse VALUES("0987654321", "MA1200");
INSERT INTO TutorCourse VALUES("0987654321", "ENG1272");
INSERT INTO TutorCourse VALUES("0987654321", "ENG1252");
INSERT INTO TutorCourse VALUES("0987654321", "HUM2000");
INSERT INTO TutorCourse VALUES("0987654321", "BIO1110");
INSERT INTO TutorCourse VALUES("0987654321", "BIO1210");
INSERT INTO TutorCourse VALUES("0987654321", "BIO1000");

INSERT INTO TutorCourse VALUES("0000000002", "PH1300");
INSERT INTO TutorCourse VALUES("0000000002", "PH1310");
INSERT INTO TutorCourse VALUES("0000000002", "MA1200");
INSERT INTO TutorCourse VALUES("0000000002", "PH2300");
INSERT INTO TutorCourse VALUES("0000000002", "PH2310");
INSERT INTO TutorCourse VALUES("0000000002", "IS2350");
INSERT INTO TutorCourse VALUES("0000000002", "CS3810");

INSERT INTO TutorCourse VALUES("0000000003", "SS1110");
INSERT INTO TutorCourse VALUES("0000000003", "HUM2000");
INSERT INTO TutorCourse VALUES("0000000003", "CJ1100");
INSERT INTO TutorCourse VALUES("0000000003", "LS1100");
INSERT INTO TutorCourse VALUES("0000000003", "SS2800");

INSERT INTO TutorCourse VALUES("0000000004", "MA1210");
INSERT INTO TutorCourse VALUES("0000000004", "MA2150");
INSERT INTO TutorCourse VALUES("0000000004", "IME2110");
INSERT INTO TutorCourse VALUES("0000000004", "MA3200");
INSERT INTO TutorCourse VALUES("0000000004", "MA1200");

/*TutorAvailability*/
/*TutorID must already exist in the tutor table*/
/*time stamps are in HH:MM 24 hour where 00:00 == 12am format (buisness rule: earliest StartTime == 7am and latest EndTime == 8pm)*/
/*Syntax (order matters): INSERT INTO TutorAvailability VALUES("TutorID", "Weekday", "StartTime", "EndTime");*/
INSERT INTO TutorAvailability VALUES("1234567890", "wednesday", "18:00", "20:00");
INSERT INTO TutorAvailability VALUES("1234567890", "thursday", "12:00", "15:00");
INSERT INTO TutorAvailability VALUES("0000000002", "monday", "07:00", "12:00");
INSERT INTO TutorAvailability VALUES("0000000002", "friday", "09:00", "14:00");
INSERT INTO TutorAvailability VALUES("0987654321", "friday", "07:00", "12:00");
INSERT INTO TutorAvailability VALUES("0987654321", "thursday", "11:00", "13:00");
INSERT INTO TutorAvailability VALUES("0000000004", "monday", "08:00", "13:00");
INSERT INTO TutorAvailability VALUES("0000000004", "sunday", "13:00", "18:00");
INSERT INTO TutorAvailability VALUES("0000000003", "sunday", "10:00", "14:00");
INSERT INTO TutorAvailability VALUES("0000000002", "wednesday", "10:00", "14:00");

/*Student*/
/*Student email must be unique and in the format something@example.com (example.com is not a real domain so it is good to use wont send emails by mistake)*/
/*Syntax (order matters): INSERT INTO Student VALUES("StudentEmail", "FirstName", "LastName");*/
INSERT INTO Student VALUES("jjones@example.com", "Josh", "Jones");
INSERT INTO Student VALUES("rxrain@example.com", "Rose", "Rain");
INSERT INTO Student VALUES("bigmoneysteve@example.com", "Steve", "Smith");

/*Appointment*/
/*TutorID must already exist in the tutor table*/
/*StudentEmail must already exist in the student table*/
/*CourseCode must already exist in the Course table and be in compliance with the courses the tutor tutors from the TutorCourse table*/
/*Date is YYYY-MM-DD*/
/*Time is HH:MM*/
/*The date and time combined must be 24 hours in the future of inserting 
 	(this is enforced in the database so I will just fix it later if its not correct at the time I run the script)
 		you can leave them blank or put a date a couple weeks out
 		**MUST BE IN COMPLIANCE WITH THE TUTORS AVAILABILITY FROM THE AVAILABILITY TABLE** */
/*Syntax (order matters): INSERT INTO Appointment VALUES("TutorID", "Date", "Time", "StudentEmail", "CourseCode");*/
INSERT INTO Appointment VALUES("0000000002", "2026-03-09", "07:00", "jjones@example.com", "PH1300");
INSERT INTO Appointment VALUES("0000000002", "2026-03-13", "09:00", "rxrain@example.com", "PH1300");
INSERT INTO Appointment VALUES("1234567890", "2026-03-11", "18:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("1234567890", "2026-04-16", "18:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("0000000003", "2026-05-17", "11:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("0987654321", "2026-05-14", "12:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("0000000004", "2026-05-11", "10:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("0000000002", "2026-05-11", "08:00", "rxrain@example.com", "EGR2000");
INSERT INTO Appointment VALUES("0000000002", "2026-05-15", "09:00", "rxrain@example.com", "EGR2000");


/*TimeOff*/
/*Syntax (order matters): INSERT INTO TimeOff VALUES("TutorID", "Date", "Reason");*/
INSERT INTO TimeOff VALUES("1234567890", "2026-04-30", "Doctors Appointment");
INSERT INTO TimeOff VALUES("0000000002", "2026-04-29", "Interview");
INSERT INTO TimeOff VALUES("0000000002", "2026-05-13", "Dentist");
INSERT INTO TimeOff VALUES("0987654321", "2026-05-14", "Exam Studying");
INSERT INTO TimeOff VALUES("1234567890", "2026-05-21", "Interview");


/*Semester*/
/*Syntax (order matters): INSERT INTO Semester VALUES("ID", "Name", "StartDate", "EndDate", "Active");*/
INSERT INTO Semester VALUES("1", "Spring 2026", "2026-01-12", "2026-05-29", "1");
INSERT INTO Semester VALUES("2", "Fall 2026", "2026-08-17", "2026-12-10", "0");

