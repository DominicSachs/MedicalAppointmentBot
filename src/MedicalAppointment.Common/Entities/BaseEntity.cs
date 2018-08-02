using System;

namespace MedicalAppointment.Common.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreationDate { get; internal set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}
