using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechAcademy.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
