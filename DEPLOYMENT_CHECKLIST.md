# Проверочный список WT-6 перед сдачей

## Локальная проверка
- [ ] `dotnet restore` завершилась без ошибок.
- [ ] `dotnet build -c Release` завершилась без ошибок.
- [ ] Главный endpoint `/` возвращает `200` и текст `Minimal API работает!`.
- [ ] `/api/info` и `/api/players` возвращают JSON со статусом `200`.
- [ ] `/api/players/1` возвращает игрока; `/api/players/999` возвращает `404`.
- [ ] Поиск `/api/players?search=cyber` отбирает игроков.
- [ ] POST создаёт игрока с серверным ID, статусом `201` и заголовком `Location`.
- [ ] POST с ошибочными значениями возвращает `400` и сообщения проверки.
- [ ] PUT изменяет существующего игрока и возвращает `200`; неизвестный ID возвращает `404`.
- [ ] DELETE возвращает `204`; повторное получение удалённого ID даёт `404`.
- [ ] Swagger UI открывается по `/swagger`, все endpoints описаны и исполняются через **Try it out**.

## Docker
- [ ] `docker build -t wt-lab6-6minimalapi .` завершилась успешно.
- [ ] `docker run --rm -p 10000:10000 -e PORT=10000 wt-lab6-6minimalapi` запускает сервис.
- [ ] Через контейнер доступны `/api/players` и `/swagger`.

## GitHub и Render
- [ ] В GitHub есть не менее трёх осмысленных commits.
- [ ] Render Web Service собирается через Docker.
- [ ] Сохранены скриншоты Render Dashboard и успешного deployment.
- [ ] После первого deployment проверены `/`, `/api/info`, `/api/players` и `/swagger` по публичному HTTPS URL.
- [ ] Создан новый commit, отправлен через `git push`, выполнен повторный deployment.
- [ ] После повторного deployment повторно проверены основные CRUD endpoints.
- [ ] Фактические GitHub и Render URL вписаны в `README.md` и отчёт.
