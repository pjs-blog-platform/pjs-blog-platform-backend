using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.API.Database;
using Users.API.Models.DTO;
using Users.API.Models;
using Users.API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.Intrinsics.X86;
using Microsoft.Data.SqlClient;
using System.Net.Sockets;

namespace Users.API.Tests
{
    public class UserServiceTests
    {
        private readonly Mock<UsersContext> _mockContext;
        private readonly Mock<IUserMapper> _mockMapper;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockContext = new Mock<UsersContext>();
            _mockMapper = new Mock<IUserMapper>();
            _userService = new UserService(_mockContext.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAsync_WhenCalled_ReturnsAllUsers()
        {
            // Arrange
            var user1 = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                PasswordHash = "testpassword",
                FullName = "Test User",
                Bio = "testbio",
                ProfilePictureUrl = "testprofilepicture.url.com",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                IsActive = false,
            };

            var user2 = new User
            {
                Id = 2,
                Username = "test2user",
                Email = "test2user@example.com",
                PasswordHash = "test2password",
                FullName = "Test2 User",
                Bio = "test2bio",
                ProfilePictureUrl = "test2profilepicture.url.com",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                IsActive = false,
            };

            var user3 = new User
            {
                Id = 3,
                Username = "test3user",
                Email = "test3user@example.com",
                PasswordHash = "test3password",
                FullName = "Test3 User",
                Bio = "test3bio",
                ProfilePictureUrl = "test3profilepicture.url.com",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                IsActive = false,
            };

            var userDTO1 = new UserDTO
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                FullName = "Test User",
                Bio = "testbio",
                ProfilePictureUrl = "testprofilepicture.url.com",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000",
                IsActive = false,
            };

            var userDTO2 = new UserDTO
            {
                Id = 2,
                Username = "test2user",
                Email = "test2user@example.com",
                FullName = "Test2 User",
                Bio = "test2bio",
                ProfilePictureUrl = "test2profilepicture.url.com",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000",
                IsActive = false,
            };

            var userDTO3 = new UserDTO
            {
                Id = 3,
                Username = "test3user",
                Email = "test3user@example.com",
                FullName = "Test3 User",
                Bio = "test3bio",
                ProfilePictureUrl = "test3profilepicture.url.com",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000",
                IsActive = false,
            };

            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User> { user1, user2, user3 }.AsQueryable());
            _mockMapper.Setup(m => m.Map(user1)).Returns(userDTO1);
            _mockMapper.Setup(m => m.Map(user2)).Returns(userDTO2);
            _mockMapper.Setup(m => m.Map(user3)).Returns(userDTO3);

