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
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RolePermission> _rolePermissionRepository;
        private readonly IRepository<Permission> _permissionRepository;
        private readonly IMapper _mapper;
        private readonly EmailHelper _emailHelper;
        private readonly PasswordResetSettings _resetSettings;

        public UserService(
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<Role> roleRepository,
            IRepository<RolePermission> rolePermissionRepository,
            IRepository<Permission> permissionRepository,
            IMapper mapper,
            EmailHelper emailHelper,
            PasswordResetSettings resetSettings)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _permissionRepository = permissionRepository;
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
            
            // Load user roles
            var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == userId);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
            var allRoles = await _roleRepository.GetAllAsync();
            var roles = allRoles.Where(r => roleIds.Contains(r.Id)).ToList();
            
            var userDto = _mapper.Map<UserDto>(user);
            userDto.RoleIds = roles.Select(r => r.Id).ToList();
            userDto.RoleNames = roles.Select(r => r.Name).ToList();
            
            // Load permissions from all user roles
            var permissions = new List<PermissionDto>();
            foreach (var roleId in roleIds)
            {
                var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
                var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
                var allPermissions = await _permissionRepository.GetAllAsync();
                var rolePerms = allPermissions.Where(p => permissionIds.Contains(p.Id)).ToList();
                permissions.AddRange(_mapper.Map<List<PermissionDto>>(rolePerms));
            }
            userDto.Permissions = permissions.DistinctBy(p => p.Id).ToList();
            
            return userDto;
        }

        public async Task<UserDto?> GetUserByUserNameAsync(string userName)
        {
            var user = (await _userRepository.FindAsync(u => u.UserName == userName)).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            
            // Load user roles
            var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == user.Id);
            var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
            var allRoles = await _roleRepository.GetAllAsync();
            var roles = allRoles.Where(r => roleIds.Contains(r.Id)).ToList();
            
            var userDto = _mapper.Map<UserDto>(user);
            userDto.RoleIds = roles.Select(r => r.Id).ToList();
            userDto.RoleNames = roles.Select(r => r.Name).ToList();
            
            // Load permissions from all user roles
            var permissions = new List<PermissionDto>();
            foreach (var roleId in roleIds)
            {
                var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
                var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
                var allPermissions = await _permissionRepository.GetAllAsync();
                var rolePerms = allPermissions.Where(p => permissionIds.Contains(p.Id)).ToList();
                permissions.AddRange(_mapper.Map<List<PermissionDto>>(rolePerms));
            }
            userDto.Permissions = permissions.DistinctBy(p => p.Id).ToList();
            
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = (await _userRepository.GetAllAsync()).ToList();
            var userDtos = new List<UserDto>();
            
            foreach (var user in users)
            {
                var userRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == user.Id);
                var roleIds = userRoles.Select(ur => ur.RoleId).ToList();
                var allRoles = await _roleRepository.GetAllAsync();
                var roles = allRoles.Where(r => roleIds.Contains(r.Id)).ToList();
                
                var userDto = _mapper.Map<UserDto>(user);
                userDto.RoleIds = roles.Select(r => r.Id).ToList();
                userDto.RoleNames = roles.Select(r => r.Name).ToList();
                
                // Load permissions from all user roles
                var permissions = new List<PermissionDto>();
                foreach (var roleId in roleIds)
                {
                    var rolePermissions = await _rolePermissionRepository.FindAsync(rp => rp.RoleId == roleId);
                    var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();
                    var allPermissions = await _permissionRepository.GetAllAsync();
                    var rolePerms = allPermissions.Where(p => permissionIds.Contains(p.Id)).ToList();
                    permissions.AddRange(_mapper.Map<List<PermissionDto>>(rolePerms));
                }
                userDto.Permissions = permissions.DistinctBy(p => p.Id).ToList();
                
                userDtos.Add(userDto);
            }
            
            return userDtos;
        }
        
        public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            // Hash the password before saving
            user.Password = EncryptionHelper.ComputeMd5Hash(user.Password);
            var createdUser = await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
            
            // Add user roles
            foreach (var roleId in createUserDto.RoleIds)
            {
                var userRole = new UserRole
                {
                    UserId = createdUser.Id,
                    RoleId = roleId
                };
                await _userRoleRepository.AddAsync(userRole);
            }
            await _userRoleRepository.SaveChangesAsync();
            
            return await GetUserByIdAsync(createdUser.Id) ?? _mapper.Map<UserDto>(createdUser);
        }

        public async Task<UserDto?> UpdateUserAsync(UserDto updateUserDto)
        {
            var existingUser = await _userRepository.GetByIdAsync(updateUserDto.Id);
            if (existingUser == null)
            {
                return null;
            }
            
            existingUser.UserName = updateUserDto.UserName;
            existingUser.Email = updateUserDto.Email;
            await _userRepository.UpdateAsync(existingUser);
            
            // Update user roles - remove old ones and add new ones
            var existingUserRoles = await _userRoleRepository.FindAsync(ur => ur.UserId == updateUserDto.Id);
            foreach (var userRole in existingUserRoles)
            {
                await _userRoleRepository.DeleteAsync(userRole);
            }
            
            foreach (var roleId in updateUserDto.RoleIds)
            {
                var userRole = new UserRole
                {
                    UserId = updateUserDto.Id,
                    RoleId = roleId
                };
                await _userRoleRepository.AddAsync(userRole);
            }
            
            await _userRepository.SaveChangesAsync();
            await _userRoleRepository.SaveChangesAsync();
            
            return await GetUserByIdAsync(updateUserDto.Id);
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
            try
            {
                await _emailHelper.SendPasswordResetEmailAsync(user.Email!, user.UserName, resetToken, _resetSettings.ResetUrl);
            }
            catch (Exception)
            {
                // Email sending failed, but we still return success
                // In production, you should log this error
                // For development, print the token to console
                Console.WriteLine($"[DEV] Password Reset Token for {user.UserName}: {resetToken}");
            }

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
