/* api.js — one shared helper that every page uses to get data.

   While USE_FAKE is true, the pages run on their own using the
   sample data below. Set it to false once the real API is ready,
   and the same function names will fetch from the server instead. */

const USE_FAKE = true;

/* ---- Sample data (only used while USE_FAKE is true) ---- */
const FAKE = {
    user: { username: "Sophia", role: "Admin" },
    stats: { totalCostumes: 40, available: 31, onHire: 9, overdue: 3 },
    /* NOTE: "era" is a display-only field for the catalogue subtitle.
       If the team wants it to be real, it must be added to the
       Costume model + database. For now it lives in the sample data. */
    costumes: [
        { costumeId: 101, name: "Victorian Ball Gown",  category: "Historical", era: "Victorian Era",  size: "M", pricePerDay: 1500, isAvailable: true  },
        { costumeId: 102, name: "Royal Court Ensemble", category: "Historical", era: "18th Century",   size: "L", pricePerDay: 1200, isAvailable: false },
        { costumeId: 103, name: "Masquerade Dress",     category: "Masquerade", era: "Renaissance",    size: "S", pricePerDay: 1300, isAvailable: true  },
        { costumeId: 104, name: "Vampire Cape",         category: "Fantasy",    era: "Gothic",         size: "L", pricePerDay:  800, isAvailable: true  },
        { costumeId: 105, name: "Pirate Captain",       category: "Theatre",    era: "Golden Age",     size: "M", pricePerDay:  350, isAvailable: false },
        { costumeId: 106, name: "Military Officer Uniform", category: "Historical", era: "19th Century", size: "L", pricePerDay: 1000, isAvailable: true },
        { costumeId: 107, name: "Opera Phantom Cloak",  category: "Theatre",    era: "Opera",          size: "M", pricePerDay: 1400, isAvailable: true  },
        { costumeId: 108, name: "Renaissance Gown",     category: "Historical", era: "Renaissance",    size: "S", pricePerDay: 1300, isAvailable: true  },
        { costumeId: 109, name: "Elven Warrior",        category: "Fantasy",    era: "Mythic",         size: "M", pricePerDay:  950, isAvailable: false },
        { costumeId: 110, name: "Film Noir Detective",  category: "Film",       era: "1940s",          size: "L", pricePerDay:  700, isAvailable: true  },
        { costumeId: 111, name: "Venetian Masque",      category: "Masquerade", era: "Baroque",        size: "M", pricePerDay: 1100, isAvailable: true  },
        { costumeId: 112, name: "Hollywood Starlet",    category: "Film",       era: "1950s",          size: "S", pricePerDay: 1250, isAvailable: true  }
    ],
    recentRentals: [
        { customerName: "Priya Ramjee", costumeName: "Pirate Captain",      dueDate: "2026-12-02", status: "Overdue"  },
        { customerName: "Emma Wilson",  costumeName: "Victorian Ball Gown", dueDate: "2026-12-10", status: "On Hire"  },
        { customerName: "James Carter", costumeName: "Royal Court Ensemble",dueDate: "2026-11-28", status: "Returned" }
    ],
    /* Active rentals (not yet returned) for the Reservations page. */
    activeRentals: [
        { rentalId: 1, customerName: "Priya Ramjee", costumeName: "Pirate Captain",      dueDate: "2026-11-15", status: "Overdue" },
        { rentalId: 2, customerName: "Emma Wilson",  costumeName: "Royal Court Ensemble", dueDate: "2026-12-10", status: "On Hire" },
        { rentalId: 3, customerName: "Daniel Brown", costumeName: "Elven Warrior",        dueDate: "2026-12-05", status: "On Hire" }
    ]
};

/* ---- GET: ask the server (or FAKE) for data ---- */
async function apiGet(url) {
    if (USE_FAKE) return fakeGet(url);

    const res = await fetch(url, { credentials: "same-origin" });
    if (res.status === 401) { window.location.href = "login.html"; throw new Error("Not logged in"); }
    if (!res.ok) throw new Error("Request failed");
    return res.json();
}

/* ---- POST: send data to the server (or pretend to) ---- */
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

/* ---- Fake router: returns sample data based on the URL ---- */
function fakeGet(url) {
    if (url.startsWith("/api/auth/me"))         return Promise.resolve(FAKE.user);
    if (url.startsWith("/api/dashboard/stats")) return Promise.resolve({ ...FAKE.stats, recentRentals: FAKE.recentRentals });
    if (url.startsWith("/api/costumes/search")) {
        const term = decodeURIComponent((url.split("term=")[1] || "")).toLowerCase();
        const hits = FAKE.costumes.filter(c =>
            c.name.toLowerCase().includes(term) || c.category.toLowerCase().includes(term));
        return Promise.resolve(hits);
    }
    if (url.startsWith("/api/costumes")) return Promise.resolve(FAKE.costumes);
    if (url.startsWith("/api/rentals"))  return Promise.resolve(FAKE.activeRentals);
    return Promise.resolve([]);
}

/* ---- Small helper: safely put text into HTML (stops broken layout
   if a name ever contains < or > characters). ---- */
function escapeHtml(s) {
    return String(s).replace(/[&<>"']/g, c => ({
        "&":"&amp;","<":"&lt;",">":"&gt;",'"':"&quot;","'":"&#39;"
    }[c]));
}

/* ---- Small helper: build a coloured status badge ----
   We keep the data value (status) but SHOW institutional wording,
   as the design brief asks ("Archive Ready" instead of "Available"). */
function statusBadge(status) {
    const key = status.toLowerCase().replace(/\s/g, "");
    const cls = key === "available" ? "available" : key === "overdue" ? "overdue" : "onhire";

    /* Map the plain status to prestigious archive language. */
    let label;
    if      (key === "available") label = "Archive Ready";
    else if (key === "overdue")   label = "Recall Due";
    else if (key === "returned")  label = "Returned";
    else                          label = "On Loan";   /* "On Hire" */

    return `<span class="badge badge--${cls}">${escapeHtml(label)}</span>`;
}
