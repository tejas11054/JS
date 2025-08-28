document.addEventListener("DOMContentLoaded", () => {
    const token = localStorage.getItem("token");
    const userId = localStorage.getItem("userId");
    const username = localStorage.getItem("username");

    if (!token) {
        window.location.href = "index.html";
        return;
    }

    const tabsContainer = document.getElementById("tabs");
    const contentContainer = document.getElementById("tabContent");
    const welcomeElem = document.getElementById("welcomeUser");
    const logoutBtn = document.getElementById("logoutBtn");

    // Welcome message
    welcomeElem.innerText = `Welcome, ${username} (EMPLOYEE)`;

    logoutBtn.addEventListener("click", () => {
        localStorage.clear();
        window.location.href = "index.html";
    });

    // Only employee tabs
    const tabs = [
        { id: "profile", label: "Profile" },
        { id: "projects", label: "Projects" },
        { id: "timesheet", label: "Timesheet" }
    ];

    // Render tabs
    tabs.forEach((tab, i) => {
        const li = document.createElement("li");
        li.className = "nav-item";
        const a = document.createElement("a");
        a.className = `nav-link ${i === 0 ? "active" : ""}`;
        a.href = `#${tab.id}`;
        a.dataset.tab = tab.id;
        a.innerText = tab.label;
        a.addEventListener("click", e => {
            e.preventDefault();
            loadTab(tab.id);
            document.querySelectorAll("#tabs .nav-link").forEach(l => l.classList.remove("active"));
            a.classList.add("active");
        });
        li.appendChild(a);
        tabsContainer.appendChild(li);
    });

    // Load first tab
    loadTab(tabs[0].id);

    async function loadTab(tabId) {
        contentContainer.innerHTML = "<p>Loading...</p>";
        switch (tabId) {
            case "profile": return loadProfile(token, userId, contentContainer);
            case "projects": return loadProjects(token, contentContainer);
            case "timesheet": return loadTimesheets(token, userId, contentContainer);
        }
    }
});
