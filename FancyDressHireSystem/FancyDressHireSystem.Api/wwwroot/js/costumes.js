/* costumes.js — the Costume Catalogue page.
   Shows costumes as cards, filters by category + search, and
   manages costumes through modals:
     - View Details (read-only popup)
     - Add / Edit (a form in a modal)
     - Delete (a confirmation modal)
     - Filters (a small panel for size / availability)
   Field names match the backend Costume class. */

let allCostumes = [];
let activeCategory = "All";
let extraFilters = { size: "", availability: "" };   /* from the Filters panel */

document.addEventListener("DOMContentLoaded", init);

async function init() {

    /* Show who is logged in, in the nav. */
    try {
        const me = await apiGet("/api/auth/me");
        document.getElementById("navUser").textContent = me.userName;
        document.getElementById("navAvatar").textContent = me.userName.charAt(0).toUpperCase();
    } catch (e) { return; }

    /* Load every costume once, then show them. */
    try {
        allCostumes = await apiGet("/api/costumes");
        render();
    } catch (e) {
        document.getElementById("costumeGrid").innerHTML =
            `<div class="empty-state">Could not load the collection.</div>`;
    }

    /* Category tabs filter the grid. */
    document.getElementById("filterTabs").addEventListener("click", function (e) {
        if (!e.target.classList.contains("filter-tab")) return;
        document.querySelectorAll(".filter-tab").forEach(t => t.classList.remove("is-active"));
        e.target.classList.add("is-active");
        activeCategory = e.target.dataset.category;
        render();
    });

    /* Search box filters as the user types. */
    document.getElementById("searchInput").addEventListener("input", render);

    /* Wire up the buttons and modals. */
    wireAddButton();
    wireFilters();
    wireModalClosing();

    /* Logout is handled by logout.js (shows a confirmation modal). */
}

/* ============================================================
   DRAW THE GRID
   ============================================================ */
function render() {
    const term = document.getElementById("searchInput").value.trim().toLowerCase();

    const shown = allCostumes.filter(function (c) {
        const matchesCategory = (activeCategory === "All") || (c.category === activeCategory);
        const matchesSearch = (term === "") ||
            c.costumeName.toLowerCase().includes(term) ||
            c.category.toLowerCase().includes(term);
        const matchesSize = (extraFilters.size === "") || (c.size === extraFilters.size);
        let matchesAvail = true;
        if (extraFilters.availability === "available") matchesAvail = isCostumeAvailable(c);
        if (extraFilters.availability === "onhire") matchesAvail = !isCostumeAvailable(c);
        return matchesCategory && matchesSearch && matchesSize && matchesAvail;
    });

    drawGrid(shown);
}

function drawGrid(costumes) {
    const grid = document.getElementById("costumeGrid");
    grid.innerHTML = "";

    if (costumes.length === 0) {
        grid.innerHTML = `<div class="empty-state">No costumes match your search.</div>`;
        return;
    }

    costumes.forEach(function (c) {
        const status = isCostumeAvailable(c) ? "Available" : "On Hire";
        const subtitle = c.era ? c.era : c.category;

        const card = document.createElement("div");
        card.className = "costume-card";
        /* NOTE: the favourite heart button has been removed. */
        card.innerHTML =
            "<div class='costume-card__image'>" +
            "<span class='costume-card__badge'>" + statusBadge(status) + "</span>" +
            "<span class='costume-card__placeholder'>GA</span>" +
            "</div>" +
            "<div class='costume-card__body'>" +
            "<h3 class='costume-card__name'>" + escapeHtml(c.costumeName) + "</h3>" +
            "<p class='costume-card__era'>" + escapeHtml(subtitle) + "</p>" +
            "<p class='costume-card__price'>Rs " + c.dailyHirePrice + " / day</p>" +
            "<button class='btn btn--primary costume-card__view' data-id='" + c.costumeId + "'>View Details →</button>" +
            "</div>";
        grid.appendChild(card);
    });

    /* Each "View Details" button opens the details modal. */
    document.querySelectorAll(".costume-card__view").forEach(function (btn) {
        btn.addEventListener("click", function () {
            openDetails(btn.dataset.id);
        });
    });
}

