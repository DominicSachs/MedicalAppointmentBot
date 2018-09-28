using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Data;
using MedicalAppointment.Common.Entities;
using MedicalAppointment.Common.Storage.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MedicalAppointment.Common.Storage.Implementations
{
    public class PatientStorage : Storage<Patient>, IPatientStorage
    {
        public PatientStorage(AppDbContext context) : base(context) { }

        public async Task<Patient> Get(string firstName, string lastName, DateTime birthDate)
        {
            return await Context.Patients.Include(p => p.Appointments).SingleOrDefaultAsync(p => p.FirstName == firstName && p.LastName == lastName && p.BirthDate == birthDate);
        }
    }
}