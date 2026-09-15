# Day 1 — Audit and Close Test Coverage Gaps

## Overview
Audited test coverage across every endpoint in CardioTrack, prioritized 
gaps by risk, and closed the 5 highest-priority gaps: login, token refresh, 
patient list retrieval, patient portal data isolation, and account 
deactivation edge cases.

## Coverage Audit Summary
- Fully covered: VitalSign alert evaluation, Appointment service, Medication 
  ownership, Medical history ownership
- Previously uncovered (now closed): AuthService, TokenService, 
  GetPatientsService, PatientService (portal), ActiveDeactiveActorService
- Still uncovered (Sprint 5 backlog): Admin staff management, doctor 
  schedule, appointment listing endpoints

## Gaps Closed This Sprint (by priority)
1. AuthService.LoginAsync — valid login, wrong password, unknown email
2. TokenService.RefreshAsync — expired token, revoked token, valid rotation
3. GetPatientsService — non-doctor rejection, empty result handling
4. PatientService (portal) — unlinked user rejection, data isolation between patients
5. ActiveDeactiveActorService — already-inactive account edge case

## Full Suite Status
All tests passing after this sprint's additions.

## Tools Used
- xUnit, Moq, EF Core InMemory provider