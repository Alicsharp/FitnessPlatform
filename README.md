# 🏋️ FitnessPlatform Enterprise API

![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)
![C#](https://img.shields.io/badge/C%23-12.0-purple.svg)
![Clean Architecture](https://img.shields.io/badge/Architecture-Clean-brightgreen.svg)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)

An enterprise-grade fitness management platform backend, built to demonstrate advanced software engineering practices, scalable architecture, and robust domain modeling.

## 🎯 Project Overview
FitnessPlatform is not just a standard CRUD application. It is designed to handle complex business rules and real-world enterprise challenges such as race conditions in booking systems, decoupled asynchronous processing, and automated background jobs.

## ✨ Key Enterprise Features

*   **🛡️ Optimistic Concurrency Control:** Implemented advanced concurrency tokens in EF Core to absolutely prevent double-booking in group classes, ensuring data integrity under heavy concurrent requests.
*   **📨 Event-Driven Gamification:** Utilized **RabbitMQ** to decouple the core workout session tracking from the gamification module. Achievements and badges are processed asynchronously via Message Bus.
*   **⏳ Automated Background Jobs:** Integrated **Quartz.NET** for reliable background processing, such as scanning and expiring user subscriptions automatically at midnight.
*   **⚡ High-Performance Caching:** Designed to leverage **Redis** for caching frequently accessed data (like active subscription plans) to minimize database load.
*   **💰 Robust Billing System:** A fully isolated module for handling user subscriptions, plan management, and session tracking.

## 🏗️ Architecture & Design Patterns

The system strictly adheres to **Clean Architecture** principles and **Domain-Driven Design (DDD)**, ensuring zero coupling between the domain logic and infrastructure concerns.

*   **CQRS (Command Query Responsibility Segregation):** Implemented using **MediatR** for isolating read and write operations.
*   **Rich Domain Model:** Business logic is encapsulated within the domain entities (e.g., `GroupClass`, `UserSubscription`), utilizing custom Domain Events and strictly controlled invariants.
*   **Exception Wrapping:** Database-specific exceptions (like `DbUpdateConcurrencyException`) are caught at the infrastructure layer and translated into pure domain/application exceptions.

## 🛠️ Technology Stack

*   **Framework:** .NET 8 Web API
*   **Language:** C# 12
*   **ORM:** Entity Framework Core
*   **Database:** SQL Server
*   **Message Broker:** RabbitMQ
*   **Background Service:** Quartz.NET
*   **Mediator Pattern:** MediatR

## 🚀 Getting Started

### Prerequisites
*   .NET 8 SDK
*   SQL Server
*   RabbitMQ (can be run via Docker)
*   Redis (can be run via Docker)

### Installation
1. Clone the repository:
   ```bash
   git clone [https://github.com/Alicsharp/FitnessPlatform.git](https://github.com/Alicsharp/FitnessPlatform.git)
