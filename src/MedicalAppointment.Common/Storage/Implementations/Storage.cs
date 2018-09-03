using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public abstract class Storage<T> : IStorage<T> where T : BaseEntity
    {
        protected readonly AppDbContext Context;

        protected Storage(AppDbContext context)
        {
            Context = context;
        }

        public async Task<T> Get(int id)
        {
            return await Context.Set<T>().SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Add(T item)
        {
            var patient = await Context.Set<T>().AddAsync(item);
            await Context.SaveChangesAsync();

            return patient.Entity.Id;
        }

        public async Task Delete(int id)
        {
            var item = await Get(id);
            Context.Set<T>().Remove(item);
            await Context.SaveChangesAsync();
        }

        public async Task Update(T item)
        {
            await Context.SaveChangesAsync();
        }
    }
}
