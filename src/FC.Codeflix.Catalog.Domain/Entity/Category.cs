using FC.Codeflix.Catalog.Domain.Exceptions;

namespace FC.Codeflix.Catalog.Domain.Entity;

public class Category
{
    public void Activate()
    {
        IsActive = true;
        Validate();
    }

    public void Deactivate()
    {
        IsActive = false;
        Validate();
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Category(string name, string description, bool isActive = true)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        IsActive = isActive;
        CreatedAt = DateTime.UtcNow;

        Validate();
    }

    public void Validate()
    {
        if (String.IsNullOrWhiteSpace(Name))
            throw new EntityValidationException($"{nameof(Name)} should not be empty or null");

        if (Description == null)
            throw new EntityValidationException($"{nameof(Description)} should not be null");

        if (Name.Length < 3)
            throw new EntityValidationException($"{nameof(Name)} should have at least 3 characters");

        if (Name.Length > 255)
            throw new EntityValidationException($"{nameof(Name)} should have at most 255 characters");

        if (Description.Length > 10_000)
            throw new EntityValidationException($"{nameof(Description)} should have at most 10.000 characters");
    }
}
