# 🔐 SecureAuth

**SecureAuth** is a secure and extensible **ASP.NET Core Web API** project focused on modern **authentication and authorization** workflows.  
It supports **email verification before signup**, **sign in**, **sign out**, **JWT**, **refresh tokens**, **role-based authorization**, **policy-based authorization**, and asynchronous email delivery using a **background service**.

The project is built with Clean Architecture in mind, with a structure designed for scalability, maintainability, and future expansion. It also applies the Result Pattern to represent operation outcomes in a consistent way, avoiding exception-driven control flow for expected cases and making success/failure handling more explicit, predictable, and easier to maintain.


## ✨ Features

- **Email verification before signup**
- **User signup**
- **User signin**
- **User signout**
- **JWT access token**
- **Refresh token**
- **Role-based authorization**
- **Policy-based authorization**
- **Background service for sending emails**
- **Clean Architecture**
- **Dependency Injection**
- **Interface-based communication between layers**
- **Modular structure using Class Library projects**


## 📌 Project Idea

The project is designed as a more realistic backend foundation that can be expanded over time.

Some parts of the project may look more layered or abstract than a small app strictly needs, but that is intentional. The structure was chosen to support:

- clean separation of concerns
- loose coupling
- easier testing
- better maintainability
- simpler future extension


## 🏛 Architecture

The project follows **Clean Architecture** principles.

### Main idea

- There is **one startup project**
- The remaining parts are separated into **Class Library** projects
- The layers communicate through **interfaces**
- Dependencies are managed via **Dependency Injection**

This approach helps keep the codebase modular and easier to scale.


## ⚙️ Technologies Used

- **ASP.NET Core Web API**
- **C#**
- **Entity Framework Core**
- **Background Services**
- **Email Service / SMTP**
- **Dependency Injection**


## 🔐 Authentication Flow

The registration flow in this project starts with **email verification**.

### 1. Email Verification
Before a user can complete signup, the email address must be verified first.

### 2. Signup
After email verification is completed, the user can finish the signup process and create an account.

### 3. Signin
A registered user can sign in and receive:

- **Access Token**
- **Refresh Token**

### 5. Signout
The user can sign out and the authentication token lifecycle is handled accordingly.


## 📧 Email Delivery

The project uses a **Background Service** to send emails.

This provides several benefits:

- faster API response time
- email sending is separated from the request pipeline
- better scalability
- cleaner asynchronous processing

This is especially useful for verification-related email workflows.


### Role-based Authorization
Access can be controlled based on roles such as:

- `Admin`
- `Moderator`
- `User`

### Policy-based Authorization
Policies are used for more advanced and fine-grained authorization rules.

This makes the project suitable for real applications where access rules may become more complex over time.


## 🧩 Why So Many Layers?

A project like this may initially look more structured than necessary, but that is intentional.

The goal is to make the system:

- easier to maintain
- easier to test
- easier to scale
- easier to extend with new features

Using interfaces, abstractions, and DI helps keep the project flexible and reduces tight coupling between modules.


## 📌 Example Capabilities

- `POST /api/auth/email-verification/otp`
- `POST /api/auth/email-verification/verify`
- `POST /api/auth/signup`
- `POST /api/auth/signin`
- `POST /api/auth/signout`
- `POST /api/auth/refresh-token`

---

## 🚀 Future Improvements

The project is structured in a way that makes it easier to add features such as:

- password reset
- forgot password
- audit logging
- caching
- unit tests
- integration tests
- Docker support


## 📈 Project Philosophy

SecureAuth is built around these principles:

- **security first**
- **clean structure**
- **extensibility**
- **maintainability**
- **clear separation of concerns**
- **real-world scalability mindset**

## 👨‍💻 Author

Developed by **build-infinity**
