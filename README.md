# 📸 InstaCore — Instagram-Style REST API

InstaCore is a clean and modular Instagram-style backend API built with ASP.NET Core, Entity Framework Core, and JWT Authentication.
The goal of the project is to focus on clean architecture, global error handling, and modular service layers, while implementing real social-network features.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

## 🧰 Technologies Used
- ASP.NET Core 9
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- BCrypt Password Hashing
- Swagger / OpenAPI
- Global exception middleware

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
## 🚀 Features Implemented

### ✅ Authentication & Authorization
- JWT Bearer tokens
- Login / Register
- Secure protected endpoints
- Password hashing using BCrypt

### ✅ Users
- View your own profile (/users/me)
- View any public profile (/users/{username})
- Update your profile (/users/me)
- Get follower / following counts

### ✅ Follows
- Follow another user
- Unfollow another user
- Idempotent operations (safe to repeat)
- Full validation & error responses

### ✅ Posts
- Create a post (caption + optional image URL)
- Fetch post by ID
- (Coming soon) User posts, feed, comments, likes