/* ============================================================
   VIEW DETAILS MODAL (read-only)
   ============================================================ */
function openDetails(costumeId) {
    const c = allCostumes.find(x => x.costumeId === costumeId);
    if (!c) return;

    const status = isCostumeAvailable(c) ? "Available" : "On Hire";
    const body = document.getElementById("detailsBody");
    body.innerHTML =
        "<div class='details-image'>" +
        "<span class='details-image__corner details-image__corner--tl'></span>" +
        "<span class='details-image__corner details-image__corner--tr'></span>" +
        "<span class='details-image__corner details-image__corner--bl'></span>" +
        "<span class='details-image__corner details-image__corner--br'></span>" +
        "<span class='details-image__mono'>G<i>A</i></span>" +
        "<span class='details-image__caption'>The Grand Atelier</span>" +
        "</div>" +
        "<div class='details-info'>" +
        "<p class='details-eyebrow'>" + escapeHtml(c.category) + " · " + escapeHtml(c.era) + "</p>" +
        "<h2 class='details-name'>" + escapeHtml(c.costumeName) + "</h2>" +
        "<div class='details-badge'>" + statusBadge(status) + "</div>" +
        "<dl class='details-grid'>" +
        detailRow("Archive No.", c.costumeId) +
        detailRow("Size", c.size) +
        detailRow("Hire Price", "Rs " + c.dailyHirePrice + " / day") +
        detailRow("In Collection", c.totalQuantity) +
        detailRow("Available", c.availableQuantity) +
        "</dl>" +
        "<div class='details-actions'>" +
        "<button class='btn btn--outline' data-edit='" + c.costumeId + "'>Edit</button>" +
        "<button class='btn btn--danger-outline' data-delete='" + c.costumeId + "'>Delete</button>" +
        "</div>" +
        "</div>";

    body.querySelector("[data-edit]").addEventListener("click", function () {
        closeModal("detailsModal");
        openCostumeForm(c);
    });
    body.querySelector("[data-delete]").addEventListener("click", function () {
        closeModal("detailsModal");
        openDeleteConfirm(c);
    });

    openModal("detailsModal");
}

function detailRow(label, value) {
    return "<div class='details-row'><dt>" + escapeHtml(label) + "</dt>" +
        "<dd>" + escapeHtml(value) + "</dd></div>";
}

/* ============================================================
   ADD / EDIT COSTUME FORM MODAL
   ============================================================ */
function wireAddButton() {
    document.getElementById("addCostumeBtn").addEventListener("click", function () {
        openCostumeForm(null);
    });
}

function openCostumeForm(costume) {
    const isEdit = costume !== null;
    document.getElementById("formTitle").textContent = isEdit ? "Edit Costume" : "Add Costume";

    setVal("f_id", isEdit ? costume.costumeId : "");
    setVal("f_name", isEdit ? costume.costumeName : "");
    setVal("f_cat", isEdit ? costume.category : "Royal");
    setVal("f_era", isEdit ? costume.era : "");
    setVal("f_size", isEdit ? costume.size : "M");
    setVal("f_price", isEdit ? costume.dailyHirePrice : "");
    setVal("f_total", isEdit ? costume.totalQuantity : "");
    setVal("f_avail", isEdit ? costume.availableQuantity : "");

    document.getElementById("f_id").disabled = isEdit;   /* ID can't change on edit */
    document.getElementById("formMessage").textContent = "";
    document.getElementById("costumeForm").dataset.mode = isEdit ? "edit" : "add";

    openModal("formModal");
}

