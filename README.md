# Media-Sharing-Platform

A full-featured web platform for uploading, sharing, and managing media content (videos & audios)
The goal of this project is to create a community-driven media platform where people can share their experiences, ideas, and creativity through videos and audios — connecting individuals through the power of media.




## Key Features

- Secure Authentication & Authorization using ASP.NET Identity  
- Complete Account Management: email confirmation, password reset, and two-factor authentication  
- Editable user profiles (name, email, and profile picture)  
- Dynamic Admin Dashboard for managing users, roles, and permissions  
- Email service integration (SendGrid) for automated notifications and verifications  
- Media management system — users can upload, view, and delete their media content (videos & audios)  
- Personalized user experience with favorites and watch history tracking  
- User-to-user interaction through public profiles and shared media  
- Fully responsive design and structured architecture for scalability and maintainability




## Tech Stack

- **Backend:** ASP.NET Core, Entity Framework Core
- **Frontend:** HTML, CSS, Razor Views
- **Database:** SQL Server 
- **Deployment:** Docker, Docker Compose




## Installation 

Follow these steps to set up and run the project locally using Docker:



---


### 1️⃣ Clone the repository

```
git clone https://github.com/abderhmansherif/Media-Sharing-Platform.git
cd Media-Sharing-Platform
```

### 2️⃣ Create the configuration file

Before running the app, you need to create a file named **`appsettings.json`**  
in the project root directory to include your API keys and configuration details.

Example:
```
{
  "CloudinarySettings": {
    "CloudName ": "your-cloud-name",
    "ApiKey ": "your-api-key",
    "ApiSecret ": "your-api-secret"
  },
  "SendGridConfig": {
    "ApiKey": "your-sendgrid-api-key"
  },
 
}
```

### 3️⃣ Run the application using Docker Compose

The project already includes a **Dockerfile** and **docker-compose.yml** for containerized deployment.

Run:
```
docker-compose up --build

```

- Build the application image (beatbox:v1.0)
- Start the SQL Server container (app-db)
- Launch the web application container (app)
- Once the build is complete, open your browser at:
👉 http://localhost:8080


## Usage

After starting the containers:

Access the app via http://localhost:8080

Register or log in as a user

Upload and share videos or audios

Explore other users’ profiles and media

(Admin only) Manage users, roles, and media content through the admin panel



