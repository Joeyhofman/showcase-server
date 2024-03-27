using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.ContactForm.Commands.SendMessage;
using Application.Validators;
using FluentAssertions;

namespace Testing.Application.ContactForm.Commands
{

    public class SendMessageCommandValidatorTests 
    {

        private readonly SendMessageCommandValidator _validator;

        public SendMessageCommandValidatorTests()
        {
            _validator = new SendMessageCommandValidator();
        }

        public static string GenerateStringWithLength(int length)
        {
            return new string('a', length);
        }

        [Fact]
        public void Validate_ValidCommand_ShouldNotHaveErrors()
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptyFirstName_ShouldHaveError(string firstName)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                firstName,
                "Doe",
                "john@example.com",
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("FirstName");
        }

        [Theory]
        [InlineData(61)]
        [InlineData(100)]
        [InlineData(200)]
        [InlineData(1000)]
        public void Validate_FirstName_Should_Not_Exceed_Max_Length(int firstNameLength)
        {
            string firstName = new string('a', firstNameLength);
            // Arrange
            var command = new SendMessageCommand
            (
                firstName,
                "Doe",
                "john@example.com",
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("FirstName");
        }


        //lastnme tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptyLastName_ShouldHaveError(string lastName)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                lastName,
                "john@example.com",
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("LastName");
        }

        [Theory]
        [InlineData(61)]
        [InlineData(100)]
        [InlineData(200)]
        [InlineData(1000)]
        public void Validate_LastName_Should_Not_Exceed_Max_Length(int lastNameLength)
        {
            string lastName = new string('a', lastNameLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                lastName,
                "john@example.com",
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("LastName");
        }

        // phonenumber tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptyPhoneNumber_ShouldHaveError(string phoneNumber)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(13)]
        [InlineData(20)]
        [InlineData(50)]
        [InlineData(1000)]
        public void Validate_PhoneNumber_Should_Not_Exceed_Max_Length(int phoneNumberLength)
        {
            string phoneNumber = new string('a', phoneNumberLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(9)]
        [InlineData(8)]
        [InlineData(4)]
        [InlineData(1)]
        public void Validate_PhoneNumber_Should_Not_Be_Less_Then_Min_Length(int phoneNumberLength)
        {
            string phoneNumber = new string('a', phoneNumberLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }


        [Fact]
        public void Validate_PhoneNumber_Can_Contain_Plus_At_First_Character()
        {
            string phoneNumber = "+31611111111";
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Validate_PhoneNumber_Cant_Contain_Plus_At_Other_Character_Then_First_Character()
        {
            string phoneNumber = "3+1611111111";
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Validate_PhoneNumber_Cant_Contain_Alphanumeric_Characters()
        {
            string phoneNumber = "3+161c1b1a11";
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                phoneNumber,
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }

        //subject tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptySubject_ShouldHaveError(string subject)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                "+31611411323",
                subject,
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("Subject");
        }

        [Theory]
        [InlineData(201)]
        [InlineData(300)]
        [InlineData(400)]
        [InlineData(1000)]
        public void Validate_Subject_Should_Not_Exceed_Max_Length(int subjectLength)
        {
            string subject = new string('a', subjectLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                "+31611411323",
                subject,
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("Subject");
        }

        //message tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptyMessage_ShouldHaveError(string message)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                "+31611411323",
                "Test Subject",
                message
            ); ;

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("Message");
        }

        [Theory]
        [InlineData(601)]
        [InlineData(700)]
        [InlineData(800)]
        [InlineData(1000)]
        public void Validate_Message_Should_Not_Exceed_Max_Length(int messageLength)
        {
            string subject = new string('a', messageLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                "john@example.com",
                "+31611411323",
                "Test subject",
                subject
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Should().ContainSingle()
                .Which.PropertyName.Should().Be("Message");
        }

        //email tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public void Validate_EmptyEmail_ShouldHaveError(string email)
        {
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                email,
                "+31611411323",
                "Test Subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(81)]
        [InlineData(100)]
        [InlineData(250)]
        [InlineData(1000)]
        public void Validate_Email_Should_Not_Exceed_Max_Length(int emailLength)
        {
            string email = new string('a', emailLength);
            // Arrange
            var command = new SendMessageCommand
            (
                "John",
                "Doe",
                email,
                "+31611411323",
                "Test subject",
                "Test Message"
            );

            // Act
            var validationResult = _validator.Validate(command);

            // Assert
            validationResult.IsValid.Should().BeFalse();
        }
    }
}
