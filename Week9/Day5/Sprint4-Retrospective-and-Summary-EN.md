# Sprint 4 Retrospective & Summary

## Sprint Review — what was demonstrated

- Full test coverage audit across 60 endpoints, closing the top 5
  highest-risk gaps (role protection, auth endpoints, lab-request data
  isolation, staff account creation, account activation/deactivation)
- Discovered and fixed a real security bug: deactivated accounts could
  still log in
- Full documentation: XML docs + Swagger examples + Postman review +
  professional README
- CI pipeline via GitHub Actions: build → test automatically on every
  push/PR
- [Update once hosting is resolved] Automated deploy gated on tests passing

## Retrospective

### What went well
- _(Write here — e.g. "Prioritizing gaps by risk saved time; we didn't
  waste effort on low-value gaps first")_
- _(Write here — e.g. "Catching the `IsActive` bug early, before it
  reached production")_

### What needs improvement
- _(Write here — e.g. "Assuming CI/CD was ready without actually testing
  on a real hosting platform cost us a late discovery — the Railway
  payment requirement should have been checked at the start of the week,
  not the end")_
- _(Write here — e.g. "The nullable warnings and the ERD were both
  forgotten until the audit — the DoD checklist should be checked from
  the first sprint, not just the last")_

### The one action for Week 10

Since next week is entirely about presenting (not building), the
suggested action:

> **Prepare the demo assuming something will fail during the
> presentation — run through the full demo (from GitHub, to Swagger, to a
> real Postman request) at least twice before presentation day, not once.**

(Replace this with whatever action fits better based on how the sprint
actually went.)

---

## Sprint 4 Summary — ready to paste into Notion/Trello

### Sprint achievements

| Area | Achievement |
|---|---|
| Testing | Audited 60 endpoints, closed 5 high-risk gaps, 106/106 tests passing |
| Bug found | `AuthService.LoginAsync` wasn't checking `IsActive` — fixed |
| Documentation | XML docs, Swagger examples, full Postman review, professional README |
| CI | GitHub Actions build+test runs automatically; confirmed it fails correctly on a broken test |
| CD | Deploy job written and gated on `needs: build-and-test` — pending hosting resolution |
| ERD | _(update once completed)_ |
| Build warnings | _(update once completed)_ |

### Open items as of this summary

1. Live hosting is blocked — Railway is asking for a payment card;
   evaluating between adding a verification card or switching to Render.com
2. ERD not yet created
3. Nullable warnings not yet checked
