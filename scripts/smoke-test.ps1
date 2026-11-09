param(
    [string]$BaseUrl = "http://localhost:10000"
)

$ErrorActionPreference = "Stop"
$client = [System.Net.Http.HttpClient]::new()
$failures = 0

function Send-ApiRequest {
    param(
        [string]$Method,
        [string]$Path,
        [string]$JsonBody = $null
    )

    $request = [System.Net.Http.HttpRequestMessage]::new([System.Net.Http.HttpMethod]::new($Method), "$BaseUrl$Path")
    if ($null -ne $JsonBody) {
        $request.Content = [System.Net.Http.StringContent]::new($JsonBody, [System.Text.Encoding]::UTF8, "application/json")
    }
    $response = $client.SendAsync($request).GetAwaiter().GetResult()
    $body = $response.Content.ReadAsStringAsync().GetAwaiter().GetResult()
    $statusCode = [int]$response.StatusCode
    $location = $null
    if ($null -ne $response.Headers.Location) {
        $location = $response.Headers.Location.ToString()
    }
    $response.Dispose()
    $request.Dispose()
    return [PSCustomObject]@{
        StatusCode = $statusCode
        Body = $body
        Location = $location
    }
}

function Assert-Status {
    param([string]$Name, [object]$Response, [int]$Expected)
    if ($Response.StatusCode -eq $Expected) {
        Write-Host "PASS  $Name -> $Expected" -ForegroundColor Green
    } else {
        Write-Host "FAIL  $Name -> ожидается $Expected, получено $($Response.StatusCode)" -ForegroundColor Red
        if ($Response.Body) { Write-Host $Response.Body }
        $script:failures++
    }
}

try {
    Assert-Status "GET /" (Send-ApiRequest "GET" "/") 200
    Assert-Status "GET /api/info" (Send-ApiRequest "GET" "/api/info") 200
    Assert-Status "GET /api/players" (Send-ApiRequest "GET" "/api/players") 200
    Assert-Status "GET Swagger OpenAPI JSON" (Send-ApiRequest "GET" "/swagger/v1/swagger.json") 200
    Assert-Status "GET /api/players/1" (Send-ApiRequest "GET" "/api/players/1") 200
    Assert-Status "GET /api/players/999" (Send-ApiRequest "GET" "/api/players/999") 404
    Assert-Status "GET search" (Send-ApiRequest "GET" "/api/players?search=cyber") 200

    $validPlayer = '{"nickname":"SmokeTestPlayer","game":"Valorant","rating":1100,"wins":5,"losses":2}'
    $created = Send-ApiRequest "POST" "/api/players" $validPlayer
    Assert-Status "POST valid player" $created 201
    if ($created.StatusCode -eq 201) {
        $id = (ConvertFrom-Json $created.Body).id
        if (-not $id) { throw "POST returned 201 but did not include id in response body." }
        if (-not $created.Location) { Write-Host "WARN  Location header was not returned." -ForegroundColor Yellow }

        $updatedJson = '{"nickname":"SmokeTestUpdated","game":"Counter-Strike 2","rating":1400,"wins":6,"losses":2}'
        Assert-Status "PUT existing player" (Send-ApiRequest "PUT" "/api/players/$id" $updatedJson) 200
        Assert-Status "PUT missing player" (Send-ApiRequest "PUT" "/api/players/99999" $updatedJson) 404
        Assert-Status "GET updated player" (Send-ApiRequest "GET" "/api/players/$id") 200
        Assert-Status "DELETE existing player" (Send-ApiRequest "DELETE" "/api/players/$id") 204
        Assert-Status "GET deleted player" (Send-ApiRequest "GET" "/api/players/$id") 404
    }
    Assert-Status "DELETE missing player" (Send-ApiRequest "DELETE" "/api/players/99999") 404

    $invalidPlayer = '{"nickname":"","game":"","rating":-1,"wins":-1,"losses":-1}'
    Assert-Status "POST invalid player" (Send-ApiRequest "POST" "/api/players" $invalidPlayer) 400
} catch {
    Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
    $failures++
} finally {
    $client.Dispose()
}

if ($failures -gt 0) {
    Write-Host "Проверка завершена: ошибок $failures." -ForegroundColor Red
    exit 1
}

Write-Host "Проверка завершена без ошибок. Дополнительно откройте $BaseUrl/swagger и проверьте Swagger UI." -ForegroundColor Green
