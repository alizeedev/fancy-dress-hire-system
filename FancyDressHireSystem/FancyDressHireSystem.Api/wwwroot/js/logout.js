/* logout.js — shows a confirmation modal before logging out.
   Shared by dashboard, costumes and rentals pages. */

document.addEventListener("DOMContentLoaded", function () {
    const logoutBtn = document.getElementById("logoutBtn");
    const overlay   = document.getElementById("logoutModal");
    const cancelBtn = document.getElementById("logoutCancel");
    const confirmBtn= document.getElementById("logoutConfirm");

    /* If this page has no logout button/modal, do nothing. */
    if (!logoutBtn || !overlay) return;

    /* Open the modal when Logout is clicked. */
    logoutBtn.addEventListener("click", function () {
        overlay.classList.add("is-open");
    });

    /* Cancel: just close the modal. */
    cancelBtn.addEventListener("click", function () {
        overlay.classList.remove("is-open");
    });

    /* Click outside the white box also closes it. */
    overlay.addEventListener("click", function (e) {
        if (e.target === overlay) overlay.classList.remove("is-open");
    });

    /* Confirm: actually log out and go to the login page. */
    confirmBtn.addEventListener("click", async function () {
        try { await apiPost("/api/auth/logout", {}); } catch (e) {}
        window.location.href = "login.html";
    });
});
