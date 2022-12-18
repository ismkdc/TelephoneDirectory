using FluentAssertions;
using MapsterMapper;
using MockQueryable.Moq;
using Moq;
using TelephoneDirectory.ContactAPI;
using TelephoneDirectory.ContactAPI.Records;
using TelephoneDirectory.ContactAPI.Services;
using TelephoneDirectory.Data.Entities;
using TelephoneDirectory.Data.Enums;
using TelephoneDirectory.Infrastructure.Errors;

namespace TelephoneDirectory.Tests;

public class ContactServiceTests
{
    private readonly Mock<TelephoneDirectoryContext> _mockContext;
    private readonly IContactService _service;

    public ContactServiceTests()
    {
        _mockContext = new Mock<TelephoneDirectoryContext>();

        var mapper = new Mapper(MappingConfiguration.Generate());
        _service = new ContactService(_mockContext.Object, mapper);
    }

    [Fact]
    public async Task Get_ReturnsNull_WhenContactDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var mock = Array.Empty<Contact>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        var result = await _service.Get(id);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task Get_ReturnsContactDetail_WhenContactExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contact = new Contact
        {
            Id = id, Name = "John Smith",
            ContactInformation = new List<ContactInformation>(),
            CreatedAt = DateTime.UtcNow
        };
        var contactInformation = new ContactInformation
        {
            Contact = contact,
            ContactInformationType = ContactInformationTypeEnum.Location,
            Content = "New York",
            CreatedAt = DateTime.UtcNow
        };

        contact.ContactInformation.Add(contactInformation);

        var contactMock = new List<Contact> { contact }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(contactMock.Object);

        var contactInformationMock = new List<ContactInformation> { contactInformation }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(contactInformationMock.Object);

        // Act
        var result = await _service.Get(id);

        // Assert
        result.Should().NotBeNull();
        result?.Name.Should().Be("John Smith");
        result?.ContactInformation.Should().Contain(x => x.Content == "New York");
    }

    [Fact]
    public async Task GetAll_ReturnsEmptyArray_WhenNoContactsExist()
    {
        // Arrange
        var mock = Array.Empty<Contact>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        var result = await _service.GetAll();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetAll_ReturnsContacts_WhenContactsExist()
    {
        // Arrange
        var contact1 = new Contact { Id = Guid.NewGuid(), Name = "John Smith", CreatedAt = DateTime.UtcNow };
        var contact2 = new Contact { Id = Guid.NewGuid(), Name = "Jane Smith", CreatedAt = DateTime.UtcNow };

        var mock = new List<Contact> { contact1, contact2 }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        var result = await _service.GetAll();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(x => x.Name == "John Smith");
    }

    [Fact]
    public async Task GetReportData_ReturnsEmptyArray_WhenNoLocationInformationExists()
    {
        // Arrange
        var contact1 = new Contact { Id = Guid.NewGuid(), Name = "John Smith" };
        var contact2 = new Contact { Id = Guid.NewGuid(), Name = "Jane Smith" };

        var contactInformation1 = new ContactInformation
        {
            Contact = contact1, ContactInformationType = ContactInformationTypeEnum.PhoneNumber,
            Content = "123-456-7890"
        };
        var contactInformation2 = new ContactInformation
        {
            Contact = contact2, ContactInformationType = ContactInformationTypeEnum.PhoneNumber,
            Content = "123-456-7890"
        };

        var mock = new List<ContactInformation>
        {
            contactInformation1,
            contactInformation2
        }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(mock.Object);

        // Act
        var result = await _service.GetReportData();

        // Assert
        result.Should().BeEmpty();
    }


    [Fact]
    public async Task GetReportData_ReturnsCorrectData_WhenLocationInformationExists()
    {
        // Arrange
        var contact1 = new Contact { Id = Guid.NewGuid(), Name = "John Smith" };
        var contact2 = new Contact { Id = Guid.NewGuid(), Name = "Jane Smith" };
        var contact3 = new Contact { Id = Guid.NewGuid(), Name = "Bob Smith" };

        var contactInformation1 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact1.Id,
            Contact = contact1,
            ContactInformationType = ContactInformationTypeEnum.Location,
            Content = "New York"
        };
        var contactInformation2 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact1.Id,
            Contact = contact1,
            ContactInformationType = ContactInformationTypeEnum.PhoneNumber,
            Content = "123-456-7890"
        };
        var contactInformation3 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact2.Id,
            Contact = contact2,
            ContactInformationType = ContactInformationTypeEnum.Location,
            Content = "New York"
        };
        var contactInformation4 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact3.Id,
            Contact = contact3,
            ContactInformationType = ContactInformationTypeEnum.Location,
            Content = "Los Angeles"
        };
        var contactInformation5 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact3.Id,
            Contact = contact3,
            ContactInformationType = ContactInformationTypeEnum.PhoneNumber,
            Content = "123-456-7890"
        };
        var contactInformation6 = new ContactInformation
        {
            Id = Guid.NewGuid(),
            ContactId = contact3.Id,
            Contact = contact3,
            ContactInformationType = ContactInformationTypeEnum.PhoneNumber,
            Content = "123-456-7890"
        };

        var list = new List<ContactInformation>
        {
            contactInformation1,
            contactInformation2,
            contactInformation3,
            contactInformation4,
            contactInformation5,
            contactInformation6
        };

        var mock = list.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(mock.Object);

        // Act
        var result = await _service.GetReportData();

        // Assert
        result.Should().Contain(x =>
            x.Location == "New York" && x.ContactCount == 2 && x.PhoneNumberCount == 1);
        result.Should().Contain(x =>
            x.Location == "Los Angeles" && x.ContactCount == 1 && x.PhoneNumberCount == 2);
    }

    [Fact]
    public async Task Create_ThrowsException_WhenNameIsEmpty()
    {
        // Arrange
        var model = new CreateContact("", "Smith", "Karfur");

        // Act
        Func<Task> action = async () => await _service.Create(model);

        // Assert
        await action.Should().ThrowAsync<TelephoneDirectoryException>().WithMessage(CustomErrors.E_101.ErrorMessage);
    }

    [Fact]
    public async Task Create_CallsSaveChangesAsync_WhenSuccessful()
    {
        // Arrange
        var model = new CreateContact("John", "Smith", "Karfur");

        var mock = Array.Empty<Contact>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        await _service.Create(model);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_ThrowsException_WhenContactDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var mock = Array.Empty<Contact>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        Func<Task> action = async () => await _service.Delete(id);

        // Assert
        await action.Should().ThrowAsync<TelephoneDirectoryException>().WithMessage(CustomErrors.E_102.ErrorMessage);
    }
}