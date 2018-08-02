using System.Collections.Generic;

namespace MedicalAppointment.App.Models
{
    public class InMemoryPromptState : Dictionary<string, object>
    {
        private const string NameKey = "name";
        private const string AgeKey = "age";

        public InMemoryPromptState()
        {
            this[NameKey] = null;
            this[AgeKey] = 0;
        }

        public string Name
        {
            get => (string)this[NameKey];
            set => this[NameKey] = value;
        }

        public int Age
        {
            get => (int)this[AgeKey];
            set => this[AgeKey] = value;
        }
    }
}
