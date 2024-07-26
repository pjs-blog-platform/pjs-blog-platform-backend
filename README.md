# PJS Blog Platform Backend

Welcome to the PJS Blog Platform Backend! This is the backend application for the PJS Blog Platform, built using .NET 8 with a microservices architecture.

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Architecture](#architecture)
- [Installation](#installation)
- [Development](#development)
- [License](#license)
- [Contact](#contact)

## Introduction

The PJS Blog Platform Backend provides the core API services for the PJS Blog Platform. It is designed using a microservices architecture with .NET 8, enabling scalable and maintainable services for user management, blog management, comments, and more.

## Features

- **Microservices Architecture**: Modular services for user management, blog management, and commenting.
- **Scalability**: Designed for high scalability and performance.
- **Data Persistence**: Integrated with databases for storing and managing data.
- **Logging and Monitoring**: Built-in logging and monitoring for effective maintenance and troubleshooting.

## Architecture

The backend is composed of several microservices:

- **User Service**: Handles user registration, authentication, and profile management.
- **Blog Service**: Manages blog posts, categories, and tags.
- **Comment Service**: Manages comments on blog posts.

## Installation

To get started with the project, clone the repository and restore the dependencies:

```bash
git clone https://github.com/pjs-blog-platform/pjs-blog-platform-backend.git
cd pjs-blog-platform-backend
dotnet restore
```

## Development
To start the development server, navigate to each microservice directory and run:
```bash
dotnet run
```

To build the application for production, use:
```bash
dotnet publish -c Release
```

This will create optimized production builds in the bin/Release/net8.0/publish directory for each service.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Contact

For any inquiries, feedback, or support, please contact us at:

- **Email**: pjorge.silvaa@gmail.com
- **Phone Number**: +351 913 955 007
