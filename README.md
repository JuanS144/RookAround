# RookAround

RookAround - Chess Tournament Management System
RookAround is a chess tournament management system built with C#, Entity Framework Core and Avalonia. The application enables organizers to create and manage chess tournaments with various match styles, player types, and venue requirements.

Features
Tournament Management: Create, edit, and remove tournaments with specific start and end dates
Resource Management: Track available resources (tables, chairs, boards, etc.) needed for tournaments
Player Management: Register regular players and Grandmasters (GMs) with specific availability
Venue Scheduling: Manage venue bookings and availability
Match Composition: Create different match types using the Decorator pattern
Data Persistence: Store data in PostgreSQL or JSON files
Architecture
The system implements several design patterns:

Repository Pattern: Through the IDataManager interface, supporting both Entity Framework and JSON storage
Decorator Pattern: For composing different match types (regular chess, duck chess, GM vs Player matches)
Entity Framework Core: For database interaction with PostgreSQL
Core Components
Tournament: Manages matches, players, resources, and scheduling
Festival: Handles multiple tournaments and ensures resources availability
Match: Defines the rules and requirements for chess matches
Players: Regular players and Grandmasters with different properties and availabilities
Venues: Physical locations with capacity and specific availability
Data Model
The application uses Entity Framework Core with the following main entities:

Tournaments
Players (including GM specialists)
Matches
Venues
Resources
Managers


Getting Started
Prerequisites
.NET 6.0 or higher
PostgreSQL (for database storage)
Installation
Clone the repository
Restore dependencies: dotnet restore
Update database connection string in RookAroundContext.cs if needed
Run the application: dotnet run