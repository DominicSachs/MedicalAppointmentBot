using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public abstract class Storage<T> : IStorage<T> where T : BaseEntity
    {
        public async Task<T> Get(int id)
        {
            using (var ctx = new AppDbContext())
            {
                return await ctx.Set<T>().SingleOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<int> Add(T item)
        {
            using (var ctx = new AppDbContext())
            {
                var patient = await ctx.Set<T>().AddAsync(item);
                await ctx.SaveChangesAsync();

                return patient.Entity.Id;
            }
        }

        public async Task Delete(int id)
        {
            using (var ctx = new AppDbContext())
            {
                var item = await Get(id);
                ctx.Set<T>().Remove(item);
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Update(T item)
        {
            using (var ctx = new AppDbContext())
            {
                await ctx.SaveChangesAsync();
            }
        }
    }
}
