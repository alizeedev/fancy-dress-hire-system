/* dropdown.js — turns every <select> into an elegant custom dropdown. */

document.addEventListener("DOMContentLoaded", function () {
    /* Upgrade every select on the page. */
    document.querySelectorAll("select").forEach(upgradeSelect);

    /* Close any open menu when clicking elsewhere on the page. */
    document.addEventListener("click", function () {
        document.querySelectorAll(".dd.is-open").forEach(function (dd) {
            dd.classList.remove("is-open");
        });
    });
});

/* Build one custom dropdown that wraps a single <select>. */
function upgradeSelect(select) {
    if (select.dataset.upgraded) return;   /* don't wrap twice */
    select.dataset.upgraded = "true";

    /* Wrapper that holds the hidden select + our custom UI. */
    const dd = document.createElement("div");
    dd.className = "dd";
    select.parentNode.insertBefore(dd, select);
    dd.appendChild(select);                /* move the real select inside */
    select.classList.add("dd__native");    /* CSS hides it */

    /* The button the user sees and clicks. */
    const button = document.createElement("button");
    button.type = "button";                /* never submits the form */
    button.className = "dd__button";
    dd.appendChild(button);

    /* The menu that drops down. */
    const menu = document.createElement("div");
    menu.className = "dd__menu";
    dd.appendChild(menu);

    /* Draw the options now, and again whenever the select changes
       (the customer/costume lists are filled by JS after load). */
    build();
    watchForRebuild(select, build);

    /* Open / close the menu when the button is clicked. */
    button.addEventListener("click", function (e) {
        e.stopPropagation();               /* don't trigger the page-close */
        const open = dd.classList.contains("is-open");
        /* Close other menus first, then toggle this one. */
        document.querySelectorAll(".dd.is-open").forEach(function (o) {
            o.classList.remove("is-open");
        });
        if (!open) dd.classList.add("is-open");
    });

    /* (Re)draw the button label and the list of options. */
    function build() {
        button.textContent = select.options.length ? select.options[select.selectedIndex].text : "";
        menu.innerHTML = "";

        Array.from(select.options).forEach(function (opt, i) {
            const item = document.createElement("div");
            item.className = "dd__option" + (i === select.selectedIndex ? " is-selected" : "");
            item.textContent = opt.text;

            /* Picking an option updates the real select, then fires its
               "change" event so existing page code reacts as normal. */
            item.addEventListener("click", function (e) {
                e.stopPropagation();
                select.selectedIndex = i;
                select.dispatchEvent(new Event("change", { bubbles: true }));
                button.textContent = opt.text;
                dd.classList.remove("is-open");
                build();                   /* refresh the tick mark */
            });

            menu.appendChild(item);
        });
    }
}

/* Rebuild the custom menu when the hidden select's options change.
   Some selects (customer, costume) are empty at first and filled
   later by other scripts; this keeps our menu in sync. */
function watchForRebuild(select, build) {
    const observer = new MutationObserver(build);
    observer.observe(select, { childList: true });
}