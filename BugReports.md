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
The `Assignment` constructor does not assign the notes parameter to the `Notes` property.

**Fix:**  
Assign the `notes` parameter correctly inside the constructor.

**Test Added:**  
A test will be added to confirm proper assignment of the `Notes` field.

---

### BUG-2025-349: IsOverdue Logic Incorrect  
**Reported by:** API Developer  
**Date:** 2025-06-02  

**Steps to Reproduce:**  
- Create an incomplete assignment with no due date — it incorrectly shows as overdue  
- Create a completed assignment with a past due date — it still shows as overdue  

**Root Cause:**  
The `IsOverdue()` method does not check whether a due date is provided or if the assignment is completed.

**Fix:**  
Update the logic to ensure the method returns `true` only when:
- A due date exists  
- The assignment is not completed  
- The due date is in the past

**Tests Added:**  
Tests will be added to verify the fixed behavior for each case.
