using FC.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;
namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

public class CategoryTest
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public async Task Instantiate()
    {
        // O Padrão Triple A (Arrange, Act, Assert) é uma convenção para escrever testes unitários.

        // Arrange
        // Nesta etapa nós configuramos tudo o que é necessário para que o nosso teste possa rodar, inicializamos variáveis, criamos alguns test doubles como Mocks ou Spies dentre outras coisas.
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        DateTime datetimeBefore = DateTime.UtcNow;
        await Task.Delay(TimeSpan.FromMilliseconds(1));

        // Act
        // Nesta etapa nós executamos o código que queremos testar.
        var category = new DomainEntity.Category(validData.Name, validData.Description);
        await Task.Delay(TimeSpan.FromMilliseconds(1));
        DateTime datetimeAfter = DateTime.UtcNow;

        // Assert
        // Nesta etapa nós verificamos se o resultado do código é o que esperávamos.
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.True(category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public async Task InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validData = new
        {
            Name = "Category Name",
            Description = "Category Description"
        };
        DateTime datetimeBefore = DateTime.UtcNow;
        await Task.Delay(TimeSpan.FromMilliseconds(1));

        // Act
        var category = new DomainEntity.Category(validData.Name, validData.Description, isActive);
        await Task.Delay(TimeSpan.FromMilliseconds(1));
        DateTime datetimeAfter = DateTime.UtcNow;

        // Assert
        Assert.NotNull(category);
        Assert.Equal(validData.Name, category.Name);
        Assert.Equal(validData.Description, category.Description);
        Assert.NotEqual(default(Guid), category.Id);
        Assert.NotEqual(default(DateTime), category.CreatedAt);
        Assert.True(category.CreatedAt > datetimeBefore);
        Assert.True(category.CreatedAt < datetimeAfter);
        Assert.Equal(isActive, category.IsActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("    ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        Action action = () => new DomainEntity.Category(name!, "Category Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        Action action = () => new DomainEntity.Category("Category Name", null!);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should not be null", exception.Message);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        Action action = () => new DomainEntity.Category(invalidName, "Category Ok Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should have at least 3 characters", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        string invalidName = Enumerable.Range(1, 256).Select(_ => "a").Aggregate((a, b) => a + b);
        Action action = () => new DomainEntity.Category(invalidName, "Category Ok Description");
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should have at most 255 characters", exception.Message);
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        string invalidDescription = Enumerable.Range(1, 10_001).Select(_ => "a").Aggregate((a, b) => a + b);
        Action action = () => new DomainEntity.Category("Category Name", invalidDescription);
        var exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should have at most 10.000 characters", exception.Message);
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description", false);
        Assert.False(category.IsActive);

        // Act
        category.Activate();

        // Assert
        Assert.True(category.IsActive);
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        Assert.True(category.IsActive);

        // Act
        category.Deactivate();

        // Assert
        Assert.False(category.IsActive);
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        var newValues = new
        {
            Name = "New Name",
            Description = "New Description"
        };

        // Act
        category.Update(newValues.Name, newValues.Description);

        // Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(newValues.Description, category.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        string currentDescription = category.Description;
        var newValues = new { Name = "New Name" };

        // Act
        category.Update(newValues.Name);

        // Assert
        Assert.Equal(newValues.Name, category.Name);
        Assert.Equal(currentDescription, category.Description);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");

        // Act
        void action() => category.Update(name!);

        // Assert
        EntityValidationException exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should not be empty or null", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsLessThan3Characters()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        string invalidName = "a".PadLeft(2);

        // Act
        void action() => category.Update(invalidName);

        // Assert
        EntityValidationException exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should have at least 3 characters", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        string invalidName = "a".PadLeft(256, 'a');

        // Act
        void action() => category.Update(invalidName);

        // Assert
        EntityValidationException exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Name should have at most 255 characters", exception.Message);
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Arrange
        var category = new DomainEntity.Category("Category Name", "Category Description");
        string invalidDescription = "a".PadLeft(10_001, 'a');

        // Act
        void action() => category.Update("Category Name", invalidDescription);

        // Assert
        EntityValidationException exception = Assert.Throws<EntityValidationException>(action);
        Assert.Equal("Description should have at most 10.000 characters", exception.Message);
    }
}
