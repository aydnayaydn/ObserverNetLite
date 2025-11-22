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

# Test 3: Login with user credentials
Write-Host "3. Login Endpoint Test (user)..." -ForegroundColor Yellow
$loginBody = @{
    userName = "user"
    password = "user123"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    $tokenResponse = $response.Content | ConvertFrom-Json
    $userToken = $tokenResponse.token
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

# Test 6: Test User Endpoint (with user token)
Write-Host "6. Test User Endpoint (User token ile)..." -ForegroundColor Yellow
try {
    $headers = @{
        Authorization = "Bearer $userToken"
    }
    $response = Invoke-WebRequest -Uri "$baseUrl/api/auth/test-user" -Method GET -Headers $headers
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Response: $($response.Content)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 7: Get All Users
Write-Host "7. Get All Users..." -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users" -Method GET
    $users = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Kullanici sayisi: $($users.Count)" -ForegroundColor Green
    foreach ($user in $users) {
        Write-Host "     - ID: $($user.id), UserName: $($user.userName), Role: $($user.role)" -ForegroundColor Cyan
    }
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 8: Get User by ID
Write-Host "8. Get User by ID (admin kullanicisi)..." -ForegroundColor Yellow
try {
    $userId = "8e445865-a24d-4543-a6c6-9443d048cdb9"
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users/$userId" -Method GET
    $user = $response.Content | ConvertFrom-Json
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK User: $($user.userName), Role: $($user.role)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 9: Create User
Write-Host "9. Create New User..." -ForegroundColor Yellow
$newUser = @{
    userName = "testuser"
    password = "testpass123"
    role = "user"
} | ConvertTo-Json

try {
    $response = Invoke-WebRequest -Uri "$baseUrl/api/users" -Method POST -Body $newUser -ContentType "application/json"
    $createdUser = $response.Content | ConvertFrom-Json
    $createdUserId = $createdUser.id
    Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
    Write-Host "   OK Olusturulan kullanici ID: $createdUserId" -ForegroundColor Green
    Write-Host "   OK UserName: $($createdUser.userName)" -ForegroundColor Green
}
catch {
    Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
}
Write-Host ""

# Test 10: Update User
Write-Host "10. Update User..." -ForegroundColor Yellow
if ($createdUserId) {
    $updateUser = @{
        id = $createdUserId
        userName = "testuser_updated"
        role = "admin"
    } | ConvertTo-Json

    try {
        $response = Invoke-WebRequest -Uri "$baseUrl/api/users/$createdUserId" -Method PUT -Body $updateUser -ContentType "application/json"
        $updatedUser = $response.Content | ConvertFrom-Json
        Write-Host "   OK Status: $($response.StatusCode)" -ForegroundColor Green
        Write-Host "   OK Updated UserName: $($updatedUser.userName)" -ForegroundColor Green
        Write-Host "   OK Updated Role: $($updatedUser.role)" -ForegroundColor Green
    }
    catch {
        Write-Host "   FAIL Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}
else {
    Write-Host "   SKIP Kullanici olusturulamadigi icin atlandi" -ForegroundColor Yellow
}
Write-Host ""

# Test 11: Delete User
Write-Host "11. Delete User..." -ForegroundColor Yellow
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

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Tum testler tamamlandi!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
