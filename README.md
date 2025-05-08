# ♟️ RookAround — Chess Tournament Management System

**RookAround** is a comprehensive chess tournament management system built with **C#**, **Entity Framework Core**, and **Avalonia**. It enables tournament organizers to efficiently manage players, venues, resources, and various match styles with a flexible and scalable architecture.

---

## Features

- **Tournament Management**  
  Create, edit, and delete tournaments with custom start/end dates

- **Resource Tracking**  
  Manage and allocate tables, chairs, chess boards, and other resources

- **Player Management**  
  Register and manage regular players and Grandmasters (GMs), including availability tracking

- **Venue Scheduling**  
  Book and manage venues with availability and capacity constraints

- **Match Composition**  
  Create different match types (e.g., regular, duck chess, GM vs player) using the **corator Pattern**- **Data Persistence**  
  Store and load data via **PostgreSQL** or local **JSON** files

---

## Architecture & Design Patterns

- **Repository Pattern**  
  Abstracted via `IDataManager`, supports both EF Core (PostgreSQL) and JSON file storage

- **Decorator Pattern**  
  Enables flexible creation of composite match types

- **Entity Framework Core**  
  For robust database interaction and migration handling

---

## Core Components

- **Tournament**: Central unit managing matches, players, and resources  
- **Festival**: A container for multiple tournaments with shared resource coordination  
- **Match**: Configurable chess match types with rules and requirements  
- **Player**: Regular participants and Grandmasters with scheduling constraints  
- **Venue**: Physical locations with availability and capacity limits  
- **Resource**: Inventory (e.g., boards, clocks, chairs) managed across events

---

## 🗂 Data Model

The system uses **Entity Framework Core** to manage:

- `Tournaments`  
- `Players` (including GMs)  
- `Matches`  
- `Venues`  
- `Resources`  
- `Managers`

---

## 🛠 Getting Started

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [PostgreSQL](https://www.postgresql.org/) (optional if using JSON storage)

### Installation

```bash
# 1. Clone the repository
git clone https://github.com/yourusername/RookAround.git
cd RookAround

# 2. Restore dependencies
dotnet restore

# 3. (Optional) Update the PostgreSQL connection string in RookAroundContext.cs

# 4. Run the application
dotnet run
