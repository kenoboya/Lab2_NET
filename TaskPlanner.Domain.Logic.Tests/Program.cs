using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Domain.Logic;
using Moq;
using Xunit;

namespace TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_SortsByPriority_Correctly()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", Priority = Priority.l, IsCompleted = false },
                new WorkItem { Title = "Task 2", Priority = Priority.h, IsCompleted = false },
                new WorkItem { Title = "Task 3", Priority = Priority.m, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);

            Assert.Equal(Priority.h, result[0].Priority);
            Assert.Equal(Priority.m, result[1].Priority);
            Assert.Equal(Priority.l, result[2].Priority);
        }

        [Fact]
        public void CreatePlan_ExcludesCompletedTasks()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", Priority = Priority.l, IsCompleted = true },
                new WorkItem { Title = "Task 2", Priority = Priority.h, IsCompleted = false },
                new WorkItem { Title = "Task 3", Priority = Priority.m, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);

            Assert.Equal(2, result.Length);
            Assert.DoesNotContain(result, item => item.IsCompleted);
        }

        [Fact]
        public void CreatePlan_SortsByDate_Correctly()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", DueDate = DateTime.Now.AddDays(3), IsCompleted = false },
                new WorkItem { Title = "Task 2", DueDate = DateTime.Now.AddDays(1), IsCompleted = false },
                new WorkItem { Title = "Task 3", DueDate = DateTime.Now.AddDays(2), IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Date);

            Assert.Equal("Task 2", result[0].Title);
            Assert.Equal("Task 3", result[1].Title);
            Assert.Equal("Task 1", result[2].Title);
        }

        [Fact]
        public void CreatePlan_ReturnsEmpty_WhenNoTasks()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>();

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);

            Assert.Empty(result);
        }

        [Fact]
        public void CreatePlan_HighestPriorityFirst()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", Priority = Priority.m, IsCompleted = false },
                new WorkItem { Title = "Task 2", Priority = Priority.h, IsCompleted = false },
                new WorkItem { Title = "Task 3", Priority = Priority.l, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);

            Assert.Equal("Task 2", result[0].Title);
        }

        [Fact]
        public void CreatePlan_SortsByTitle_Correctly()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Beta", Priority = Priority.l, IsCompleted = false },
                new WorkItem { Title = "Alpha", Priority = Priority.h, IsCompleted = false },
                new WorkItem { Title = "Gamma", Priority = Priority.m, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Alphabet);

            Assert.Equal("Alpha", result[0].Title);
            Assert.Equal("Beta", result[1].Title);
            Assert.Equal("Gamma", result[2].Title);
        }

        [Fact]
        public void CreatePlan_SortsByComplexity_Correctly()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", Complexity = Complexity.month, IsCompleted = false },
                new WorkItem { Title = "Task 2", Complexity = Complexity.day, IsCompleted = false },
                new WorkItem { Title = "Task 3", Complexity = Complexity.years, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), (x, y) => x.Complexity.CompareTo(y.Complexity));

            Assert.Equal(Complexity.day, result[0].Complexity);
            Assert.Equal(Complexity.month, result[1].Complexity);
            Assert.Equal(Complexity.years, result[2].Complexity);
        }

        [Fact]
        public void CreatePlan_ShouldFail_WhenInvalidLogic()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            var workItems = new List<WorkItem>
            {
                new WorkItem { Title = "Task 1", Priority = Priority.l, IsCompleted = false },
                new WorkItem { Title = "Task 2", Priority = Priority.h, IsCompleted = false }
            };

            mockRepository.Setup(r => r.GetAll()).Returns(workItems.ToArray());
            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var result = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);

            Assert.Equal(Priority.l, result[0].Priority);
        }
    }
}
