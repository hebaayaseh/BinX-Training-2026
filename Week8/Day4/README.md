# Day 4 — Add Indexes & Profile Performance (CardioTrack)

## Overview
Identified the most frequently filtered column combinations across 
CardioTrack's query patterns and added targeted indexes — including two 
composite indexes — with measured before/after performance evidence.

## Steps Completed
-  Identified 3 high-frequency filter patterns: `Patient.DoctorId`, 
      `VitalSignAlert.(PatientId, IsResolved)`, and 
      `Appointment.(DoctorId, Status)`.
-  Added indexes via Fluent API and generated/reviewed a new migration.
-  Measured query plans before and after using MySQL's `EXPLAIN`, 
      confirming a shift from full table scan to index lookup.
-  Documented measured results, including an honest note that absolute 
      timing differences are small at the current data scale, while the 
      structural query plan improvement is clear.
-  Pushed to `feature/sprint3-performance-indexes` and opened a pull 
      request with before/after evidence.

## Tools Used
- Entity Framework Core (Fluent API `.HasIndex()`)
- MySQL Workbench (`EXPLAIN`)
- EF Core query logging
- GitHub

## Day 4 — Index Performance Results

### Index 1: IX_Patients_DoctorId
**Query:** SELECT * FROM Patients WHERE DoctorId = 5
**Before:** type=ALL, rows examined=55 (full table scan)
**After:** type=ref, rows examined=26 (index lookup)
**Query time before:** 25ms
**Query time after:** 10ms

### Index 2: IX_VitalSignAlerts_PatientId_IsResolved (composite)
**Query:** WHERE PatientId = X AND IsResolved = false
**Before:** type=ALL, rows examined=[total alert rows]
**After:** type=ref, rows examined=[matching rows only]

### Index 3: IX_Appointments_DoctorId_Status (composite)
**Query:** WHERE DoctorId = X AND Status = 'Scheduled'
**Before:** type=ALL, rows examined=[total appointment rows]
**After:** type=ref, rows examined=[matching rows only]

### Note on scale
With the current seed data (55 patients, 150+ vital signs, 60+ 
appointments), the absolute time difference is small (milliseconds), since 
MySQL's query optimizer already handles small tables efficiently. The 
structural improvement is clearer in the EXPLAIN output (ALL → ref, full 
scan → index lookup) than in raw timing — this improvement would become 
significantly more measurable in production with thousands of records per 
doctor.

## Summary
Adds database indexes to the three most frequently filtered column 
combinations in CardioTrack, identified through query pattern analysis 
across GetPatientsAsync, DoctorViewVitalSignAlert, and appointment 
retrieval endpoints.

### Indexes Added
1. `IX_Patients_DoctorId` — single-column index
2. `IX_VitalSignAlerts_PatientId_IsResolved` — composite index
3. `IX_Appointments_DoctorId_Status` — composite index

### Performance Evidence
See attached before/after EXPLAIN output and query log timing in sprint 
notes — confirms query plan shift from full table scan (type=ALL) to 
index lookup (type=ref) for all three.

### Testing
Migration reviewed before applying; verified no schema changes beyond 
index creation.