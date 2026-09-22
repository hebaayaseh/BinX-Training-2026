# CardioTrack — Mock Technical Q&A

## 1. Why did you use Redis caching?

We used Redis because some read operations can return the same data repeatedly, especially the doctor's patient list.

We implemented a cache-aside approach using `IDistributedCache`. The cached data has a 5-minute expiration, and the cache is explicitly invalidated when patient data is created or updated.

During the performance work, the measured cache response was approximately:

* Cache miss: **16 ms**
* Cache hit: **6 ms**

This showed a measurable improvement for repeated requests.

---

## 2. How did you identify the performance problems?

We first enabled EF Core query logging and tested the important list endpoints using realistic data:

* 55 patients
* 150+ vital signs
* 60+ appointments

We checked the actual SQL queries instead of assuming that an N+1 problem existed.

The investigation showed that the tested endpoints were already using a single query. The main issue we found was unnecessary data being loaded through an `Include`.

After removing the unnecessary `Include`, the query count remained **1 ? 1**, but the selected columns were reduced from approximately **12 ? 2** for that case.

We also added database indexes and verified the query plans using MySQL `EXPLAIN`.

---

## 3. How does authentication and authorization work in your API?

The API uses JWT authentication.

After login, the server generates a JWT containing the required claims, including the user's identity, role, and center information.

Authorization is then applied to protected endpoints based on roles and policies.

For example, different actors such as Doctor, Patient, Technician, Admin, and SuperAdmin have different access levels.

We also found a security issue during testing where inactive users could still log in because `User.IsActive` was not checked during login. We fixed the issue and added regression coverage for it.

---

## 4. How did you test the project?

We used xUnit for testing, together with mocking and integration testing.

During Sprint 4, we audited **60 API endpoints** and created tests around authentication, role protection, and important services.

Initially, the test suite had **27 failing tests**.

We investigated the failures and fixed issues including:

* Shared integration-test database configuration
* Redis being registered in the testing environment
* Missing default value for `User.PhoneNumber`
* JWT collisions during tests
* Overly strict exception assertions

After the fixes, the test suite reached **0 failing tests**, apart from one intentionally failing regression test used to document a known bug scenario.

---

## 5. Why did you use EF Core?

EF Core allowed us to work with the database using C# entities and LINQ instead of writing every database operation manually.

It also provided:

* Migrations
* Relationships
* Fluent API configuration
* LINQ queries
* Database schema management

We used it together with MySQL for the CardioTrack backend.

---

## 6. How would you improve the project in the future?

The next improvements would focus on completing the remaining deployment and documentation work, reviewing nullable warnings, and continuing to improve test coverage around important medical-data workflows.

I would also continue monitoring real production performance instead of relying only on local measurements.

---

## 7. What was the biggest technical challenge?

The biggest challenge was maintaining correctness while the API and its features were growing.

The project contains multiple roles, protected medical data, authentication, authorization, database relationships, caching, testing, and deployment concerns.

The testing sprint was especially useful because it exposed issues that were not obvious during normal development, including the inactive-user login problem and integration-test environment problems.

---

## 8. Why didn't you just use `IMemoryCache`?

`IMemoryCache` stores data inside the memory of one application instance.

Because the project can run on multiple servers/instances, a distributed cache such as Redis is more appropriate. Multiple application instances can access the same Redis cache.

That is why the performance implementation uses `IDistributedCache` with Redis.
