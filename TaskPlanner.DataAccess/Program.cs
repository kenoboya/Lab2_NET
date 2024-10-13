using System.Xml;
using Domain.Models;
using System.Text.Json;

namespace TaskPlanner.DataAccess
{
    public class FileWorkItemsRepository : IWorkItemsRepository
    {
        private const string FileName = "C:\\Users\\flash\\source\\repos\\Lab1_NET_Maximo\\work-items.json"; 
        private Dictionary<Guid, WorkItem> _items;

        public FileWorkItemsRepository()
        {
            if (File.Exists(FileName))
            {
                string json = File.ReadAllText(FileName);
                _items = JsonSerializer.Deserialize<List<WorkItem>>(json)
                    ?.ToDictionary(item => item.Id) ?? new Dictionary<Guid, WorkItem>();
            }
            else
            {
                _items = new Dictionary<Guid, WorkItem>();
            }
        }

        public Guid Add(WorkItem workItem)
        {
            workItem.Id = Guid.NewGuid();
            _items[workItem.Id] = workItem;
            return workItem.Id;
        }

        public WorkItem[] GetAll()
        {
            return _items.Values.ToArray();
        }

        public void SaveChanges()
        {
            string json = JsonSerializer.Serialize(_items.Values, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FileName, json); 
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var repository = new FileWorkItemsRepository();

            var workItem = new WorkItem { Title = "Test Work Item" };
            repository.Add(workItem);
            repository.SaveChanges();

            var allItems = repository.GetAll();
            foreach (var item in allItems)
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Title}");
            }

            Console.WriteLine("Work element was saved.");
        }
    }
}
