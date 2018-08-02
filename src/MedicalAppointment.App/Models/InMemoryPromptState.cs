using System;
using System.Collections.Generic;

namespace MedicalAppointment.App.Models
{
    public class InMemoryPromptState : Dictionary<string, object>
    {
        private const string NameKey = "name";
        private const string BithdateKey = "birth";

        public InMemoryPromptState()
        {
            this[NameKey] = null;
            this[BithdateKey] = DateTime.MinValue;
        }

        public string Name
        {
            get => (string)this[NameKey];
            set => this[NameKey] = value;
        }

        public DateTime Birthdate
        {
            get => (DateTime)this[BithdateKey];
            set => this[BithdateKey] = value;
        }
    }
}
