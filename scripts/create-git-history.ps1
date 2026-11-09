$ErrorActionPreference = "Stop"

$name = (& git config user.name 2>$null)
$email = (& git config user.email 2>$null)
if ([string]::IsNullOrWhiteSpace($name) -or [string]::IsNullOrWhiteSpace($email)) {
    Write-Host "Сначала укажите имя и email для Git:" -ForegroundColor Yellow
    Write-Host '  git config --global user.name "Ваше имя"'
    Write-Host '  git config --global user.email "ваш-email@example.com"'
    exit 1
}

if (-not (Test-Path ".git")) {
    git init
    if ($LASTEXITCODE -ne 0) { throw "git init завершился с ошибкой." }
}

git branch -M main
if ($LASTEXITCODE -ne 0) { throw "Не удалось установить ветку main." }

# Commit 1: project definition, model and in-memory data.
git add .gitignore wt-lab6-6minimalapi-denisenko.csproj wt-lab6-6minimalapi-denisenko.slnx Models Data Properties
if ($LASTEXITCODE -ne 0) { throw "Не удалось добавить файлы первого коммита." }
git commit -m "Create Player model and in-memory store"
if ($LASTEXITCODE -ne 0) { throw "Первый commit завершился с ошибкой." }

# Commit 2: API endpoints, validation, statuses and Swagger.
git add Program.cs
if ($LASTEXITCODE -ne 0) { throw "Не удалось добавить Program.cs." }
git commit -m "Implement Player REST CRUD and Swagger"
if ($LASTEXITCODE -ne 0) { throw "Второй commit завершился с ошибкой." }

# Commit 3: container configuration, documentation and test scripts.
git add .dockerignore Dockerfile README.md DEPLOYMENT_CHECKLIST.md requests.http scripts
if ($LASTEXITCODE -ne 0) { throw "Не удалось добавить файлы третьего коммита." }
git commit -m "Add Docker deployment guide and API tests"
if ($LASTEXITCODE -ne 0) { throw "Третий commit завершился с ошибкой." }

Write-Host "Созданы три осмысленных commit. Проверьте историю командой: git log --oneline" -ForegroundColor Green
Write-Host "Затем добавьте origin и отправьте main на GitHub." -ForegroundColor Green
