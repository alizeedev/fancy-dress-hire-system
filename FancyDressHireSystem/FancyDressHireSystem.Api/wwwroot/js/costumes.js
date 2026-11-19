/* costumes.js — the Costume Catalogue page.
   Loads all costumes, shows them as cards, and lets the user
   filter by category (the tabs) and search by text (the box). */

/* We keep the full list here once loaded, so filtering is instant
   and doesn't need to ask the server again every keystroke. */
let allCostumes = [];
let activeCategory = "All";

document.addEventListener("DOMContentLoaded", init);

async function init() {

    /* Show who is logged in, in the nav. */
    try {
        const me = await apiGet("/api/auth/me");
        document.getElementById("navUser").textContent   = me.username;
        document.getElementById("navAvatar").textContent = me.username.charAt(0).toUpperCase();
    } catch (e) { return; }

    /* Load every costume once, then show them. */
    try {
        allCostumes = await apiGet("/api/costumes");
        render();
    } catch (e) {
        document.getElementById("costumeGrid").innerHTML =
            `<div class="empty-state">Could not load the collection.</div>`;
    }

    /* Category tabs: clicking one filters the grid. */
    document.getElementById("filterTabs").addEventListener("click", function (e) {
        if (!e.target.classList.contains("filter-tab")) return;   /* ignore clicks between tabs */

        /* Move the "active" highlight to the clicked tab. */
        document.querySelectorAll(".filter-tab").forEach(t => t.classList.remove("is-active"));
        e.target.classList.add("is-active");

        activeCategory = e.target.dataset.category;   /* remember the choice */
        render();
    });

    /* Search box: filter as the user types. */
    document.getElementById("searchInput").addEventListener("input", render);

    /* Logout is handled by logout.js (shows a confirmation modal). */
}

/* Decide which costumes to show, then draw them. */
function render() {
    const term = document.getElementById("searchInput").value.trim().toLowerCase();

    /* Keep only costumes that match BOTH the category and the search. */
    const shown = allCostumes.filter(function (c) {
        const matchesCategory = (activeCategory === "All") || (c.category === activeCategory);
        const matchesSearch   = (term === "") ||
                                c.name.toLowerCase().includes(term) ||
                                c.category.toLowerCase().includes(term);
        return matchesCategory && matchesSearch;
    });

    drawGrid(shown);
}

/* Build the cards and put them on the page. */
function drawGrid(costumes) {
    const grid = document.getElementById("costumeGrid");
    grid.innerHTML = "";

    /* Friendly message if nothing matches. */
    if (costumes.length === 0) {
        grid.innerHTML = `<div class="empty-state">No costumes match your search.</div>`;
        return;
    }

    /* One card per costume. */
    costumes.forEach(function (c) {
        /* Work out the status label + badge colour. */
        const status = c.isAvailable ? "Available" : "On Hire";
        /* Use the era if we have one, otherwise fall back to the category. */
        const subtitle = c.era ? c.era : c.category;

        const card = document.createElement("div");
        card.className = "costume-card";
        card.innerHTML =
            /* Image placeholder with a status badge and a favourite heart */
            "<div class='costume-card__image'>" +
                "<span class='costume-card__badge'>" + statusBadge(status) + "</span>" +
                "<button class='costume-card__heart' aria-label='Add to favourites'>♡</button>" +
                "<span class='costume-card__placeholder'>GA</span>" +
            "</div>" +
            /* Card details */
            "<div class='costume-card__body'>" +
                "<h3 class='costume-card__name'>" + escapeHtml(c.name) + "</h3>" +
                "<p class='costume-card__era'>" + escapeHtml(subtitle) + "</p>" +
                "<p class='costume-card__price'>Rs " + c.pricePerDay + " / day</p>" +
                "<button class='btn btn--primary costume-card__view'>View Details →</button>" +
            "</div>";
        grid.appendChild(card);
    });
}
