using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TechAcademy.Models;

namespace TechAcademy.DAL
{
    public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>
    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student>
            {
                new Student{FirstName = "Pierson", LastName = "Han", EnrollmentDate = DateTime.Parse("2019-01-16")},
                new Student{FirstName = "Jason", LastName = "Squallante", EnrollmentDate = DateTime.Parse("2019-06-19")},
                new Student{FirstName = "Jesse", LastName = "Johnson", EnrollmentDate = DateTime.Parse("2014-09-26")}
            };

            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges();
            var courses = new List<Course>
            {
                new Course{CourseID=1234, Title="C Sharp", Credits=10,},
                new Course{CourseID=1235, Title="Python", Credits=20,},
                new Course{CourseID=1236, Title="Github", Credits=10,},
                new Course{CourseID=1237, Title="Javascript", Credits=20,},
                new Course{CourseID=1238, Title="CSS", Credits=5,},
                new Course{CourseID=1239, Title="HTML", Credits=10,},
            };
            courses.ForEach(s => context.Courses.Add(s));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment{StudentID=1, CourseID=1234, Grade=Grade.A},
                new Enrollment{StudentID=1, CourseID=1235, Grade=Grade.B},
                new Enrollment{StudentID=1, CourseID=1238, Grade=Grade.C},
                new Enrollment{StudentID=1, CourseID=1237, Grade=Grade.C},
                new Enrollment{StudentID=2, CourseID=1234, Grade=Grade.F},
                new Enrollment{StudentID=2, CourseID=1236, Grade=Grade.B},
                new Enrollment{StudentID=2, CourseID=1239, Grade=Grade.F},
                new Enrollment{StudentID=3, CourseID=1235, Grade=Grade.C},
                new Enrollment{StudentID=3, CourseID=1234, Grade=Grade.D},
                new Enrollment{StudentID=3, CourseID=1236, Grade=Grade.A}
            };
            enrollments.ForEach(e => context.Enrollments.Add(e));
            context.SaveChanges();
        }
    }
}