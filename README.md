# Movie App

A comprehensive movie management application built with .NET MAUI and ASP.NET Core Web API.

## Features

- User Management (Registration, Login, Profile Management)
- Movie Management
- Favorite Movies System
- Movie Rating System
- Advanced Movie Filtering and Search
- Secure Authentication and Authorization

## Technologies Used

- .NET MAUI
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- BCrypt Password Hashing

## Project Structure

- `MovieApp.Api`: Backend API project
- `MovieApp.Maui`: MAUI client application

## API Endpoints

### User Endpoints
- POST `/api/users/register` - Register new user
- POST `/api/users/login` - User login
- GET `/api/users` - List all users
- GET `/api/users/{id}` - Get specific user
- PUT `/api/users/{id}` - Update user
- DELETE `/api/users/{id}` - Delete user
- PUT `/api/users/update-password` - Update password

### Movie Endpoints
- GET `/api/movies` - List all movies
- POST `/api/movies` - Add new movie
- GET `/api/movies/{id}` - Get specific movie
- PUT `/api/movies/{id}` - Update movie
- DELETE `/api/movies/{id}` - Delete movie

### Favorite Movies Endpoints
- GET `/api/users/{userId}/favorites` - List user's favorite movies
- POST `/api/users/{userId}/favorites/{movieId}` - Add movie to favorites
- DELETE `/api/users/{userId}/favorites/{movieId}` - Remove movie from favorites

### Rating Endpoints
- GET `/api/movies/{movieId}/ratings` - Get movie ratings
- GET `/api/users/{userId}/ratings` - Get user's ratings
- POST `/api/users/{userId}/ratings` - Add new rating
- GET `/api/users/{userId}/ratings/{movieId}` - Get user's rating for movie
- PUT `/api/users/{userId}/ratings/{movieId}` - Update rating
- DELETE `/api/users/{userId}/ratings/{movieId}` - Delete rating

## Getting Started

1. Clone the repository
2. Set up the database
3. Configure the connection string in `appsettings.json`
4. Run the API project
5. Run the MAUI project

## Security

- JWT-based authentication
- Password hashing with BCrypt
- CORS protection
- Input validation
- Error handling

## License

This project is licensed under the MIT License. 