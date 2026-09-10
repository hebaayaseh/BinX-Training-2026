# Day 5 - Retrospective — CardioTrack

### What went well
- Investigated N+1 rigorously using actual query logs rather than assuming 
  a problem existed — found the real issue was over-fetching via an unused 
  .Include(), not N+1 itself, and documented that distinction honestly 
  instead of forcing the lab's framing onto findings that didn't match.
- Redis cache-aside implementation correctly scoped cache keys per doctor 
  and verified invalidation works immediately on data changes, not just 
  after expiration — tested this explicitly rather than assuming it.
- Composite indexes were chosen based on actual query patterns observed in 
  the codebase (PatientId+IsResolved, DoctorId+Status), not arbitrary columns.

### What to improve
- Two migration failures this sprint (FK-dependent index drop order, and 
  MySQL's 3072-byte key length limit on a longtext column) revealed that 
  EF Core's auto-generated migrations for index changes need manual review 
  and reordering before applying — this wasn't anticipated ahead of time.
- The longtext-instead-of-varchar issue on Appointment.Status was an 
  existing schema flaw only discovered because indexing forced it to 
  surface — a reminder that schema review should happen proactively, not 
  just reactively when an operation fails.
- Performance measurements at the current seed data scale (dozens to low 
  hundreds of rows) show structural improvement (query plan, data volume) 
  more clearly than raw timing differences — worth being upfront about this 
  limitation rather than overstating results.

### One concrete action for Sprint 4
Before adding any new indexed column or enum-backed string property, apply 
HasMaxLength explicitly from the start, and manually review any 
auto-generated migration that touches an existing index or foreign-key- 
dependent column before running Update-Database — catching these issues at 
review time instead of at migration-failure time.

# Sprint 3 Summary — CardioTrack

## Sprint Goal
Diagnose and address performance bottlenecks across CardioTrack's primary 
list endpoints — query efficiency, caching, and indexing — under realistic 
data volume, while incorporating the Sprint 2 retrospective action of 
writing authorization edge-case tests before extending endpoints.

## Before/After Measurements

### Query Logging & N+1 (Day 1-2)
- No genuine N+1 found; real issue was unused `.Include(d => d.Doctor)` in 
  `GetAppointmentToNurseAsync`
- Before: ~12 columns fetched per row (Appointment + full Doctor entity)
- After: 2 columns fetched per row (Id, AppointmentDate only)
- Query count unchanged (1 → 1) — this was a data-volume fix, not a 
  query-count fix

### Caching (Day 3)
- Redis cache-aside applied to `GetPatientsAsync`, scoped per doctor
- Cache Miss: 16 ms | Cache Hit: 6ms
- Invalidation verified: patient update reflected immediately on next request

### Indexing (Day 4)
- 3 indexes added: `Patients.DoctorId`, `VitalSignAlerts.(PatientId, 
  IsResolved)`, `Appointments.(DoctorId, Status)`
- EXPLAIN confirmed query plan shift: type=ALL (full scan) → type=ref 
  (index lookup) for all three
- Discovered and fixed a pre-existing schema issue: `Appointment.Status` 
  was stored as `longtext` instead of a bounded `varchar`

## Caching Strategy
Cache-aside pattern with `IDistributedCache` (Redis). Cache key scoped per 
doctor (`patients:doctor:{doctorId}`) rather than per full query 
combination, since `IDistributedCache` doesn't support pattern-based key 
deletion — filtering/sorting/pagination applied in memory after 
cache/DB retrieval. 5-minute expiration; explicit invalidation on any 
patient create/update.
