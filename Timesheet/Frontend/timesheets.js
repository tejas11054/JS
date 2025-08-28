async function loadTimesheets(token, userId, contentContainer) {
    try {
        const res = await fetch("https://localhost:7170/api/Timesheet", {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) throw new Error("Failed to load timesheets");

        let timesheets = await res.json();
        timesheets = timesheets.filter(ts => ts.userId == userId);

        let html = `<h3>Timesheets</h3>
                    <button id="addTimesheetBtn" class="btn btn-primary mb-3">Add Timesheet</button>
                    <table class="table table-bordered">
                        <thead><tr>
                            <th>Project</th><th>Date</th><th>Hours Worked</th><th>Task Description</th><th>Actions</th>
                        </tr></thead>
                        <tbody id="timesheetTableBody"></tbody>
                    </table>`;
        contentContainer.innerHTML = html;

        const tbody = document.getElementById("timesheetTableBody");
        timesheets.forEach(ts => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${ts.project?.projectName || ts.projectId}</td>
                <td>${new Date(ts.date).toLocaleDateString()}</td>
                <td>${ts.hoursWorked}</td>
                <td>${ts.taskDescription || ""}</td>
                <td>
                    <button class="btn btn-sm btn-warning editBtn" data-id="${ts.id}">Edit</button>
                    <button class="btn btn-sm btn-danger deleteBtn" data-id="${ts.id}">Delete</button>
                </td>`;
            tbody.appendChild(tr);
        });

        document.getElementById("addTimesheetBtn").addEventListener("click", () => showTimesheetForm(token, userId, contentContainer));
        attachTimesheetActions(token, userId, contentContainer);

    } catch (err) {
        contentContainer.innerHTML = `<p>Error loading timesheets: ${err.message}</p>`;
    }
}

function attachTimesheetActions(token, userId, contentContainer) {
    document.querySelectorAll(".editBtn").forEach(btn => {
        btn.addEventListener("click", async e => {
            const id = e.target.dataset.id;
            const res = await fetch(`https://localhost:7170/api/Timesheet/${id}`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            const ts = await res.json();
            showTimesheetForm(token, userId, contentContainer, ts);
        });
    });

    document.querySelectorAll(".deleteBtn").forEach(btn => {
        btn.addEventListener("click", async e => {
            const id = e.target.dataset.id;
            if (confirm("Are you sure to delete this timesheet?")) {
                await fetch(`https://localhost:7170/api/Timesheet/${id}`, {
                    method: "DELETE",
                    headers: { Authorization: `Bearer ${token}` }
                });
                loadTimesheets(token, userId, contentContainer);
            }
        });
    });
}

function showTimesheetForm(token, userId, contentContainer, ts = null) {
    fetch("https://localhost:7170/api/Project", { headers: { Authorization: `Bearer ${token}` } })
        .then(res => res.json())
        .then(projects => {
            contentContainer.innerHTML = `
                <h4>${ts ? "Edit" : "Add"} Timesheet</h4>
                <form id="timesheetForm">
                    <div class="mb-2">
                        <label>Project:</label>
                        <select id="tsProjectId" class="form-control" required>
                            ${projects.map(p => `<option value="${p.id}" ${ts?.projectId === p.id ? "selected" : ""}>${p.projectName}</option>`).join("")}
                        </select>
                    </div>
                    <div class="mb-2">
                        <label>Date:</label>
                        <input type="date" id="tsDate" class="form-control" value="${ts ? ts.date.split('T')[0] : ""}" required>
                    </div>
                    <div class="mb-2">
                        <label>Hours Worked:</label>
                        <input type="number" id="tsHours" class="form-control" value="${ts?.hoursWorked || ""}" required>
                    </div>
                    <div class="mb-2">
                        <label>Task Description:</label>
                        <input type="text" id="tsTask" class="form-control" value="${ts?.taskDescription || ""}">
                    </div>
                    <button class="btn btn-success">${ts ? "Update" : "Add"}</button>
                    <button type="button" class="btn btn-secondary" id="cancelBtn">Cancel</button>
                </form>
                <div id="tsMessage" class="mt-2"></div>
            `;

            document.getElementById("cancelBtn").addEventListener("click", () => loadTimesheets(token, userId, contentContainer));

            document.getElementById("timesheetForm").addEventListener("submit", async e => {
                e.preventDefault();
                const newTs = {
                    userId,
                    projectId: parseInt(document.getElementById("tsProjectId").value),
                    date: document.getElementById("tsDate").value,
                    hoursWorked: parseInt(document.getElementById("tsHours").value),
                    taskDescription: document.getElementById("tsTask").value
                };

                try {
                    if (ts) {
                        await fetch(`https://localhost:7170/api/Timesheet/${ts.id}`, {
                            method: "PUT",
                            headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                            body: JSON.stringify({ ...ts, ...newTs })
                        });
                        document.getElementById("tsMessage").innerHTML = `<span class="text-success">Updated successfully!</span>`;
                    } else {
                        await fetch(`https://localhost:7170/api/Timesheet`, {
                            method: "POST",
                            headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                            body: JSON.stringify(newTs)
                        });
                        document.getElementById("tsMessage").innerHTML = `<span class="text-success">Added successfully!</span>`;
                    }
                    loadTimesheets(token, userId, contentContainer);
                } catch (err) {
                    document.getElementById("tsMessage").innerHTML = `<span class="text-danger">Error: ${err.message}</span>`;
                }
            });
        });
}