            // Act
            var result = await _userService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());

            var result1 = result.ToList()[0];
            var result2 = result.ToList()[1];
            var result3 = result.ToList()[2];

            Assert.Equal(userDTO1.Id, result1.Id);
            Assert.Equal(userDTO1.Username, result1.Username);
            Assert.Equal(userDTO1.Email, result1.Email);

            Assert.Equal(userDTO2.Id, result2.Id);
            Assert.Equal(userDTO2.Username, result2.Username);
            Assert.Equal(userDTO2.Email, result2.Email);

            Assert.Equal(userDTO3.Id, result3.Id);
            Assert.Equal(userDTO3.Username, result3.Username);
            Assert.Equal(userDTO3.Email, result3.Email);

        }

        [Fact]
        public async Task GetAsync_NoUsers_ReturnsEmptyList()
        {
            // Arrange
            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User>().AsQueryable());

            // Act
            var result = await _userService.GetAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());

        }

        [Fact]
        public async Task GetByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                PasswordHash = "testpassword",
                FullName = "Test User",
                Bio = "testbio",
                ProfilePictureUrl = "testprofilepicture.url.com",
                CreatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                UpdatedAt = new DateTime(2024, 7, 28, 10, 00, 00, 00),
                IsActive = false,
            };


            var userDTO = new UserDTO
            {
                Id = 1,
                Username = "testuser",
                Email = "testuser@example.com",
                FullName = "Test User",
                Bio = "testbio",
                ProfilePictureUrl = "testprofilepicture.url.com",
                CreatedAt = "2024-07-28T10:00:00.0000000",
                UpdatedAt = "2024-07-28T10:00:00.0000000",
                IsActive = false,
            };

            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User> { user }.AsQueryable());
            _mockMapper.Setup(m => m.Map(user)).Returns(userDTO);

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Username, result.Username);
            Assert.Equal(userDTO.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            _mockContext.Setup(c => c.Users).ReturnsDbSet(new List<User>().AsQueryable());

            // Act
            var result = await _userService.GetByIdAsync(1);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ValidUser_ReturnsCreatedUserDTO()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = "testusername",
                Email = "test.email@example.com",
                Password = "testpassword",
            };

            var date = DateTime.UtcNow;

            var user = new User
            {
                Username = createUserDTO.Username,
                Email = createUserDTO.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDTO.Password),
                FullName = "",
                Bio = "",
                ProfilePictureUrl = "",
                CreatedAt = date,
                UpdatedAt = date,
                IsActive = false
            };

            var userDTO = new UserDTO
            {
                Id = 1,  // Assume this ID is assigned by the database
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                CreatedAt = user.CreatedAt.ToString("o"),
                UpdatedAt = user.UpdatedAt.ToString("o"),
                IsActive = user.IsActive
            };

            // Create a mock DbSet<User>
            var mockSet = new Mock<DbSet<User>>();

            mockSet.Setup(m => m.Add(It.IsAny<User>())).Callback<User>(u =>
            {
                u.Id = 1;
            });

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Mock the mapper
            _mockMapper.Setup(m => m.Map(It.IsAny<User>())).Returns((User u) =>
            {
                userDTO.Id = u.Id;
                return userDTO;
            }).Verifiable();

            // Act
            var result = await _userService.CreateAsync(createUserDTO);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userDTO.Id, result.Id);
            Assert.Equal(userDTO.Username, result.Username);
            Assert.Equal(userDTO.Email, result.Email);
            Assert.Equal(userDTO.FullName, result.FullName);
            Assert.Equal(userDTO.Bio, result.Bio);
            Assert.Equal(userDTO.ProfilePictureUrl, result.ProfilePictureUrl);
            Assert.Equal(userDTO.CreatedAt, result.CreatedAt);
            Assert.Equal(userDTO.UpdatedAt, result.UpdatedAt);
            Assert.Equal(userDTO.IsActive, result.IsActive);

            // Verify that the mapper was called once
            _mockMapper.Verify(m => m.Map(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_NullCreateUserDTO_ThrowsArgumentNullException()
        {
            // Arrange
            CreateUserDTO nullCreateUserDTO = null;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _userService.CreateAsync(nullCreateUserDTO));
        }

        [Fact]
        public async Task CreateAsync_NullUsername_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = null,
                Email = "valid.email@example.com",
                Password = "validpassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptyUsername_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = string.Empty,
                Email = "valid.email@example.com",
                Password = "validpassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_NullEmail_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = "validusername",
                Email = null,
                Password = "validpassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptyEmail_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = "validusername",
                Email = string.Empty,
                Password = "validpassword"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_NullPassword_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = "validusername",
                Email = "valid.email@example.com",
                Password = null
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_EmptyPassword_ThrowsArgumentException()
        {
            // Arrange
            var createUserDTO = new CreateUserDTO
            {
                Username = "validusername",
                Email = "valid.email@example.com",
                Password = string.Empty
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.CreateAsync(createUserDTO));
        }

        [Fact]
        public async Task CreateAsync_DuplicateEmail_ThrowsException()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password"
            };

            // Simulate the unique constraint violation for Email
            var _mockSet = new Mock<DbSet<User>>();

            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);

            _mockSet.Setup(m => m.Add(It.IsAny<User>())).Throws(new DbUpdateException("Unique constraint violation",
                new Exception("Users_Email")));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(createUserDto));
            Assert.Equal("Email already in use", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_DuplicateUsername_ThrowsException()
        {
            // Arrange
            var createUserDto = new CreateUserDTO
            {
                Username = "testuser",
                Email = "test2@example.com",
                Password = "password"
            };

            // Simulate the unique constraint violation for Username
            var _mockSet = new Mock<DbSet<User>>();

            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);

            _mockSet.Setup(m => m.Add(It.IsAny<User>())).Throws(new DbUpdateException("Unique constraint violation",
                new Exception("Users_Username")));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _userService.CreateAsync(createUserDto));
            Assert.Equal("Username already in use", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = "Updated Bio",
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updateUser.FullName, existingUser.FullName);
            Assert.Equal(updateUser.Bio, existingUser.Bio);
            Assert.Equal(updateUser.ProfilePictureUrl, existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 999; // ID that doesn't exist
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = "Updated Bio",
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var users = new List<User>().AsQueryable(); // Empty list of users

            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_NullFullname_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = null,
                Bio = "Updated Bio",
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.NotNull(existingUser.FullName);
            Assert.Equal(updateUser.Bio, existingUser.Bio);
            Assert.Equal(updateUser.ProfilePictureUrl, existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyFullname_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "",
                Bio = "Updated Bio",
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.NotEmpty(existingUser.FullName);
            Assert.Equal(updateUser.Bio, existingUser.Bio);
            Assert.Equal(updateUser.ProfilePictureUrl, existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullBio_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = null,
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updateUser.FullName, existingUser.FullName);
            Assert.NotNull(existingUser.Bio);
            Assert.Equal(updateUser.ProfilePictureUrl, existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyBio_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = "",
                ProfilePictureUrl = "https://example.com/updated-profile.jpg"
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updateUser.FullName, existingUser.FullName);
            Assert.NotEmpty(existingUser.Bio);
            Assert.Equal(updateUser.ProfilePictureUrl, existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_NullProfilePictureUrl_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = "Updated Bio",
                ProfilePictureUrl = null
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updateUser.FullName, existingUser.FullName);
            Assert.Equal(updateUser.Bio, existingUser.Bio);
            Assert.NotNull(existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task UpdateAsync_EmptyProfilePictureUrl_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var updateUser = new UpdateUserDTO
            {
                FullName = "Updated Full Name",
                Bio = "Updated Bio",
                ProfilePictureUrl = ""
            };

            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.UpdateAsync(userId, updateUser);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);

            // Verify that the properties were updated as expected
            Assert.Equal(updateUser.FullName, existingUser.FullName);
            Assert.Equal(updateUser.Bio, existingUser.Bio);
            Assert.NotEmpty(existingUser.ProfilePictureUrl);
        }

        [Fact]
        public async Task DeleteAsync_ValidUserId_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var existingUser = new User
            {
                Id = userId,
                Username = "OriginalUsername",
                Email = "originalemail@example.com",
                PasswordHash = "OriginalPasswordHash",
                FullName = "Original Full Name",
                Bio = "Original Bio",
                ProfilePictureUrl = "https://example.com/original-profile.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var users = new List<User> { existingUser }.AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.True(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_UserDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 1;
            var users = new List<User>().AsQueryable();

            // Create a mock DbSet<User> that supports LINQ operations
            var mockSet = new Mock<DbSet<User>>();
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            _mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Mock SaveChangesAsync
            _mockContext.Setup(c => c.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            // Act
            var result = await _userService.DeleteAsync(userId);

            // Assert
            Assert.False(result);
            _mockContext.Verify(c => c.SaveChangesAsync(default), Times.Never);
        }
    }
}
