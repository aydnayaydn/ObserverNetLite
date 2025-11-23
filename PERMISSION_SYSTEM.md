# ObserverNetLite - Permission Control System

## Overview
This document describes the permission-based access control system implemented in the ObserverNetLite application.

## Permission Codes

### Page Access Permissions
- `view_dashboard` - View Dashboard page
- `view_users` - View Users page
- `view_roles` - View Roles page
- `view_menus` - View Menus page

### User Operation Permissions
- `create_user` - Create new user
- `edit_user` - Edit user
- `delete_user` - Delete user

### Role Operation Permissions
- `create_role` - Create new role
- `edit_role` - Edit role
- `delete_role` - Delete role
- `assign_permissions` - Assign permissions to role

### Menu Operation Permissions
- `create_menu` - Create new menu
- `edit_menu` - Edit menu
- `delete_menu` - Delete menu

## Backend Changes

### 1. UserDto Update
```csharp
public class UserDto
{
    // ... existing fields
    public List<PermissionDto> Permissions { get; set; } = new();
}
```

### 2. UserService Update
- Permission loading added to `GetUserByIdAsync`, `GetUserByUserNameAsync`, and `GetAllUsersAsync` methods
- Permissions collected from all user roles and returned uniquely

### 3. Repository Dependencies
Repositories added to UserService:
- `IRepository<RolePermission>`
- `IRepository<Permission>`

## System Flow

1. **Login:** User logs in
2. **Permission Loading:** Backend collects permissions from all user roles
3. **JWT Token:** Permissions are not stored in token, backend checks on each request
4. **Frontend State:** User info with permissions stored in zustand store
5. **Backend Validation:** Backend performs permission check on each API request (security)

## Security Notes

⚠️ **IMPORTANT:**
- Frontend permission checks are for UX only
- Real security must be enforced on the backend
- Permission checks should be performed on every API endpoint
- Permission codes are fixed and should not be changed

## Database Structure

```
Users
  ├─ UserRoles (User-Role relationship, Many-to-Many)
  │   └─ Roles
  │       └─ RolePermissions (Role-Permission relationship, Many-to-Many)
  │           └─ Permissions
```

## Usage Examples

### Example 1: Create Admin Role
1. Create a new "Admin" role on the Roles page
2. Click "Manage Permissions" button
3. Select all permissions and save
4. Assign Admin role to user

### Example 2: Limited Permission User
1. Create a new "Viewer" role
2. Grant only `view_*` permissions
3. Assign this role to user
4. User can only browse pages, cannot add/delete/edit

### Example 3: User Manager Role
1. Create "User Manager" role
2. Permissions:
   - `view_dashboard`
   - `view_users`
   - `create_user`
   - `edit_user`
   - (DO NOT grant delete_user)
3. User can add and edit users on users page but cannot delete

## Future Improvements

- [ ] Add permission_code field to Menu entity
- [ ] Dynamic permission loading (add new permissions at runtime)
- [ ] Permission groups (e.g., "ADMIN_ALL", "USER_READ_ONLY")
- [ ] Audit log (Who used which permission and when)
- [ ] Permission caching
