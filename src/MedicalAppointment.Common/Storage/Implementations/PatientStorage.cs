using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public class PatientStorage : IStorage<Patient>
    {
        public async Task<Patient> Get(int id)
        {
            using (var ctx = new AppDbContext())
            {
                return await ctx.Patients.SingleOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<int> Add(Patient item)
        {
            using (var ctx = new AppDbContext())
            {
                var patient = await ctx.Patients.AddAsync(item);
                return patient.Entity.Id;
            }
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Update(Patient item)
        {
            throw new NotImplementedException();
        }
    }
}