using System;
using System.Threading.Tasks;

namespace MedicalAppointment.Common.Storage.Interfaces
{
    public interface IStorage<T> where T : class
    {
        Task<T> Get(int id);
        
        Task<int> Add(T item);
        
        Task Delete(int id);
        
        Task Update(T item);
    }
}