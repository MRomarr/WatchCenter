# 📌 WatchCenter

WatchCenter is a .NET Core Clean Architecture** backend project for managing movies, series, and related content efficiently. It is built with scalability and  maintainability

---

## 🚀 Features

- 🎬 **Content Management**: Add, update, delete, and retrieve movies or series.  
- 🏷 **Genres**: Associate content with one or multiple genres.  
- 🔐 **Authentication & Authorization** (JWT-based).  
- 🗂 **Separation of Concerns** using Clean Architecture principles.  
- 📄 **RESTful API** for frontend or mobile applications.  
- 🛠 **Dependency Injection** for modular design.  
- 🧪 Unit test-ready architecture.  

---

## 🏗 Project Structure

This project follows **Clean Architecture** with the following layers:
- WatchCenter.API # Entry point for the application (controllers, endpoints, middleware)
- WatchCenter.Application # Business logic, DTOs, service and interfaces
- WatchCenter.Domain # Core entities
- WatchCenter.Infrastructure # Data access (EF Core, repositories), external services


---

## 🛠 Technologies Used

- **.NET 9** 
- **Entity Framework Core**
- **SQL Server**
- **JWT Authentication**
- **Clean Architecture**
- **Automapper** 

---

ERD

<img width="1385" height="718" alt="image" src="https://github.com/user-attachments/assets/f1e74de6-624a-4e80-bbc9-740d5c51f54f" />

