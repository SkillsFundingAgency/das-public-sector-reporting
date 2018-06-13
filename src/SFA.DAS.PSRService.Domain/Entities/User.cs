using System;

namespace SFA.DAS.PSRService.Domain.Entities
{
    public class User
    {
        private string _name;
        public Guid Id { get; set; }

        public string Name
        {
            get => _name;
            set => checkAndSetToValue(value);
        }

        private void checkAndSetToValue(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(Name));

            _name = value;
        }
    }
}
