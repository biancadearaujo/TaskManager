using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using TaskManager.Application.DTOs.UserDTOs;
using TaskManager.Application.Exceptions;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using Xunit;

namespace TaskManager.Tests.Unit.Application.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<UserEntity>> _userManagerMock;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            var userStoreMock = new Mock<IUserStore<UserEntity>>();
            
            var passwordHasherMock = new Mock<IPasswordHasher<UserEntity>>();
            passwordHasherMock
                .Setup(p => p.HashPassword(It.IsAny<UserEntity>(), It.IsAny<string>()))
                .Returns("hashed-password");

            var userValidators = new List<IUserValidator<UserEntity>>();
            var passwordValidators = new List<IPasswordValidator<UserEntity>>();
            var keyNormalizerMock = new Mock<ILookupNormalizer>();
            var errorsMock = new Mock<IdentityErrorDescriber>();
            var servicesMock = new Mock<IServiceProvider>();
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UserManager<UserEntity>>>();

            _userManagerMock = new Mock<UserManager<UserEntity>>(
                userStoreMock.Object,
                null,
                passwordHasherMock.Object,
                userValidators,
                passwordValidators,
                keyNormalizerMock.Object,
                errorsMock.Object,
                servicesMock.Object,
                loggerMock.Object
            );

            _userService = new UserService(_userManagerMock.Object);
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateUser_WhenDataIsValid()
        {
            var createUserDto = new CreateUserDto
            {
                Email = "test@example.com",
                Password = "StrongPassword123!"
            };

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>(), createUserDto.Password))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _userService.CreateAsync(createUserDto);

            result.Should().NotBeNull();
            result.Email.Should().Be(createUserDto.Email);
            result.UserName.Should().Be(createUserDto.Email);

            _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<UserEntity>(), createUserDto.Password), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldThrowException_WhenCreationFails()
        {
            var createUserDto = new CreateUserDto
            {
                Email = "fail@example.com",
                Password = "weak"
            };

            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<UserEntity>(), createUserDto.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            Func<Task> act = async () => await _userService.CreateAsync(createUserDto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Falha ao criar usuário");
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var user = new UserEntity
            {
                Id = userId,
                Email = "exists@example.com",
                UserName = "exists@example.com",
                CreatedAt = DateTime.UtcNow
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            var result = await _userService.GetByIdAsync(userId);

            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((UserEntity)null);

            Func<Task> act = async () => await _userService.GetByIdAsync(userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with id {userId} not found");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser_WhenDataIsValid()
        {
            var userId = Guid.NewGuid();
            var existingUser = new UserEntity
            {
                Id = userId,
                Email = "old@example.com",
                UserName = "old@example.com",
                CreatedAt = DateTime.UtcNow
            };
            var updateUserDto = new UpdateUserDto
            {
                Email = "new@example.com",
                Password = "NewStrongPassword1!"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(existingUser);

            _userManagerMock.Setup(u => u.UpdateAsync(existingUser))
                .ReturnsAsync(IdentityResult.Success);

            await _userService.UpdateAsync(userId, updateUserDto);

            existingUser.Email.Should().Be(updateUserDto.Email);
            existingUser.PasswordHash.Should().Be("hashed-password"); // via passwordHasherMock

            _userManagerMock.Verify(u => u.UpdateAsync(existingUser), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();
            var updateUserDto = new UpdateUserDto
            {
                Email = "new@example.com",
                Password = "password"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((UserEntity)null);

            Func<Task> act = async () => await _userService.UpdateAsync(userId, updateUserDto);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with id {userId} not found");
        }

        [Fact]
        public async Task UpdateAsync_ShouldThrowException_WhenUpdateFails()
        {
            var userId = Guid.NewGuid();
            var existingUser = new UserEntity
            {
                Id = userId,
                Email = "old@example.com",
                UserName = "old@example.com",
                CreatedAt = DateTime.UtcNow
            };
            var updateUserDto = new UpdateUserDto
            {
                Email = "new@example.com",
                Password = "password"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(existingUser);

            _userManagerMock.Setup(u => u.UpdateAsync(existingUser))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Error" }));

            Func<Task> act = async () => await _userService.UpdateAsync(userId, updateUserDto);

            await act.Should().ThrowAsync<Exception>()
                .WithMessage("Falha ao atualizar usuário");
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser_WhenUserExists()
        {
            var userId = Guid.NewGuid();
            var existingUser = new UserEntity
            {
                Id = userId,
                Email = "delete@example.com",
                UserName = "delete@example.com",
                CreatedAt = DateTime.UtcNow
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(existingUser);

            _userManagerMock.Setup(u => u.DeleteAsync(existingUser))
                .ReturnsAsync(IdentityResult.Success);

            await _userService.DeleteAsync(userId);

            _userManagerMock.Verify(u => u.DeleteAsync(existingUser), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ShouldThrowNotFoundException_WhenUserDoesNotExist()
        {
            var userId = Guid.NewGuid();

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((UserEntity)null);

            Func<Task> act = async () => await _userService.DeleteAsync(userId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"User with id {userId} not found");
        }
    }
}
