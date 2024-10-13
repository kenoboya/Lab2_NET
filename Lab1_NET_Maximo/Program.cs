using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Logic
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    List<WorkItem> workItems = new List<WorkItem>();
        //    IWorkItemsRepository repository = new MockWorkItemsRepository(); 
        //    SimpleTaskPlanner taskPlanner = new SimpleTaskPlanner(repository);

        //    workItems.Add(new WorkItem
        //    {
        //        Title = "Title1",
        //        DueDate = DateTime.ParseExact("23/10/2004", "dd/MM/yyyy", null),
        //        Priority = Enum.Parse<Priority>("l", true)
        //    });

        //    workItems.Add(new WorkItem
        //    {
        //        Title = "Title31221",
        //        DueDate = DateTime.ParseExact("12/11/2002", "dd/MM/yyyy", null),
        //        Priority = Enum.Parse<Priority>("m", true)
        //    });

        //    workItems.Add(new WorkItem
        //    {
        //        Title = "Title31221",
        //        DueDate = DateTime.ParseExact("13/03/2000", "dd/MM/yyyy", null),
        //        Priority = Enum.Parse<Priority>("u", true)
        //    });

        //    while (true)
        //    {
        //        Console.WriteLine("Enter (y) to continue adding or (n) to finish ");
        //        string input = Console.ReadLine();

        //        if (input == "n")
        //        {
        //            break;
        //        }

        //        WorkItem workItem = new WorkItem();

        //        Console.WriteLine("Enter title ");
        //        workItem.Title = Console.ReadLine();

        //        Console.WriteLine("Enter the date (dd/MM/yyyy) ");
        //        workItem.DueDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", null);

        //        Console.WriteLine("Enter the priority (n-none, l-low, m-medium, h-high, u-urgent) ");
        //        workItem.Priority = Enum.Parse<Priority>(Console.ReadLine(), true);

        //        workItems.Add(workItem);
        //    }

        //    Console.WriteLine("Enter 1 to sort alphabetically, 2 by date, or 3 by priority ");
        //    int sortOption = int.Parse(Console.ReadLine());

        //    WorkItem[] sortedItems;
        //    switch (sortOption)
        //    {
        //        case 1:
        //            sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Alphabet);
        //            break;
        //        case 2:
        //            sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Date);
        //            break;
        //        case 3:
        //            sortedItems = taskPlanner.CreatePlan(workItems.ToArray(), taskPlanner.Priority);
        //            break;
        //        default:
        //            return;
        //    }

        //    Console.WriteLine("Sorting results:");
        //    foreach (var item in sortedItems)
        //    {
        //        Console.WriteLine(item.ToString());
        //    }
        //}
    }

    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository;
        }

        public WorkItem[] CreatePlan(WorkItem[] items, Comparison<WorkItem> comparison)
        {
            List<WorkItem> workItems = items.ToList();
            workItems.Sort(comparison);
            return workItems.ToArray();
        }

        public int Date(WorkItem oneItem, WorkItem twoItem)
        {
            return oneItem.DueDate.CompareTo(twoItem.DueDate);
        }

        public int Priority(WorkItem oneItem, WorkItem twoItem)
        {
            return twoItem.Priority.CompareTo(oneItem.Priority);
        }

        public int Alphabet(WorkItem oneItem, WorkItem twoItem)
        {
            return string.Compare(oneItem.Title, twoItem.Title, StringComparison.OrdinalIgnoreCase);
        }
    }

    // Пример фиктивного репозитория
    public class MockWorkItemsRepository : IWorkItemsRepository
    {
        private readonly List<WorkItem> _workItems = new List<WorkItem>();
        private int _idCounter = 0; 

        public Guid Add(WorkItem workItem)
        {
            workItem.Id = Guid.NewGuid(); 
            _workItems.Add(workItem);
            return workItem.Id;
        }

        public WorkItem[] GetAll()
        {
            return _workItems.ToArray();
        }

        public void SaveChanges()
        {
        }
    }
}
