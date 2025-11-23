using AutoMapper;
using ObserverNetLite.Service.Abstractions;
using ObserverNetLite.Service.DTOs;
using ObserverNetLite.Service.Settings;
using ObserverNetLite.Core.Abstractions;
using ObserverNetLite.Core.Entities;
using ObserverNetLite.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObserverNetLite.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IMapper _mapper;
        private readonly EmailHelper _emailHelper;
        private readonly PasswordResetSettings _resetSettings;

        public UserService(
            IRepository<User> userRepository, 
            IMapper mapper,
            EmailHelper emailHelper,
            PasswordResetSettings resetSettings)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _emailHelper = emailHelper;
            _resetSettings = resetSettings;
        }

        public async Task<bool> ValidateUserAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return false;

            var user = (await _userRepository.FindAsync(u => u.UserName == userName)).FirstOrDefault();
            if (user == null)
                return false;

            // Hash the input password and compare with stored hash
            var hashedPassword = EncryptionHelper.ComputeMd5Hash(password);
            return user.Password == hashedPassword;
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
        
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            // Hash the password before saving
            user.Password = EncryptionHelper.ComputeMd5Hash(user.Password);
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

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            // Find user by username
            var user = (await _userRepository.FindAsync(u => u.UserName == resetPasswordDto.UserName)).FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            // Validate old password
            var hashedOldPassword = EncryptionHelper.ComputeMd5Hash(resetPasswordDto.OldPassword);
            if (user.Password != hashedOldPassword)
            {
                return false;
            }

            // Update with new hashed password
            user.Password = EncryptionHelper.ComputeMd5Hash(resetPasswordDto.NewPassword);
            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            // Find user by email
            var user = (await _userRepository.FindAsync(u => u.Email == forgotPasswordDto.Email)).FirstOrDefault();
            if (user == null)
            {
                return false; // User not found
            }

            // Generate password reset token
            var resetToken = Guid.NewGuid().ToString();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1); // Token valid for 1 hour

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            // Send password reset email
            await _emailHelper.SendPasswordResetEmailAsync(user.Email!, user.UserName, resetToken, _resetSettings.ResetUrl);

            return true;
        }

        public async Task<bool> ResetPasswordWithTokenAsync(ResetPasswordWithTokenDto resetDto)
        {
            // Find user by reset token
            var user = (await _userRepository.FindAsync(u => u.PasswordResetToken == resetDto.Token)).FirstOrDefault();
            if (user == null)
            {
                return false; // Invalid token
            }

            // Check if token has expired
            if (user.PasswordResetTokenExpiry == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return false; // Token expired
            }

            // Update password and clear reset token
            user.Password = EncryptionHelper.ComputeMd5Hash(resetDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
