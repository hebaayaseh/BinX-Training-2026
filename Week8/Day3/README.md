#  Day 3 — Cache the Patients List Endpoint (CardioTrack)

## Overview
Implemented Redis-based caching for the doctor's patient list endpoint 
using the cache-aside pattern, with explicit invalidation on any patient 
create/update, and measured the performance difference between cache miss 
and cache hit.

## Steps Completed
-  Set up Redis locally via Docker and registered `IDistributedCache` 
      using `StackExchangeRedisCache`.
-  Implemented cache-aside caching for `GetPatientsAsync`, scoped per 
      doctor, with a 5-minute expiration.
-  Invalidated the doctor-scoped cache key whenever a patient is 
      created or updated.
-  Verified that updating a patient is reflected immediately on the 
      next GET request, not after cache expiration.
-  Measured and documented response time for cache miss vs cache hit.

## Design Decision: Cache Key Scope
Unlike a typical public catalog, patient lists are doctor-specific. The 
cache key includes the doctor's ID (`patients:doctor:{doctorId}`) rather 
than being global, since `IDistributedCache` doesn't support pattern-based 
key deletion — a single per-doctor key keeps invalidation simple and 
correct.

## Tools Used
- Redis (Docker)
- StackExchange.Redis / Microsoft.Extensions.Caching.StackExchangeRedis
- `IDistributedCache`

# Redis Caching Results

### Setup
- Redis running locally via Docker (redis:latest, port 6379)
- IDistributedCache registered via StackExchangeRedisCache
- Cache-aside pattern applied to GetPatientsAsync
- Cache key scoped per doctor: `patients:doctor:{doctorId}`
- Expiration: 5 minutes (AbsoluteExpirationRelativeToNow)

### Cache Invalidation
Applied on AddPatientAsync and any patient update/deactivate operation — 
removes the doctor-scoped cache key immediately after SaveChangesAsync.

### Measured Response Times
| Scenario | Response Time |
|---|---|
| Cache Miss (first call, DB query) | ~ 16ms |
| Cache Hit (second call, from Redis) | ~ 6ms |
| Improvement | ~62.5% faster |

### Cache Freshness Test
Updated a patient's phone number, immediately called GET /get-patients — 
confirmed the response reflected the new value instantly (not after the 
5-minute expiration), verifying cache invalidation works correctly.