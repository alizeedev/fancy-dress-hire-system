/* dashboard.js — fills the dashboard with data and wires up
   the search bar and logout button. Uses api.js.
   Field names match the backend model classes. */

/* Run once the page has finished loading. */
document.addEventListener("DOMContentLoaded", init);

async function init() {

    /* 1. Who is logged in? Show their name in the nav and hero. */
    try {
        const me = await apiGet("/api/auth/me");
        document.getElementById("welcomeName").textContent = me.userName;
        document.getElementById("navUser").textContent = me.userName;
        document.getElementById("navAvatar").textContent = me.userName.charAt(0).toUpperCase();
    } catch (e) {
        return;   /* not logged in: api.js already sent us to login */
    }

    /* 2. Fill the four stat cards and the recent-rentals table. */
    try {
        const stats = await apiGet("/api/dashboard/stats");
        setText("statTotal", stats.totalCostumes);
        setText("statAvailable", stats.available);
        setText("statOnHire", stats.onHire);
        setText("statOverdue", stats.overdue);
        fillRecent(stats.recentRentals || []);
    } catch (e) {
        /* If data can't load, leave the dashes showing. */
    }

    /* 3. Search bar: click the button or press Enter. */
    document.getElementById("searchBtn").addEventListener("click", runSearch);
    document.getElementById("searchInput").addEventListener("keydown", function (e) {
        if (e.key === "Enter") runSearch();
    });

    /* 4. Logout is handled by logout.js (shows a confirmation modal). */
}

/* Helper: put a value into an element by its id. */
function setText(id, value) {
    document.getElementById(id).textContent = value;
}

/* Fill the "Recent Reservations" table with rows. */
function fillRecent(rentals) {
    const body = document.getElementById("recentBody");
    body.innerHTML = "";

    /* If there are none, show a friendly message instead of an empty table. */
    if (rentals.length === 0) {
        body.innerHTML = `<tr><td colspan="4" class="empty-state">No recent reservations.</td></tr>`;
        return;
    }

    /* Build one table row per rental. */
    rentals.forEach(function (r) {
        const row = document.createElement("tr");
        if (r.status === "Overdue") row.className = "is-overdue";   /* red tint */
        row.innerHTML =
            "<td>" + escapeHtml(r.customerName) + "</td>" +
            "<td>" + escapeHtml(r.costumeName) + "</td>" +
            "<td>" + escapeHtml(r.dueDate) + "</td>" +
            "<td>" + statusBadge(r.status) + "</td>";
        body.appendChild(row);
    });
}

/* Run a costume search and show the results. */
async function runSearch() {
    const term = document.getElementById("searchInput").value.trim();
    const box = document.getElementById("searchResults");
    box.innerHTML = "";

    if (term === "") return;   /* nothing typed: do nothing */

    try {
        const results = await apiGet("/api/costumes/search?term=" + encodeURIComponent(term));

        if (results.length === 0) {
            box.innerHTML = `<div class="empty-state">No costumes match "${escapeHtml(term)}".</div>`;
            return;
        }

        /* Build one result card per costume found. */
        results.forEach(function (c) {
            const item = document.createElement("div");
            item.className = "search-item";
            item.innerHTML =
                "<div>" +
                "<div class='search-item__name'>" + escapeHtml(c.costumeName) + "</div>" +
                "<div class='search-item__meta'>" + escapeHtml(c.category) +
                " · Size " + escapeHtml(c.size) +
                " · Rs " + c.dailyHirePrice + "/day</div>" +
                "</div>" +
                (isCostumeAvailable(c) ? statusBadge("Available") : statusBadge("On Hire"));
            box.appendChild(item);
        });
    } catch (e) {
        box.innerHTML = `<div class="empty-state">Could not run the search.</div>`;
    }
}