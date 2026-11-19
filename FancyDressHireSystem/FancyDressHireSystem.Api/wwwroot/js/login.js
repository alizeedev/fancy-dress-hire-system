/* login.js — handles the sign-in form for The Grand Atelier */

/* Temporary switch: true = use a test login so the page works
   on its own. Set to false once the real login API is ready. */
const USE_FAKE = true;

/* Grab the parts of the form we need from the HTML. */
const form     = document.getElementById("loginForm");
const message  = document.getElementById("loginMessage");
const password = document.getElementById("password");
const toggle   = document.getElementById("togglePassword");

/* ---- Show / hide the password ----
   The password box starts as type="password" (dots). Clicking
   the button flips it to type="text" (readable) and back. */
toggle.addEventListener("click", function () {
    if (password.type === "password") {
        password.type = "text";              /* reveal the characters */
        toggle.textContent = "🙈";           /* closed-eye = click to hide */
        toggle.setAttribute("aria-label", "Hide password");
    } else {
        password.type = "password";          /* hide them again */
        toggle.textContent = "👁";            /* open-eye = click to show */
        toggle.setAttribute("aria-label", "Show password");
    }
});

/* ---- Handle the login when the form is submitted ---- */
form.addEventListener("submit", async function (event) {

    event.preventDefault();      /* stop the page from reloading */
    message.textContent = "";    /* clear any old error */

    /* Read what the user typed (trim removes stray spaces). */
    const username = document.getElementById("username").value.trim();
    const pass     = password.value;

    /* Validation: both boxes must be filled in. */
    if (username === "" || pass === "") {
        message.textContent = "Please enter both your username and password.";
        return;
    }

    /* Temporary test login (works before the real API exists). */
    if (USE_FAKE) {
        if (username === "admin" && pass === "Admin123!") {
            window.location.href = "dashboard.html";   /* success */
        } else {
            message.textContent = "Invalid username or password.";
        }
        return;
    }

    /* Real login: send details to the server and wait for a reply. */
    try {
        const response = await fetch("/api/auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "same-origin",
            body: JSON.stringify({ username, password: pass })
        });

        if (response.ok) {
            window.location.href = "dashboard.html";
        } else {
            message.textContent = "Invalid username or password.";
        }
    } catch (err) {
        message.textContent = "Could not reach the server. Please try again.";
    }
});
