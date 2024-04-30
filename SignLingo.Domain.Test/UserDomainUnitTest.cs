using Moq;
using SignLingo.Domain.Interfaces;
using SignLingo.Infrastructure.Interfaces;
using SignLingo.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignLingo.Domain.Test
{
    public class UserDomainUnitTest
    {
        [Fact]
        public async Task SignUp_ValidUser_ReturnsUser()
        {
            // Arrange
            var user = new User()
            {
                First_Name = "John",
                Last_Name = "Doe",
                Password = "securePassword",
                Email = "john@example.com",
                BirthDate = new DateTime(1990, 1, 1),
                CityId = 1
            };

            // Mock
            var userInfrastructure = new Mock<IUserInfrastructure>();
            userInfrastructure.Setup(u => u.SignUp(It.IsAny<User>())).ReturnsAsync(user);

            var encryptDomain = new Mock<IEncryptDomain>();
            encryptDomain.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>((password) => password);

            var tokenDomain = new Mock<ITokenDomain>();
            tokenDomain.Setup(t => t.GenerateJwt(It.IsAny<string>())).Returns("jwt_token");

            var userDomain = new UserDomain(userInfrastructure.Object, encryptDomain.Object, tokenDomain.Object);

            // Act
            var result = await userDomain.SignUp(user);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.First_Name, result.First_Name);
            Assert.Equal(user.Last_Name, result.Last_Name);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal(user.BirthDate, result.BirthDate);
            Assert.Equal(user.CityId, result.CityId);
        }

        [Theory]
        [InlineData("", "Doe", "password", "john@example.com", 1)] // Missing first name
        [InlineData("John", "", "password", "john@example.com", 1)] // Missing last name
        [InlineData("John", "Doe", "password", "invalid-email", 1)] // Invalid email
        [InlineData("John", "Doe", "password", "john@example.com", -1)] // Invalid city ID
        public async Task SignUp_InvalidUser_InvalidEmail(string firstName, string lastName, string password, string email, int cityId)
        {
            // Arrange
            var user = new User()
            {
                First_Name = firstName,
                Last_Name = lastName,
                Password = password,
                Email = email,
                BirthDate = new DateTime(1990, 1, 1),
                CityId = cityId
            };

            // Mock
            var userInfrastructure = new Mock<IUserInfrastructure>();
            userInfrastructure.Setup(u => u.SignUp(It.IsAny<User>())).ReturnsAsync(user);

            var encryptDomain = new Mock<IEncryptDomain>();
            encryptDomain.Setup(e => e.Encrypt(It.IsAny<string>())).Returns<string>((password) => password);

            var tokenDomain = new Mock<ITokenDomain>();
            tokenDomain.Setup(t => t.GenerateJwt(It.IsAny<string>())).Returns("jwt_token");

            var userDomain = new UserDomain(userInfrastructure.Object, encryptDomain.Object, tokenDomain.Object);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => userDomain.SignUp(user));
        }
    }
}
