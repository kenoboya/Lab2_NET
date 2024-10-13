using System;

namespace Domain.Models
{
    public enum Priority
    {
        n = 0,
        l = 1,
        m = 2,
        h = 3,
        u = 4,
    }

    public enum Complexity
    {
        none = 0,
        day = 1,
        month = 2,
        years = 3,
    }

    public class WorkItem
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        public WorkItem Clone()
        {
            return (WorkItem)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{Title}: date {DueDate.ToString("dd/MM/yyyy")}, {Priority.ToString().ToLower()} priority";
        }
    }


    public interface IWorkItemsRepository
    {
        Guid Add(WorkItem workItem);
        WorkItem[] GetAll();
        void SaveChanges();
    }

    internal class Program
    {
        static void Main(string[] args)
        {

        }
    }
}
