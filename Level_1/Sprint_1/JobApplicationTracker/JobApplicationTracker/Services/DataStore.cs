using JobApplicationTracker.Models;

namespace JobApplicationTracker.Services
{
    internal class DataStore<T> where T: class, IEntity
    {
        private readonly List<T> _items = [];

        public void Seed(IList<T> list)
        {
            foreach (var item in list) Add(item);
        }

        public void Add(T item)
        {
            if (item is null)
            {
                Console.WriteLine("can't add null item");
                return;
            }

            item.Id = NextID();
            _items.Add(item);
        }

        public void Update(T item)
        {
            if (item is null)
            {
                Console.WriteLine("can't update null item");
                return;
            }

            for(int i = 0; i < _items.Count; i++)
            {
                if (_items[i].Id == item.Id)
                {
                    _items[i] = item;
                    return;
                }
            }
            Console.WriteLine("this id doesn't exist");
        }

        public void Remove(int id)
        {
            T? item = GetByID(id);
            if(item is null)
            {
                Console.WriteLine("this id already exist");
                return;
            }
            _items.Remove(item);
        }

        public int NextID()
        {
            if (_items.Count == 0) return 1;
            return _items[^1].Id + 1;
        }

        public T? GetByID(int id)
        {
            foreach (T item in _items)
            {
                if (item.Id == id) return item;
            }
            return default;
        }

        public List<T> GetAll() => [.. _items];
    }
}
