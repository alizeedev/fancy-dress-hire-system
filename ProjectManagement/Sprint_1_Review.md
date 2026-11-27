# The Grand Atelier — Sprint 1 Review and Retrospective

**Module:** CST2550 — Group Coursework  
**Project:** The Grand Atelier  
**Sprint:** 1 — Foundations  
**Sprint period:** 17–24 November 2026  
**Review and retrospective:** Completed following Sprint 1   
**Attendees:** All team members 
**Record prepared:** 26 November 2026  
**Prepared by:** Alizée, Scrum Master  


## 1. Sprint goal

Establish an ASP.NET Core Web API foundation for The Grand Atelier, including the C# solution and core models, SQL Server/data-access foundations, cookie authentication, initial styled pages, and automated tests for core classes. 

## 2. Sprint deliverables and results

| Sprint deliverable | Result at sprint close | Supporting evidence |
|---|---|---|
| Core, API and Tests projects | Completed | [Issue #16](https://github.com/alizeedev/fancy-dress-hire-system/issues/16); [PR #15](https://github.com/alizeedev/fancy-dress-hire-system/pull/15) |
| Core C# models (Costume, Customer, Rental, User) | Completed | [Issue #17](https://github.com/alizeedev/fancy-dress-hire-system/issues/17); models committed on 19 Nov |
| Initial login and dashboard interface, shared stylesheet and JavaScript | Implemented | Frontend commits and [PR #34](https://github.com/alizeedev/fancy-dress-hire-system/pull/34); subsequent frontend updates through 23 Nov |
| Database script, connection class, user repository and password hashing | Implemented foundations | [Issue #18](https://github.com/alizeedev/fancy-dress-hire-system/issues/18); [database script](https://github.com/alizeedev/fancy-dress-hire-system/blob/main/database/grand_atelier.sql), `DatabaseConnection.cs`, `UserRepository.cs`, `PasswordHasher.cs` |
| Cookie authentication and login/logout/current-user endpoints | Implemented in source | [AuthController.cs](https://github.com/alizeedev/fancy-dress-hire-system/blob/main/FancyDressHireSystem/FancyDressHireSystem.Api/Controllers/AuthController.cs), associated 21–23 Nov commits |
| Initial automated unit tests | Test files committed | `CostumeTests.cs`, `CustomerTests.cs`, `RentalsTests.cs`, `UserTests.cs` and `PasswordHasherTests.cs` in the Tests project |
| Hash table interface and API contract | Contract documented | Repository `README.md`; **full custom hash table implementation continues in Sprint 2** |



## 3. Demonstration and quality assessment

- The Web API can start locally: the Scrum Master has successfully run the application at `http://localhost:5022` on macOS Monterey. This confirms successful startup in that environment as well.
- The login page, dashboard, shared styles and frontend interactions are present in the repository.
- The codebase includes authentication endpoints and several unit-test classes. A recorded full `dotnet test` result was not available for this review record.
- At the close of Sprint 1, the application was **not yet established as a fully integrated database-backed costume hire system**. The custom hash table, end-to-end workflows and further integration were scheduled for subsequent sprints.

## 4. Outstanding items and carry-over

| Item | Position at close-out | Follow-up |
|---|---|---|
| Custom hash table (`HashTable`, `HashNode`) and search algorithms | Interface/design contract existed; full implementation scheduled next | Anju — Sprint 2 |
| Remaining repositories, rental service and database-backed hire/return flows | Not yet demonstrated end-to-end | Preyashi — Sprint 2 |
| Unit-test execution evidence and hash-table edge cases | Test source exists; complete run evidence and new cases needed | Sakshee — Sprint 2 |
| Frontend/live API integration | Interface exists; some frontend paths used demonstration data | Sakshee with Anju and Preyashi — Sprints 2–3 |


## 5. Sprint retrospective

**What went well**

- The team moved from its earlier console-application idea to an ASP.NET Core Web API approach
- Multiple contributors delivered changes through Git commits and Pull Requests.
- Frontend development, database preparation and unit-test development progressed alongside the C# model work.
- The initial application could be launched locally, creating a starting point for further integration.

**Areas identified for improvement**

- Test runs and system demonstrations should be recorded explicitly, not inferred from committed files.
- Additional optional features should not distract from the core requirements and the approaching code-freeze deadline.

**Follow-up actions for Sprint 2**

| Action | Follow-up owner | Evidence of completion |
|---|---|---|
| Complete the custom hash table and search operations | Anju | PR, source code and search/collision tests |
| Implement the remaining SQL repositories and rental service | Preyashi | PR and successful API/database integration checks |
| Expand and run automated tests | Sakshee | `dotnet test` results, linked Issues and fixes |
| Keep the Sprint board, task assignments and meeting evidence consistent | Alizée with Faheem | Updated GitHub Project and documented records |
| Prioritise integration of core functionality over optional features | Whole team | Sprint 2/3 backlog and completed acceptance criteria |


## 6. Review conclusion

Sprint 1 delivered the core project structure, initial models, database and authentication foundations, frontend pages and early test coverage. The sprint established a working basis for the remaining development. The custom hash table, live database integration, complete rental workflows and further testing continue in the next sprints according to the updated five-sprint plan.

**Next sprint:** Sprint 2 — 25–30 November 2026.

## 7. Supporting records

- [GitHub repository](https://github.com/alizeedev/fancy-dress-hire-system)
- [GitHub Issues and task history](https://github.com/alizeedev/fancy-dress-hire-system/issues)
- [Meeting minutes](../docs/meeting-minutes/)
- `Grand_Atelier_Gantt.xlsx` (five-sprint planning baseline)
- [Sprint Plan](Sprint_Plan.md)
- [Risk and Blocker Log](Risk_and_Blocker_Log.md)
