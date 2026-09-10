# Week 8 — Performance Diagnosis & Optimization (Sprint 3)

## Sprint Goal
Diagnose and address performance bottlenecks across CardioTrack's primary 
list endpoints — query efficiency, caching, and indexing — under realistic 
data volume, while incorporating the Sprint 2 retrospective action of 
writing authorization edge-case tests before extending endpoints further.

---

## Day 1 — Enable Logging & Diagnose N+1

### What Was Done
- Enabled EF Core query logging in development (`LogTo`, 
  `EnableSensitiveDataLogging`).
- Seeded the database with realistic volume: 55 patients, 150+ vital 
  signs, 60+ appointments.
- Ran the three most important list endpoints (`get-patients`, 
  `doctor-view-vitalsignalert`, appointment retrieval) and counted actual 
  queries generated for each.

### Finding
No genuine N+1 problem (query count scaling with result size) was found — 
all tested endpoints generated a constant single query. One real issue was 
identified instead: `GetAppointmentToNurseAsync` used an unnecessary 
`.Include(d => d.Doctor)` despite the projected DTO never using any Doctor 
field, fetching ~12 unused columns per row.

---

## Day 2 — Fix Issues & Measure Improvement

### What Was Done
- Removed the unused `.Include(d => d.Doctor)` from `GetAppointmentToNurseAsync`.
- Compared an `.Include()`-based query against a projection-only version — 
  confirmed both generate an equivalent single JOIN when the `Select` 
  already uses navigation property fields directly.
- Audited all list endpoints for 2+ simultaneous collection navigation 
  properties (`AsSplitQuery` candidates) — none found; documented as a 
  guideline for future multi-collection endpoints.

### Result
- Query count: unchanged (1 → 1) — this was a data-volume fix, not a 
  query-count fix.
- Columns fetched per row: reduced from ~12 to 2.

---

## Day 3 — Cache the Patients List Endpoint

### What Was Done
- Set up Redis locally via Docker and registered `IDistributedCache` 
  using `StackExchangeRedisCache`.
- Implemented cache-aside caching for `GetPatientsAsync`, with the cache 
  key scoped per doctor (`patients:doctor:{doctorId}`) rather than per full 
  query combination, since `IDistributedCache` doesn't support 
  pattern-based key deletion. Filtering, sorting, and pagination are 
  applied in memory after retrieval.
- 5-minute expiration; explicit cache invalidation on any patient 
  create/update.
- Verified that updating a patient is reflected immediately on the next 
  request, not after expiration.

### Result
- Cache Miss: 16 ms
- Cache Hit: 6 ms
- Invalidation confirmed working correctly in manual testing.

---

## Day 4 — Add Indexes & Profile Performance

### What Was Done
- Identified 3 frequently filtered column combinations from actual query 
  patterns: `Patient.DoctorId`, `VitalSignAlert.(PatientId, IsResolved)`, 
  `Appointment.(DoctorId, Status)`.
- Added indexes via Fluent API, including two composite indexes.
- Resolved two migration issues along the way:
  - A `DROP INDEX` failure on `Vital-Sign-Alerts.PatientId` because MySQL 
    required it for its foreign key constraint — fixed by creating the new 
    composite index first, then dropping the old one.
  - A `Specified key was too long` error on `Appointments.Status`, caused 
    by the column being stored as `longtext` instead of a bounded type — 
    fixed by reordering the migration to alter the column to `varchar(255)` 
    before creating the index.
- Measured query plans before/after using MySQL's `EXPLAIN`.

### Result
- Query plan shifted from `type=ALL` (full table scan) to `type=ref` 
  (index lookup) for all three indexes.
- Discovered and corrected a pre-existing schema issue (`longtext` column 
  used for a short enum-backed value) as a side effect of this work.

---

## Sprint 3 Backlog Status

| Task | Status |
|---|---|
| Query logging enabled | ✅ |
| Realistic seed data (50+ rows) | ✅ |
| N+1 diagnosis | ✅ (found over-fetching instead) |
| Over-fetching fix | ✅ |
| Redis caching implemented | ✅ |
| Cache invalidation verified | ✅ |
| Composite indexes added | ✅ |
| Index performance measured | ✅ |

## Sprint 4 Backlog (carried forward)
- Audit remaining enum-backed string columns for the same `longtext` issue
- Revisit caching strategy if per-doctor patient volume grows significantly
- Extend caching to other frequently-read, rarely-changed endpoints
- Add indexes on date columns if date-range filtering is added later

## Tools Used
- Entity Framework Core (query logging, Fluent API indexing)
- Redis (Docker) + StackExchange.Redis
- MySQL Workbench (`EXPLAIN`)
- GitHub
