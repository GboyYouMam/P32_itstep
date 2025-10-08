# 📝 Notes Web Application

Веб-застосунок для створення та організації особистих нотаток із тегами. Працює на ASP.NET Core MVC, використовує SQLite для зберігання даних, підтримує авторизацію та запуск у Docker.

---

## 🚦 Швидкий старт

### Локально (без Docker)
1. Встановіть [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
2. Відкрийте термінал у корені проєкту
3. Відновіть пакети:
   ```bash
   dotnet restore
   ```
4. Застосуйте міграції (якщо потрібно):
   ```bash
   dotnet ef database update
   ```
5. Запустіть застосунок:
   ```bash
   dotnet run
   ```
6. Відкрийте браузер: [http://localhost:8080](http://localhost:8080) (або порт, вказаний у консолі)

### У Docker
1. Зберіть Docker-образ:
   ```bash
   docker build -t notes-app .
   ```
2. Запустіть контейнер із volume для збереження бази:
   ```bash
   docker run -d -p 8080:8080 -v notes_data:/app/notes_data --name notes notes-app
   ```
   - Застосунок буде доступний на [http://localhost:8080](http://localhost:8080)
   - База даних зберігається у volume `notes_data` (не губиться при перезапуску)
3. Для зміни порту:
   ```bash
   docker run -d -p 5000:5000 -e ASPNETCORE_URLS=http://*:5000 -v notes_data:/app/notes_data --name notes notes-app
   ```

---

## ⚙️ Налаштування

- **ASPNETCORE_ENVIRONMENT** — середовище запуску (Development/Production)
- **ConnectionStrings__DefaultConnection** — шлях до бази (за замовчуванням: Data Source=/app/notes_data/notes.db)

---

## 📦 Структура проєкту

```
├── Controllers/           # MVC контролери
├── Models/                # Моделі даних
├── Views/                 # Представлення (Razor)
├── Db/                    # Контекст EF Core, ініціалізація
├── Migrations/            # Міграції EF Core
├── Entities/              # Сутності бази даних
├── Mappers/               # Маппери моделей
├── wwwroot/               # Статичні файли (CSS, JS, Bootstrap)
├── notes.db               # Файл бази SQLite (локально)
├── Dockerfile             # Docker-інструкції
└── Program.cs             # Точка входу
```

---

## 🚀 Можливості

- ✍️ Створення, редагування, видалення нотаток
- 🔍 Перегляд деталей нотатки
- 🔒 Персональні нотатки для кожного користувача
- 🏷️ Створення та керування тегами
- 🔗 Прив'язка тегів до нотаток
- 📊 Статистика використання тегів
- 📄 Пагінація для списків нотаток і тегів
- 👤 Аутентифікація через ASP.NET Core Identity

---

## 🛠️ Технології

- ASP.NET Core MVC 9.0
- Entity Framework Core
- SQLite (Docker/локально)
- Bootstrap 5
- ASP.NET Core Identity
- Docker

---

## 🗃️ Автор

- git @GboyYouMam

---

### 📢 Примітки
- Для production використовуйте volume для збереження бази.
- Для розробки можна запускати без Docker, база буде створена у файлі notes.db у корені проєкту.
- Якщо потрібно змінити шлях до бази — використовуйте змінну середовища ConnectionStrings__DefaultConnection.
