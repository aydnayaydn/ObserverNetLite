using AutoMapper;
using ObserverNetLite.Application.Abstractions;
using ObserverNetLite.Application.DTOs;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Helpers;
using ObserverNetLite.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObserverNetLite.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UserService(
            IRepository<User> userRepository, 
            IMapper mapper,
            IAuthService authService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _authService = authService;
        }

        public async Task<TokenResponseDto> AuthenticateAsync(LoginDto loginDto)
        {
            var isValid = await ValidateUserAsync(loginDto.UserName, loginDto.Password);
            
            if (!isValid)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            var user = (await _userRepository.FindAsync(u => u.UserName == loginDto.UserName)).FirstOrDefault();
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found");
            }
            
            return await _authService.GenerateTokenAsync(user.UserName, user.Role);
        }

        public async Task<bool> ValidateUserAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return false;

            var user = (await _userRepository.FindAsync(u => u.UserName == userName)).FirstOrDefault();
            if (user == null)
                return false;
                
            return user.Password == EncryptionHelper.ComputeMd5Hash(password);
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> GetUserByUserNameAsync(string userName)
        {
            var user = (await _userRepository.FindAsync(u => u.UserName == userName)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }
        
        public async Task<UserDto> CreateUserAsync(UserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            var createdUser = await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto?> UpdateUserAsync(UserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(updateUserDto.Id);
            if (existingUser == null)
            {
                return null;
            }
            
            _mapper.Map(updateUserDto, existingUser);
            await _userRepository.UpdateAsync(existingUser);
            await _userRepository.SaveChangesAsync();
            return _mapper.Map<UserDto>(existingUser);
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return false;
            }
            
            await _userRepository.DeleteAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}