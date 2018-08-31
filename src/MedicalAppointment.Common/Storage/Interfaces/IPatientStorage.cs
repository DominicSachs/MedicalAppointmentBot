using System;
using System.Threading.Tasks;
using MedicalAppointment.Common.Entities;

namespace MedicalAppointment.Common.Storage.Interfaces
{
    public interface IPatientStorage : IStorage<Patient>
    {
        Task<Patient> Get(string firstName, string lastName, DateTime birthDate);
    }
}