# Capstone API – Live Deployment & CI/CD

##  Overview

This project is the capstone API developed as part of the training program.
The API is built using **ASP.NET Core** and includes authentication, database integration, JWT-based authorization, and Redis support.

This lab focuses on deploying the API to a live environment and automating the deployment process using **GitHub Actions**.

---

##  Deployment

The API was manually deployed to a cloud hosting platform:

* **Platform:** Azure App Service / Railway
* **Environment:** Production
* **Deployment Type:** Manual deployment followed by automated CI/CD

###  Live API

**Live URL:**
`[ADD YOUR LIVE API URL HERE]`

The live URL can be used to verify that the API is publicly accessible.

---

##  Production Secrets

Production secrets are configured through the hosting platform's **Secrets / Environment Variables** management.

The following sensitive values are **not stored in the source code or GitHub repository**:

| Secret                                 | Description                                         |
| -------------------------------------- | --------------------------------------------------- |
| `ConnectionStrings__DefaultConnection` | Production database connection string               |
| `Jwt__Key`                             | Secret key used to generate and validate JWT tokens |
| `Redis__ConnectionString`              | Redis server connection string                      |

These values are configured directly in the production environment.

> ⚠️ Never commit production secrets, passwords, JWT keys, or connection strings to GitHub.

---

## ⚙ CI/CD Pipeline

The project uses **GitHub Actions** to automate the CI/CD process.

The workflow performs the following steps:

```text
Push / Pull Request
        ↓
    Build
        ↓
     Tests
        ↓
 Tests Passed?
    ↙       ↘
  No         Yes
  ↓           ↓
Stop       Deploy
              ↓
       Production API
```

### Pipeline Stages

1. **Build**

   * Restores dependencies.
   * Builds the ASP.NET Core API.
   * Ensures the project compiles successfully.

2. **Test**

   * Runs the automated test suite.
   * Deployment only continues if all tests pass.

3. **Deploy**

   * Runs only after successful build and tests.
   * Deploys the application to the production environment.

### Deployment Condition

The deployment job is configured to run only when:

* The workflow is triggered from the `main` branch.
* The build succeeds.
* All tests pass.

---

##  Testing the Deployment

After deployment, the live API was tested to confirm that it is reachable through the public URL.

A small change was pushed to the repository to verify the complete automated pipeline.

The expected pipeline execution is:

```text
Build → Test → Deploy
```

The successful workflow confirms that the application can be automatically deployed after passing the test stage.

---

##  Technologies Used

* **C#**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server**
* **JWT Authentication**
* **Redis**
* **Git & GitHub**
* **GitHub Actions**
* **Azure App Service / Railway**

---


