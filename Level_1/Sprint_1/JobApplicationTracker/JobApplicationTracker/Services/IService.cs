using JobApplicationTracker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobApplicationTracker.Services
{
    internal interface IService<T> where T: class, IEntity
    {
        public void Add();
        public void Update();
        public void List();
        public void Remove();
        public T? GetByID(int id);
        public void Print(T item);
    }
}
