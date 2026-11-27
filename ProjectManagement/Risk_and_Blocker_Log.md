# The Grand Atelier — Risk and Blocker Log

**Project:** The Grand Atelier — CST2550  
**Prepared:** 26 November 2026  
**Maintained by:** Scrum Master (Alizée), with input from all team members  
**Status:** Active risk register, established 26 November 2026. Risk ratings are planning assessments; monitoring owners coordinate follow-up.

## 1. What this log means

- A **risk** is something that *might* happen and affect scope, schedule, cost, quality or submission (for example, SQL Server might not work on every team member's computer).
- A **blocker** is an *actual current obstacle* stopping a particular task (for example, a developer cannot test a feature because the required API endpoint is missing). A risk is not automatically an active blocker.
- **Likelihood** and **impact** use Low / Medium / High ratings. These are qualitative planning judgements, not measured probabilities.
- An **owner** monitors the item and follows up; assigning an owner does not imply they caused the problem.

## 2. Risks and issues requiring attention

| ID | Type | Risk / concern | Likelihood | Impact | Monitoring owner | Prevention / response | Current evidence / status |
|---|---|---|---|---|---|---|---|
| R-01 | Risk | **SQL Server integration differs across machines** (Windows LocalDB vs Intel Mac; Docker/SQL Server limitations) | High | High | Preyashi | Agree a shared test database or accessible development SQL Server; document connection strings per environment; verify end-to-end tests on at least two setups | Open risk. API starts locally, which does **not** verify database access |
| R-02 | Risk | **Hash-table implementation delays API integration** | Medium | High | Anju | Prioritise `HashTable`, `HashNode`, search tests and interface agreement before dependent endpoints; flag delays in standups | Gantt schedules custom implementation for Sprint 2; no implementation files seen on `main` at the last check |
| R-03 | Observed gap | **Frontend still uses demo authentication/sample-data mode** | High | High | Sakshee + Preyashi | Agree endpoint contract; switch fake mode off only after backend tests; verify login, logout, session and failures against actual SQL Server | `login.js` and `api.js` on `main` currently contain `USE_FAKE = true`; real auth controller exists |
| R-04 | Risk | **Too much optional scope before code freeze** (agent, reminders, late fees) | High | High | Alizée + development team | Protect core inventory/search/hire/return features; get lecturer approval for optional functionality; drop/defer extras if core work is incomplete | Gantt schedules optional agent alongside major integration in Sprint 4 |
| R-05 | Risk | **Merge conflicts or unreviewed changes disrupt delivery** | Medium | Medium | Entire team / Alizée | One feature branch per task; PR review; frequent pulls; small focused commits; no direct changes to final submitted version | Initial kickoff minutes identified Git/GitHub confidence as a concern; PR collaboration is being used |
| R-06 | Risk | **Insufficient system/unit testing before submission** | Medium | High | Sakshee + developers | Run `dotnet test`; cover hash collision/resizing and negative cases; record failures and retests as Issues | Model and password-hasher test files exist; current test-run evidence not supplied |
| R-07 | Risk | **Secrets or unsafe settings appear in the public repository** | Medium | High | Preyashi + whole team | Keep credentials in local secrets/environment variables, review `.gitignore` and PRs, publish sample config without real passwords | Coursework requires public repository; inspect before submission |


## 3. Active blocker register

**No current task-blocking incident**


