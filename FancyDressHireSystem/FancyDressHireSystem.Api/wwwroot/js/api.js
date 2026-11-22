/* api.js — one shared helper that every page uses to get data.

   While USE_FAKE is true, the pages run on their own using the
   sample data below. Set it to false once the real API is ready,
   and the same function names will fetch from the server instead.
 */

const USE_FAKE = true;

/* ---- Sample data (only used while USE_FAKE is true) ----
   Fields match the backend: CostumeId, CostumeName, Category, Era,
   Size, DailyHirePrice, TotalQuantity, AvailableQuantity.
   Category must be Royal, Historical or Masquerade (backend rule). */
const FAKE = {
    user: { userName: "Sophia", userRole: "Admin" },
    stats: { totalCostumes: 40, available: 31, onHire: 9, overdue: 3 },
    costumes: [
        { costumeId: "GA001", costumeName: "Victorian Ball Gown", category: "Historical", era: "Victorian Era", size: "M", dailyHirePrice: 1500, totalQuantity: 3, availableQuantity: 2 },
        { costumeId: "GA002", costumeName: "Royal Court Ensemble", category: "Royal", era: "18th Century", size: "L", dailyHirePrice: 1200, totalQuantity: 2, availableQuantity: 0 },
        { costumeId: "GA003", costumeName: "Masquerade Dress", category: "Masquerade", era: "Renaissance", size: "S", dailyHirePrice: 1300, totalQuantity: 4, availableQuantity: 3 },
        { costumeId: "GA004", costumeName: "Velvet Court Cape", category: "Royal", era: "Baroque", size: "L", dailyHirePrice: 800, totalQuantity: 2, availableQuantity: 1 },
        { costumeId: "GA005", costumeName: "Highwayman Coat", category: "Historical", era: "Georgian", size: "M", dailyHirePrice: 350, totalQuantity: 2, availableQuantity: 0 },
        { costumeId: "GA006", costumeName: "Military Officer Dress", category: "Historical", era: "19th Century", size: "L", dailyHirePrice: 1000, totalQuantity: 3, availableQuantity: 2 },
        { costumeId: "GA007", costumeName: "Venetian Masque Gown", category: "Masquerade", era: "Baroque", size: "M", dailyHirePrice: 1400, totalQuantity: 2, availableQuantity: 2 },
        { costumeId: "GA008", costumeName: "Renaissance Gown", category: "Historical", era: "Renaissance", size: "S", dailyHirePrice: 1300, totalQuantity: 3, availableQuantity: 1 },
        { costumeId: "GA009", costumeName: "Royal Coronation Robe", category: "Royal", era: "Tudor", size: "M", dailyHirePrice: 950, totalQuantity: 2, availableQuantity: 0 },
        { costumeId: "GA010", costumeName: "Gilded Masque Suit", category: "Masquerade", era: "Rococo", size: "L", dailyHirePrice: 700, totalQuantity: 2, availableQuantity: 2 },
        { costumeId: "GA011", costumeName: "Empire Ballgown", category: "Historical", era: "Regency", size: "M", dailyHirePrice: 1100, totalQuantity: 3, availableQuantity: 2 },
        { costumeId: "GA012", costumeName: "Royal Hunt Ensemble", category: "Royal", era: "Stuart", size: "S", dailyHirePrice: 1250, totalQuantity: 2, availableQuantity: 1 }
    ],
    /* Sample customers for the hire-form dropdown (GET /api/customers). */
    customers: [
        { customerId: "C001", fullName: "Emma Wilson" },
        { customerId: "C002", fullName: "James Carter" },
        { customerId: "C003", fullName: "Priya Ramjee" },
        { customerId: "C004", fullName: "Daniel Brown" },
        { customerId: "C005", fullName: "Olivia Smith" }
    ],
    recentRentals: [
        { customerName: "Priya Ramjee", costumeName: "Highwayman Coat", dueDate: "2026-12-02", status: "Overdue" },
        { customerName: "Emma Wilson", costumeName: "Victorian Ball Gown", dueDate: "2026-12-10", status: "On Hire" },
        { customerName: "James Carter", costumeName: "Royal Court Ensemble", dueDate: "2026-11-28", status: "Returned" }
    ],
    /* Active rentals (not yet returned) for the Reservations page. */
    activeRentals: [
        { rentalId: "R001", customerName: "Priya Ramjee", costumeName: "Highwayman Coat", dueDate: "2026-11-15", status: "Overdue" },
        { rentalId: "R002", customerName: "Emma Wilson", costumeName: "Royal Court Ensemble", dueDate: "2026-12-10", status: "On Hire" },
        { rentalId: "R003", customerName: "Daniel Brown", costumeName: "Royal Coronation Robe", dueDate: "2026-12-05", status: "On Hire" }
    ]
};

/*  GET: ask the server (or FAKE) for data  */
async function apiGet(url) {
    if (USE_FAKE) return fakeGet(url);

    const res = await fetch(url, { credentials: "same-origin" });
    if (res.status === 401) { window.location.href = "login.html"; throw new Error("Not logged in"); }
    if (!res.ok) throw new Error("Request failed");
    return res.json();
}

/* POST: send data to the server (or pretend to) */
async function apiPost(url, body) {
    if (USE_FAKE) return { ok: true };

    const res = await fetch(url, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "same-origin",
        body: JSON.stringify(body)
    });
    if (res.status === 401) { window.location.href = "login.html"; throw new Error("Not logged in"); }
    if (!res.ok) throw new Error("Request failed");
    return res.json();
}

/* Fake router: returns sample data based on the URL */
function fakeGet(url) {
    if (url.startsWith("/api/auth/me")) return Promise.resolve(FAKE.user);
    if (url.startsWith("/api/dashboard/stats")) return Promise.resolve({ ...FAKE.stats, recentRentals: FAKE.recentRentals });
    if (url.startsWith("/api/costumes/search")) {
        const term = decodeURIComponent((url.split("term=")[1] || "")).toLowerCase();
        const hits = FAKE.costumes.filter(c =>
            c.costumeName.toLowerCase().includes(term) || c.category.toLowerCase().includes(term));
        return Promise.resolve(hits);
    }
    if (url.startsWith("/api/costumes")) return Promise.resolve(FAKE.costumes);
    if (url.startsWith("/api/customers")) return Promise.resolve(FAKE.customers);
    if (url.startsWith("/api/rentals/active")) return Promise.resolve(FAKE.activeRentals);
    return Promise.resolve([]);
}

/* ---- Small helper: safely put text into HTML (stops broken layout
   if a name ever contains < or > characters). ---- */
function escapeHtml(s) {
    return String(s).replace(/[&<>"']/g, c => ({
        "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#39;"
    }[c]));
}

/* ---- Small helper: build a coloured status badge ----*/
function statusBadge(status) {
    const key = status.toLowerCase().replace(/\s/g, "");
    const cls = key === "available" ? "available" : key === "overdue" ? "overdue" : "onhire";

    /* Map the plain status to prestigious archive language. */
    let label;
    if (key === "available") label = "Archive Ready";
    else if (key === "overdue") label = "Recall Due";
    else if (key === "returned") label = "Returned";
    else label = "On Loan";   /* "On Hire" */

    return `<span class="badge badge--${cls}">${escapeHtml(label)}</span>`;
}

/*
   The backend uses AvailableQuantity (a number). A costume is
   available when at least one is free, matching the backend's
   IsAvailable rule. */
function isCostumeAvailable(c) {
    return c.availableQuantity > 0;
}