async function submitCostumeForm(event) {
    event.preventDefault();
    const msg = document.getElementById("formMessage");
    msg.className = "message message--error";
    msg.textContent = "";

    const id = getVal("f_id").trim();
    const name = getVal("f_name").trim();
    const cat = getVal("f_cat");
    const era = getVal("f_era").trim();
    const size = getVal("f_size");
    const price = Number(getVal("f_price"));
    const total = Number(getVal("f_total"));
    const avail = Number(getVal("f_avail"));

    /* --- Validation (matches the backend rules) --- */
    if (id === "") { msg.textContent = "Please enter a costume ID."; return; }
    if (name === "") { msg.textContent = "Please enter a costume name."; return; }
    if (era === "") { msg.textContent = "Please enter the era."; return; }
    if (!(price > 0)) { msg.textContent = "Price must be greater than 0."; return; }
    if (total < 0) { msg.textContent = "Total quantity cannot be negative."; return; }
    if (avail < 0 || avail > total) {
        msg.textContent = "Available must be between 0 and total quantity.";
        return;
    }

    const mode = document.getElementById("costumeForm").dataset.mode;
    const payload = {
        costumeId: id, costumeName: name, category: cat, era: era,
        size: size, dailyHirePrice: price, totalQuantity: total, availableQuantity: avail
    };

    try {
        if (mode === "edit") {
            await apiPost("/api/costumes/" + id, payload);   /* PUT in the real API */
            const i = allCostumes.findIndex(x => x.costumeId === id);
            if (i !== -1) allCostumes[i] = payload;
            toast("Costume updated", "ok");
        } else {
            await apiPost("/api/costumes", payload);
            allCostumes.push(payload);
            toast("Costume added", "ok");
        }
        closeModal("formModal");
        render();
    } catch (e) {
        msg.textContent = "Could not save the costume. Please try again.";
    }
}

/* ============================================================
   DELETE CONFIRMATION MODAL
   ============================================================ */
function openDeleteConfirm(costume) {
    document.getElementById("deleteName").textContent = costume.costumeName;
    const confirmBtn = document.getElementById("deleteConfirm");

    /* Replace the button to clear any old handler, then add a fresh one. */
    const fresh = confirmBtn.cloneNode(true);
    confirmBtn.parentNode.replaceChild(fresh, confirmBtn);
    fresh.addEventListener("click", async function () {
        try {
            await apiPost("/api/costumes/" + costume.costumeId + "/delete", {});  /* DELETE in the real API */
            allCostumes = allCostumes.filter(x => x.costumeId !== costume.costumeId);
            closeModal("deleteModal");
            render();
            toast("Costume removed", "ok");
        } catch (e) { /* keep modal open on failure */ }
    });

    openModal("deleteModal");
}

/* ============================================================
   FILTERS PANEL
   ============================================================ */
function wireFilters() {
    document.getElementById("filtersBtn").addEventListener("click", function () {
        openModal("filtersModal");
    });
    document.getElementById("applyFilters").addEventListener("click", function () {
        extraFilters.size = getVal("filterSize");
        extraFilters.availability = getVal("filterAvail");
        closeModal("filtersModal");
        render();
    });
    document.getElementById("clearFilters").addEventListener("click", function () {
        extraFilters = { size: "", availability: "" };
        setVal("filterSize", "");
        setVal("filterAvail", "");
        closeModal("filtersModal");
        render();
    });
}

/* ============================================================
   SHARED MODAL HELPERS
   ============================================================ */
function openModal(id) { document.getElementById(id).classList.add("is-open"); }
function closeModal(id) { document.getElementById(id).classList.remove("is-open"); }

function wireModalClosing() {
    document.querySelectorAll(".modal-overlay").forEach(function (overlay) {
        overlay.addEventListener("click", function (e) {
            if (e.target === overlay) overlay.classList.remove("is-open");
        });
        overlay.querySelectorAll("[data-close]").forEach(function (btn) {
            btn.addEventListener("click", function () {
                overlay.classList.remove("is-open");
            });
        });
    });
    document.getElementById("costumeForm").addEventListener("submit", submitCostumeForm);
}

function getVal(id) { return document.getElementById(id).value; }
function setVal(id, v) { document.getElementById(id).value = v; }