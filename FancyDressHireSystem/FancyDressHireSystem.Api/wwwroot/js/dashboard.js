/* dashboard.js — fills the dashboard with data.
   The dashboard is a pure overview: stats, recent reservations
   and quick actions. The costume search lives on the Collection
   page, so there is no search box here any more.
   Field names match the backend model classes. */

document.addEventListener("DOMContentLoaded", init);

async function init() {

    /* 1. Who is logged in? Fill the sidebar chip and the hero. */
    try {
        const me = await apiGet("/api/auth/me");
        fillSidebar(me);
        document.getElementById("welcomeName").textContent = me.userName;
    } catch (e) {
        return;   /* not logged in: api.js already sent us to login */
    }

    /* 2. Show today's date in the topbar. */
    document.getElementById("todayDate").textContent =
        new Date().toLocaleDateString("en-GB", { weekday: "long", day: "numeric", month: "long", year: "numeric" });

    /* 3. Fill the four stat cards and the recent-rentals table. */
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

    /* 4. Logout is handled by logout.js (shows a confirmation modal). */
}

/* Shared helper: fill the sidebar profile chip (used on every page). */
function fillSidebar(me) {
    const user = document.getElementById("sideUser");
    const avatar = document.getElementById("sideAvatar");
    const role = document.getElementById("sideRole");
    if (user) user.textContent = me.userName;
    if (avatar) avatar.textContent = me.userName.charAt(0).toUpperCase();
    if (role && me.userRole) role.textContent = me.userRole;
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