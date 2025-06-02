
using AssignmentManagement.Core;
using Moq;
using Xunit;

namespace AssignmentManagement.Tests
{
    public class AssignmentServiceTests
    {
        [Fact]
        public void ListIncomplete_ReturnsOnlyIncompleteAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            var a2 = new Assignment("Title 2", "Desc 2", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 2");
            a2.MarkComplete();

            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();

            Assert.Single(result);
            Assert.Contains(a1, result);
            Assert.DoesNotContain(a2, result);
        }

        [Fact]
        public void ListIncomplete_ReturnsEmptyList_WhenNoAssignments()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var result = service.ListIncomplete();
            Assert.Empty(result);
        }

        [Fact]
        public void ListIncomplete_ReturnsAll_WhenAllAreIncomplete()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a1 = new Assignment("Title 1", "Desc 1", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            var a2 = new Assignment("Title 2", "Desc 2", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 2");

            service.AddAssignment(a1);
            service.AddAssignment(a2);

            var result = service.ListIncomplete();
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public void AddAssignment_StoresAssignmentCorrectly()
        {
            var service = new AssignmentService(new AssignmentFormatter(), new ConsoleAppLogger());
            var a = new Assignment("Title", "Desc", DateTime.Now.AddDays(3), AssignmentPriority.Medium, "Notes 1");
            service.AddAssignment(a);
            Assert.Contains(a, service.ListAll());
        }

        // BUG-2025-344: No logging when overdue status is checked.
        // This test exposes the missing CheckIfOverdue method and required log output.
        [Fact]
        public void CheckIfOverdue_ShouldLogResult_WhenAssignmentExists_Unimplemented()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var formatter = new AssignmentFormatter();
            var service = new AssignmentService(formatter, mockLogger.Object);

            var assignment = new Assignment(
                "Lecture: SOLID Principles",
                "Watch and summarize SOLID principles lecture",
                DateTime.Now.AddDays(-3), // Overdue
                AssignmentPriority.High,
                "Due before Week 8 quiz");

            service.AddAssignment(assignment);

            //Act(does not compile until method is created)
            var isOverdue = service.CheckIfOverdue("Lecture: SOLID Principles");

            //Assert
            Assert.True(isOverdue); // Should be overdue
            mockLogger.Verify(log => log.Log(It.Is<string>(msg =>
                msg.Contains("Checked if assignment") &&
                msg.Contains("Lecture: SOLID Principles") &&
                msg.Contains("True")
            )), Times.Once);
        }

        // Edge Case: AddAssignment should log error if exception occurs
        [Fact]
        public void AddAssignment_ShouldLogError_WhenExceptionThrown()
        {
            // Arrange
            var mockLogger = new Mock<IAppLogger>();
            var formatter = new AssignmentFormatter();

            // Create a derived test class that throws when adding
            var service = new ExceptionThrowingAssignmentService(formatter, mockLogger.Object);

            var assignment = new Assignment(
                "Quiz: Week 8",
                "Complete the weekly quiz",
                DateTime.Now.AddDays(1),
                AssignmentPriority.Medium,
                "Due Sunday night");

            // Act
            var result = service.AddAssignment(assignment);

            // Assert
            Assert.False(result);
            mockLogger.Verify(log => log.Log(It.Is<string>(msg =>
                msg.Contains("Error adding assignment")
            )), Times.Once);
        }

        // Helper: Throws exception to simulate Add failure
        public class ExceptionThrowingAssignmentService : AssignmentService
        {
            private readonly IAppLogger _testLogger;
            public ExceptionThrowingAssignmentService(IAssignmentFormatter formatter, IAppLogger logger)
                : base(formatter, logger) {
                _testLogger = logger;
            }

            public override bool AddAssignment(Assignment assignment)
            {
                try
                {
                    throw new InvalidOperationException("Simulated failure");
                }
                catch (Exception ex)
                {
                    _testLogger.Log($"Error adding assignment: {ex.Message}");
                    return false;
                }
            }
        }


    }
}
