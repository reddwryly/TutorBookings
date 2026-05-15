DROP VIEW IF EXISTS AppointmentView;

CREATE VIEW AppointmentView AS 
SELECT 
	a.StudentEmail,
	t.FirstName,
	a.Date, 
	a.Time, 
	a.CourseCode, 
	c.Name AS CourseName
FROM Appointment a 
INNER JOIN Tutor t ON a.TutorId = t.TutorId 
INNER JOIN Course c ON a.CourseCode = c.CourseCode