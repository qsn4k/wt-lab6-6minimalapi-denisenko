# WT-6 — Tournament Hub REST API

Лабораторная работа № 6 по дисциплине «Web-технологии». Проект продолжает предметную область Tournament Hub из WT-5, но предоставляет доступ к профилям игроков через REST API на ASP.NET Core Minimal API.

## Технологии

- .NET 10 (LTS), ASP.NET Core Minimal API;
- Swashbuckle.AspNetCore 10.3.1, OpenAPI и Swagger UI;
- JSON для HTTP-запросов и ответов;
- общая статическая коллекция `PlayerStore.Players` в памяти процесса;
- Docker multi-stage build, подготовка к размещению на Render Web Service.


## Состав проекта

```text
wt-lab6-6minimalapi-denisenko/
├── Data/
│   └── PlayerStore.cs
├── Models/
│   ├── Player.cs
│   └── PlayerRequest.cs
├── Properties/
│   └── launchSettings.json
├── scripts/
│   └── smoke-test.ps1
├── .dockerignore
├── .gitignore
├── Dockerfile
├── Program.cs
├── README.md
├── wt-lab6-6minimalapi-denisenko.slnx
├── requests.http
└── wt-lab6-6minimalapi-denisenko.csproj
```
# wt-lab6-6minimalapi-denisenko
