[README.md](https://github.com/user-attachments/files/32260627/README.md)
# ExchangeRateHub

A full-stack currency exchange management web application. Users can view live exchange rates, convert currencies, manage a list of favorite currencies, and operate a multi-currency wallet — all through a modern, dashboard-style interface with dark mode support.

Built as a REST-oriented web services course project.

![Dashboard preview](screenshots/login_register_dashboard.png)

## Features

- **Authentication** — register, login, and access protected endpoints via JWT
- **Live currency rates** — USD, EUR, TRY, PLN, GBP, JPY, CHF and more, visualized with a dynamic chart
- **Currency converter** — instant conversion using real-time exchange rates
- **Favorites** — add, view, and delete favorite currencies
- **Wallet** — deposit balances in multiple currencies and exchange between them
- **Transaction history** — every wallet operation is logged and displayed
- **Dark mode** — toggle between light and dark UI themes

## Tech Stack

**Backend**
- ASP.NET Core Web API (.NET 8)
- C#
- REST API architecture
- JWT Authentication
- Swagger / OpenAPI

**Frontend**
- HTML5, CSS3, JavaScript (vanilla, single-page dashboard)

**External APIs**
- [Frankfurter Exchange API](https://www.frankfurter.app/) — currency conversion and live rates
- National Bank of Poland (NBP) API — additional exchange rate data

**Testing**
- xUnit unit tests (`ExchangeRateHub.Tests`)

## Project Structure

```
ExchangeRateHub/
├── ExchangeRateHub/              # API + static frontend
│   ├── Controllers/              # Auth, Exchange, Favorites, Wallet, Admin
│   ├── Models/                   # Domain models
│   ├── DTOs/                     # Data transfer objects
│   ├── Services/                 # ExchangeService (external API calls)
│   ├── wwwroot/index.html        # Frontend dashboard (HTML/CSS/JS)
│   └── Program.cs                # App startup, JWT & Swagger config
└── ExchangeRateHub.Tests/        # Unit tests
```

## API Overview

| Method | Endpoint                 | Description                    |
|--------|---------------------------|---------------------------------|
| POST   | `/api/Auth/register`      | Register a new user             |
| POST   | `/api/Auth/login`         | Log in and receive a JWT        |
| GET    | `/api/Exchange/rates`     | Get live exchange rates         |
| GET    | `/api/Exchange/nbp-rates` | Get NBP exchange rates          |
| GET    | `/api/Exchange/convert`   | Convert between currencies      |
| GET    | `/api/Favorites`          | List favorite currencies        |
| POST   | `/api/Favorites`          | Add a favorite currency         |
| PUT    | `/api/Favorites/{id}`     | Update a favorite currency      |
| DELETE | `/api/Favorites/{id}`     | Delete a favorite currency      |
| GET    | `/api/Wallet`              | Get wallet balances            |
| GET    | `/api/Wallet/history`      | Get transaction history        |
| POST   | `/api/Wallet/deposit`      | Deposit money                  |
| POST   | `/api/Wallet/exchange`     | Exchange between wallet balances |

All protected endpoints require a `Bearer` JWT token, obtainable via Swagger's **Authorize** button after logging in.

The API returns standard HTTP status codes: `200`, `400`, `401`, `403`, `404`, `500`.

## Getting Started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)

### Run locally

```bash
cd ExchangeRateHub/ExchangeRateHub
dotnet restore
dotnet run
```

The app will start on the URL shown in the console (e.g. `https://localhost:7220`).

- Dashboard: `https://localhost:7220/`
- Swagger UI: `https://localhost:7220/swagger`

### Run tests

```bash
cd ExchangeRateHub/ExchangeRateHub.Tests
dotnet test
```

## Screenshots

| Login & Register | Convert & Favorites |
|---|---|
| ![Login/Register](screenshots/login_register_dashboard.png) | ![Convert/Favorites](screenshots/convert_favorites.png) |

| Wallet | Dark Mode |
|---|---|
| ![Wallet](screenshots/wallet.png) | ![Dark mode](screenshots/dark_mode.png) |

**Swagger / OpenAPI documentation**

![Swagger](screenshots/swagger.png)

## Key Technical Decisions

- JWT was chosen for stateless, secure authentication between frontend and backend.
- A dashboard-style single-page layout was used for a smoother user experience.
- A wallet system was added to simulate a real exchange platform.
- Charts and dark mode were implemented to modernize the interface.
- External APIs (Frankfurter, NBP) provide real, live financial data instead of mock values.

## Authors

- Özgür Gözaydın
- Deniz Gözaydın

## Note

The JWT signing key in `Program.cs` is hardcoded for demo/course purposes. In a production setting it should be moved to configuration (e.g. `appsettings.json` + user secrets, or an environment variable) and never committed to source control.
