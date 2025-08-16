using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Course
{
    public string Code { get; set; }
    public string Name { get; set; }
    public List<string> Prerequisites { get; set; }
    public string TimeSlot { get; set; }

    public Course(string code, string name, List<string> prerequisites, string timeSlot)
    {
        Code = code.ToUpper();
        Name = name;
        Prerequisites = prerequisites.Select(p => p.ToUpper()).ToList();
        TimeSlot = timeSlot;
    }

    public bool CanEnroll(List<string> completedCourses, out string reason)
    {
        foreach (var prereq in Prerequisites)
        {
            if (!completedCourses.Contains(prereq))
            {
                reason = $"Missing prerequisite: {prereq}";
                return false;
            }
        }
        reason = "";
        return true;
    }

    public bool HasConflict(List<Course> alreadyApproved, out string reason)
    {
        foreach (var course in alreadyApproved)
        {
            if (course.TimeSlot.Equals(this.TimeSlot, StringComparison.OrdinalIgnoreCase))
            {
                reason = $"Schedule conflict with {course.Code}";
                return true;
            }
        }
        reason = "";
        return false;
    }
}


namespace _9_acitivity_Course_Planner_with_Prerequisites
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var catalog = new List<Course>
        {
            new Course("CS101", "Intro to CS", new List<string>(), "Mon 9-11"),
            new Course("CS102", "Data Structures", new List<string>{"CS101"}, "Tue 10-12"),
            new Course("CS103", "Algorithms", new List<string>{"CS102"}, "Mon 9-11"),
            new Course("MATH101", "Calculus I", new List<string>(), "Wed 8-10"),
            new Course("MATH102", "Calculus II", new List<string>{"MATH101"}, "Thu 8-10")
        };

            var completedCourses = new List<string> { "CS101", "MATH101" }
                .Select(c => c.ToUpper()).ToList();
            var requestedCourses = new List<string> { "CS102", "CS103", "MATH102" }
                .Select(c => c.ToUpper()).ToList();

            var approved = new List<Course>();
            var rejected = new Dictionary<string, string>();

            foreach (var request in requestedCourses)
            {
                var course = catalog.FirstOrDefault(c => c.Code == request);

                if (course == null)
                {
                    rejected[request] = "Course not found in catalog";
                    continue;
                }

                if (!course.CanEnroll(completedCourses, out string prereqReason))
                {
                    rejected[request] = prereqReason;
                    continue;
                }

                if (course.HasConflict(approved, out string conflictReason))
                {
                    rejected[request] = conflictReason;
                    continue;
                }

                approved.Add(course);
            }

            Console.WriteLine("Approved Courses:");
            foreach (var c in approved)
            {
                Console.WriteLine($"{c.Code} - {c.Name}");
            }

            Console.WriteLine("\nRejected Courses:");
            foreach (var kvp in rejected)
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }
    }
}




