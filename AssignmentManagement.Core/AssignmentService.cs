using System;
using System.Collections.Generic;
using System.Linq;

namespace AssignmentManagement.Core
{
    public class AssignmentService : IAssignmentService
    {
        private readonly List<Assignment> _assignments = new();
        private readonly IAssignmentFormatter _formatter;
        private readonly IAppLogger _logger;

        public AssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
        {
            _formatter = formatter;
            _logger = logger;
        }

        public virtual bool AddAssignment(Assignment assignment)
        {
            try
            {
                _assignments.Add(assignment);
                _logger.Log($"Added Assignment [{assignment.Id}]: {assignment.Title}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.Log($"Error adding assignment: {ex.Message}");
                return false;
            }
        }

        public bool DeleteAssignment(string title)
        {
            var toRemove = _assignments.FirstOrDefault(a => a.Title == title);
            if (toRemove != null)
            {
                _assignments.Remove(toRemove);
                _logger.Log($"Deleted Assignment [{toRemove.Id}]: {toRemove.Title}");
                return true;
            }
            return false;
        }

        public List<Assignment> ListAll() => _assignments;

        public List<Assignment> ListIncomplete() => _assignments.Where(a => !a.IsCompleted).ToList();

        public List<string> ListFormatted() => _assignments.Select(a => _formatter.Format(a)).ToList();

        public Assignment FindByTitle(string title) => _assignments.FirstOrDefault(a => a.Title == title);

        public bool UpdateAssignment(string title, string newTitle, string newDescription)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.Update(newTitle, newDescription);
                return true;
            }
            return false;
        }

        public bool MarkComplete(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
                return true;
            }
            return false;
        }

        public Assignment FindAssignmentByTitle(string title)
        {
            return _assignments.FirstOrDefault(a => a.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
        }

        public bool MarkAssignmentComplete(string title)
        {
            var assignment = FindAssignmentByTitle(title);
            if (assignment != null)
            {
                assignment.MarkComplete();
                return true;
            }
            return false;
        }

        // BUG-2025-344: No logging when overdue status is checked
        public bool CheckIfOverdue(string title)
        {
            var assignment = FindByTitle(title);
            if (assignment != null)
            {
                var isOverdue = assignment.IsOverdue();
                _logger.Log($"Checked if assignment '{assignment.Title}' is overdue: {isOverdue}");
                return isOverdue;
            }

            _logger.Log($"Attempted overdue check on non-existent assignment: {title}");
            return false;
        }


    }
}