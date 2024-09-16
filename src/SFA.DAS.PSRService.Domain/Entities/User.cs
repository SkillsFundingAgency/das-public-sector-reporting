using System;

namespace SFA.DAS.PSRService.Domain.Entities;

public class User
{
    private string _name;
    public Guid Id { get; set; }

    public string Name
    {
        get => _name;
        set => CheckAndSetToValue(value);
    }

    private void CheckAndSetToValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException( nameof(Name));
        }

        _name = value;
    }
}