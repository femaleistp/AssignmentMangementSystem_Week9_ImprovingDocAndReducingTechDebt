
using System;

namespace AssignmentManagement.Core
{
    public class AssignmentFormatter : IAssignmentFormatter
    {
        public string Format(Assignment assignment)
        {
            return $"[{assignment.Id}] {assignment.Title} - {(assignment.IsCompleted ? "Completed" : "Incomplete")}";
        }
    }
}
