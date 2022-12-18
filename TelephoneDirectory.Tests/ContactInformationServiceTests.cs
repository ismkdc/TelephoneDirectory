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

public class ContactInformationServiceTests
{
    private readonly Mock<TelephoneDirectoryContext> _mockContext;
    private readonly IContactInformationService _service;

    public ContactInformationServiceTests()
    {
        _mockContext = new Mock<TelephoneDirectoryContext>();

        var mapper = new Mapper(MappingConfiguration.Generate());
        _service = new ContactInformationService(_mockContext.Object, mapper);
    }

    [Fact]
    public async Task Create_ThrowsException_WhenContentIsEmpty()
    {
        // Arrange
        var model = new CreateContactInformation(Guid.NewGuid(), ContactInformationTypeEnum.Location, "");

        // Act
        Func<Task> action = async () => await _service.Create(model);

        // Assert
        await action.Should().ThrowAsync<TelephoneDirectoryException>().WithMessage(CustomErrors.E_103.ErrorMessage);
    }

    [Fact]
    public async Task Create_ThrowsException_WhenContactDoesNotExist()
    {
        // Arrange
        var model = new CreateContactInformation(Guid.NewGuid(), ContactInformationTypeEnum.Location, "test");

        var mock = Array.Empty<Contact>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(mock.Object);

        // Act
        Func<Task> action = async () => await _service.Create(model);

        // Assert
        await action.Should().ThrowAsync<TelephoneDirectoryException>().WithMessage(CustomErrors.E_102.ErrorMessage);
    }


    [Fact]
    public async Task Create_CallsSaveChangesAsync_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var model = new CreateContactInformation(id, ContactInformationTypeEnum.Location, "test");

        var contactMock = new List<Contact> { new() { Id = id } }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.Contacts).Returns(contactMock.Object);

        var contactInformationMock = Array.Empty<ContactInformation>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(contactInformationMock.Object);

        // Act
        await _service.Create(model);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }


    [Fact]
    public async Task Delete_ThrowsException_WhenContactInformationDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        var mock = Array.Empty<ContactInformation>().AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(mock.Object);

        // Act
        Func<Task> action = async () => await _service.Delete(id);

        // Assert
        await action.Should().ThrowAsync<TelephoneDirectoryException>().WithMessage(CustomErrors.E_102.ErrorMessage);
    }

    [Fact]
    public async Task Delete_SetsDeletedAt_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contactInformation = new ContactInformation { Id = id };
        var mock = new List<ContactInformation> { contactInformation }.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(mock.Object);

        // Act
        await _service.Delete(id);

        // Assert
        contactInformation.DeletedAt.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_CallsSaveChangesAsync_WhenSuccessful()
    {
        // Arrange
        var id = Guid.NewGuid();
        var contactInformation = new ContactInformation { Id = id };

        var contactInformationList = new List<ContactInformation> { contactInformation };
        var mock = contactInformationList.AsQueryable().BuildMockDbSet();
        _mockContext.Setup(x => x.ContactInformation).Returns(mock.Object);

        // Act
        await _service.Delete(id);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}