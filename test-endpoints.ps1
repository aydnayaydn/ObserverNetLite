# ObserverNetLite API Test Script

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Observer NetLite API Endpoint Testleri" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

$baseUrl = "http://localhost:5258"

# Test 1: Connection Test
Write-Host "1. Connection Endpoint Test..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/connection" -Method GET
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Response: $($response.Content)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 2: Login with admin credentials
Write-Host "2. Login Endpoint Test (admin)..." -ForegroundColor Yellow
$loginBody = @{
    userName = "admin"
    password = "admin123"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $tokenResponse = $response.Content | ConvertFrom-Json
    $adminToken = $tokenResponse.token
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Token alindi (ilk 50 karakter): $($adminToken.Substring(0, [Math]::Min(50, $adminToken.Length)))..." -ForegroundColor Green
    Write-Host "   OK Expiration: $($tokenResponse.expiration)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 3: Login with guest credentials
Write-Host "3. Login Endpoint Test (guest)..." -ForegroundColor Yellow
$loginBody = @{
    userName = "guest"
    password = "guest123"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $tokenResponse = $response.Content | ConvertFrom-Json
    $guestToken = $tokenResponse.token
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Token alindi" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 4: Test Admin Endpoint (without token)
Write-Host "4. Test Admin Endpoint (Token olmadan - basarisiz olmali)..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/test-admin" -Method GET
    Write-Host "   FAIL Beklenmedik basari!" -ForegroundColor Red
}
catch {
    Write-Host "   OK Beklenen hata: 401 Unauthorized" -ForegroundColor Green
}
Write-Host ""

# Test 5: Test Admin Endpoint (with admin token)
Write-Host "5. Test Admin Endpoint (Admin token ile)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/test-admin" -Method GET -Headers $headers
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Response: $($response.Content)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 6: Get All Users
Write-Host "6. Get All Users..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users" -Method GET
    $users = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Kullanici sayisi: $($users.Count)" -ForegroundColor Green
    foreach ($user in $users) {
        $rolesStr = if ($user.roleNames) { $user.roleNames -join ", " } else { "N/A" }
        Write-Host "     - ID: $($user.id), UserName: $($user.userName), Roles: $rolesStr" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 7: Get User by ID
Write-Host "7. Get User by ID (admin kullanicisi)..." -ForegroundColor Yellow
try {
    $userId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users/$userId" -Method GET
    $user = $response.Content | ConvertFrom-Json
    $rolesStr = if ($user.roleNames) { $user.roleNames -join ", " } else { "N/A" }
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK User: $($user.userName), Roles: $rolesStr" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 8: Get All Roles
Write-Host "8. Get All Roles..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/roles" -Method GET -Headers $headers
    $roles = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Role sayisi: $($roles.Count)" -ForegroundColor Green
    foreach ($role in $roles) {
        Write-Host "     - ID: $($role.id), Name: $($role.name), IsActive: $($role.isActive)" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 9: Get All Permissions
Write-Host "9. Get All Permissions..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/permissions" -Method GET -Headers $headers
    $permissions = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Permission sayisi: $($permissions.Count)" -ForegroundColor Green
    foreach ($perm in $permissions) {
        Write-Host "     - Code: $($perm.code), Name: $($perm.name), Category: $($perm.category)" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 10: Get Permissions by Category
Write-Host "10. Get Permissions by Category (User)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/permissions/category/User" -Method GET -Headers $headers
    $permissions = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK User category permission sayisi: $($permissions.Count)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 11: Get All Menus
Write-Host "11. Get All Menus..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/menus" -Method GET -Headers $headers
    $menus = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Menu sayisi: $($menus.Count)" -ForegroundColor Green
    foreach ($menu in $menus) {
        Write-Host "     - Name: $($menu.name), Title: $($menu.title), Route: $($menu.route)" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 12: Get Menu Hierarchy
Write-Host "12. Get Menu Hierarchy..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $guestToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/menus/hierarchy" -Method GET -Headers $headers
    $menus = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Root menu sayisi: $($menus.Count)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 13: Get Menus by Role (admin)
Write-Host "13. Get Menus by Role (admin role)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $guestToken"
    }
    $adminRoleId = "11111111-1111-1111-1111-111111111111"
    $response = Invoke-WebRequest -Uri "$baseUrl/api/menus/role/$adminRoleId" -Method GET -Headers $headers
    $menus = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Admin gorebilecegi menu sayisi: $($menus.Count)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 14: Get Menus by Role (guest)
Write-Host "14. Get Menus by Role (guest role)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $guestToken"
    }
    $guestRoleId = "22222222-2222-2222-2222-222222222222"
    $response = Invoke-WebRequest -Uri "$baseUrl/api/menus/role/$guestRoleId" -Method GET -Headers $headers
    $menus = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Guest gorebilecegi menu sayisi: $($menus.Count)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 15: Get Role with Permissions
Write-Host "15. Get Role with Permissions (admin)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $adminRoleId = "11111111-1111-1111-1111-111111111111"
    $response = Invoke-WebRequest -Uri "$baseUrl/api/roles/$adminRoleId/permissions" -Method GET -Headers $headers
    $roleWithPerms = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Role: $($roleWithPerms.name), Permission sayisi: $($roleWithPerms.permissions.Count)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 16: Create New User with Multiple Roles
Write-Host "16. Create New User (Multiple Roles)..." -ForegroundColor Yellow
$newUser = @{
    userName = "testuser"
    password = "testpass123"
    email = "test@example.com"
    roleIds = @("11111111-1111-1111-1111-111111111111", "22222222-2222-2222-2222-222222222222")
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users" -Method POST -Body $newUser -ContentType "application/json"
    $createdUser = $response.Content | ConvertFrom-Json
    $createdUserId = $createdUser.id
    $rolesStr = if ($createdUser.roleNames) { $createdUser.roleNames -join ", " } else { "N/A" }
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Olusturulan kullanici ID: $createdUserId" -ForegroundColor Green
    Write-Host "   OK UserName: $($createdUser.userName), Roles: $rolesStr" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 17: Update User Roles
Write-Host "17. Update User Roles..." -ForegroundColor Yellow
if ($createdUserId) {
    $updateUser = @{
        id = $createdUserId
        userName = "testuser_updated"
        email = "test@example.com"
        roleIds = @("22222222-2222-2222-2222-222222222222")  # Sadece guest
    } | ConvertTo-Json

    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/api/users/$createdUserId" -Method PUT -Body $updateUser -ContentType "application/json"
        $updatedUser = $response.Content | ConvertFrom-Json
        $rolesStr = if ($updatedUser.roleNames) { $updatedUser.roleNames -join ", " } else { "N/A" }
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Updated UserName: $($updatedUser.userName)" -ForegroundColor Green
        Write-Host "   OK Updated Roles: $rolesStr" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Kullanici olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 18: Create New Role
Write-Host "18. Create New Role..." -ForegroundColor Yellow
$newRole = @{
    name = "moderator"
    description = "Moderator role for content management"
    isActive = $true
} | ConvertTo-Json

try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/roles" -Method POST -Body $newRole -ContentType "application/json" -Headers $headers
    $createdRole = $response.Content | ConvertFrom-Json
    $createdRoleId = $createdRole.id
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Olusturulan role ID: $createdRoleId" -ForegroundColor Green
    Write-Host "   OK Role Name: $($createdRole.name)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 19: Assign Permissions to Role
Write-Host "19. Assign Permissions to Role..." -ForegroundColor Yellow
if ($createdRoleId) {
    $assignPerms = @{
        roleId = $createdRoleId
        permissionIds = @(
            "10000001-0000-0000-0000-000000000001",  # USER_VIEW
            "10000003-0000-0000-0000-000000000003",  # USER_EDIT
            "30000001-0000-0000-0000-000000000001"   # MENU_VIEW
        )
    } | ConvertTo-Json

    try {
        $headers = @{
            Authorization = "Bearer $adminToken"
        }
        $response = Invoke-WebRequest -Uri "$baseUrl/api/roles/$createdRoleId/assign-permissions" -Method POST -Body $assignPerms -ContentType "application/json" -Headers $headers
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Permissions atandi" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Role olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 20: Create New Menu
Write-Host "20. Create New Menu..." -ForegroundColor Yellow
$newMenu = @{
    name = "reports"
    title = "Reports"
    icon = "chart"
    route = "/reports"
    order = 6
    isActive = $true
} | ConvertTo-Json

try {
    $headers = @{
        Authorization = "Bearer $adminToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/menus" -Method POST -Body $newMenu -ContentType "application/json" -Headers $headers
    $createdMenu = $response.Content | ConvertFrom-Json
    $createdMenuId = $createdMenu.id
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Olusturulan menu ID: $createdMenuId" -ForegroundColor Green
    Write-Host "   OK Menu Name: $($createdMenu.name)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 21: Delete Menu
Write-Host "21. Delete Menu..." -ForegroundColor Yellow
if ($createdMenuId) {
    try {
        $headers = @{
            Authorization = "Bearer $adminToken"
        }
        $response = Invoke-WebRequest -Uri "$baseUrl/api/menus/$createdMenuId" -Method DELETE -Headers $headers
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Menu silindi" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Menu olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 22: Delete Role
Write-Host "22. Delete Role..." -ForegroundColor Yellow
if ($createdRoleId) {
    try {
        $headers = @{
            Authorization = "Bearer $adminToken"
        }
        $response = Invoke-WebRequest -Uri "$baseUrl/api/roles/$createdRoleId" -Method DELETE -Headers $headers
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Role silindi" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Role olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 23: Delete User
Write-Host "23. Delete User..." -ForegroundColor Yellow
if ($createdUserId) {
    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/api/users/$createdUserId" -Method DELETE
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Kullanici silindi" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Kullanici olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 24: Reset Password
Write-Host "24. Reset Password (admin kullanicisi)..." -ForegroundColor Yellow
$resetPasswordBody = @{
    userName = "admin"
    oldPassword = "admin123"
    newPassword = "newadmin123"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/reset-password" -Method POST -Body $resetPasswordBody -ContentType "application/json"
    $result = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Message: $($result.message)" -ForegroundColor Green
    
    # Test yeni şifre ile login
    Write-Host "   Testing login with new password..." -ForegroundColor Cyan
    $newLoginBody = @{
        userName = "admin"
        password = "newadmin123"
    } | ConvertTo-Json
    
    $loginResponse = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $newLoginBody -ContentType "application/json"
    Write-Host "   OK Yeni sifre ile login basarili" -ForegroundColor Green
    
    # Şifreyi eski haline döndür
    Write-Host "   Reverting password back..." -ForegroundColor Cyan
    $revertPasswordBody = @{
        userName = "admin"
        oldPassword = "newadmin123"
        newPassword = "admin123"
    } | ConvertTo-Json
    
    Invoke-WebRequest -Uri "$baseUrl/api/auth/reset-password" -Method POST -Body $revertPasswordBody -ContentType "application/json" | Out-Null
    Write-Host "   OK Sifre eski haline donduruldu" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Tum testler tamamlandi!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
