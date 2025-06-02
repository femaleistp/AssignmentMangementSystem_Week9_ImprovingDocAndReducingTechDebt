# Bug Reports

---

### BUG-2025-341: Notes Field Missing  
**Reported by:** QA Intern  
**Date:** 2025-06-02  

**Steps to Reproduce:**  
- Use POST `/api/assignment` to create an assignment with `"Notes": "Final project guidelines"`  
- Use GET `/api/assignment` to retrieve the assignment list  
- The Notes field is missing or empty in the response  

**Root Cause:**  
The `Assignment` constructor previously failed to assign the `notes` parameter to the `Notes` property.

**Fix:**  
This bug was resolved in a prior week. No code or test changes were made in Week 9.

**Test Added:**  
A regression test (`Constructor_WithNotes_ShouldAssignNotes`) was added in the earlier week alongside the fix to ensure the Notes field is correctly assigned.

---

### BUG-2025-349: IsOverdue Logic Incorrect  
**Reported by:** API Developer  
**Date:** 2025-06-02  

**Steps to Reproduce:**  
- Create an incomplete assignment with no due date — it incorrectly shows as overdue  
- Create a completed assignment with a past due date — it still shows as overdue  

**Root Cause:**  
The `IsOverdue()` method did not check for a null due date or completion status.

**Fix:**  
The logic was updated to return `true` only when:  
- A due date exists  
- The assignment is not completed  
- The due date is in the past

**Test Added:**  
The test `IsOverdue_ShouldReturnFalse_WhenAssignmentIsCompleted` confirms that completed assignments are not flagged as overdue. Additional coverage ensures the method handles null due dates correctly.
