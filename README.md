## Fancy Dress Hire System
## Hash Table

We wrote our own generic `HashTable<TKey, TValue>` because the brief doesn't allow List or Dictionary for storage.

There are three tables, one each for costumes, customers and rentals. Each one is keyed by its ID:

- Costumes: CostumeId, e.g. "GA001"
- Customers: CustomerId, e.g. "C001"
- Rentals: RentalId, e.g. "R001"

IDs are strings. The database is the master copy, and the repositories load everything into the tables once when the app starts.

### Methods

- `Add(key, value)`: adds an item. Throws if the key already exists.
- `Search(key)`: returns the item, or null if it isn't there.
- `Remove(key)`: returns true if it removed something, false if the key wasn't found.
- `GetAll()`: returns all items as an array (empty if there's nothing in it).
- `Count`: the number of items.
- `SearchByName(term)`: costumes whose name contains the term. It ignores case.
- `SearchByCategory(category)`: costumes in that category (Royal, Historical or Masquerade).

### Implementation

It uses separate chaining. Each bucket is a chain of our own `HashNode` objects (we don't use C#'s `LinkedList`). It starts with 16 buckets and doubles in size when the load factor reaches 0.75.

### Time complexity

| Operation | Average | Worst |
|---|---|---|
| Add, Search, Remove | O(1) | O(n) |
| GetAll | O(n) | O(n) |
| SearchByName, SearchByCategory | O(n) | O(n) |
| Loading from the DB at startup | O(n) | O(n) |

## Auth Endpoints

We use cookie authentication. If you aren't logged in, API calls return 401 instead of redirecting.

`POST /api/auth/login`

Request:
```json
{ "username": "admin", "password": "yourpassword" }
```
Response (200):
```json
{ "username": "admin", "role": "Admin" }
```
Empty fields return 400. A wrong username or password returns 401 with:
```json
{ "message": "Invalid username or password" }
```
The message is the same for both cases, so it doesn't give away which usernames exist.

`POST /api/auth/logout`: clears the cookie and returns 200.

`GET /api/auth/me`: returns the same JSON as login if you're logged in, otherwise 401.

`role` is either "Admin" or "Staff". The hash and salt are never sent back.