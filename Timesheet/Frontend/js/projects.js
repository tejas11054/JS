async function loadProjects(token, contentContainer) {
    try {
        const res = await fetch("https://localhost:7170/api/Project", {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) { contentContainer.innerHTML = `<p>Error loading projects: ${res.status}</p>`; return; }

        const projects = await res.json();

        let html = `<h3>Projects</h3>
                    <button id="addProjectBtn" class="btn btn-primary mb-3">Add Project</button>
                    <table class="table table-bordered">
                        <thead><tr>
                            <th>Project Name</th>
                            <th>Description</th>
                            <th>Actions</th>
                        </tr></thead>
                        <tbody id="projectTableBody"></tbody>
                    </table>`;
        contentContainer.innerHTML = html;

        const tbody = document.getElementById("projectTableBody");
        projects.forEach(p => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${p.projectName}</td>
                <td>${p.description}</td>
                <td>
                    <button class="btn btn-sm btn-warning editProjectBtn" data-id="${p.id}">Edit</button>
                    <button class="btn btn-sm btn-danger deleteProjectBtn" data-id="${p.id}">Delete</button>
                </td>`;
            tbody.appendChild(tr);
        });

        document.getElementById("addProjectBtn").addEventListener("click", () => showProjectForm(token, contentContainer));
        attachProjectActions(token, contentContainer);

    } catch (error) {
        contentContainer.innerHTML = `<p>Error loading projects: ${error.message}</p>`;
    }
}

function attachProjectActions(token, contentContainer) {
    document.querySelectorAll(".editProjectBtn").forEach(btn => {
        btn.addEventListener("click", async e => {
            const id = e.target.dataset.id;
            const res = await fetch(`https://localhost:7170/api/Project/${id}`, {
                headers: { Authorization: `Bearer ${token}` }
            });
            const project = await res.json();
            showProjectForm(token, contentContainer, project);
        });
    });

    document.querySelectorAll(".deleteProjectBtn").forEach(btn => {
        btn.addEventListener("click", async e => {
            const id = e.target.dataset.id;
            if (confirm("Are you sure to delete this project?")) {
                await fetch(`https://localhost:7170/api/Project/${id}`, {
                    method: "DELETE",
                    headers: { Authorization: `Bearer ${token}` }
                });
                loadProjects(token, contentContainer);
            }
        });
    });
}

function showProjectForm(token, contentContainer, project = null) {
    contentContainer.innerHTML = `
        <h4>${project ? "Edit" : "Add"} Project</h4>
        <form id="projectForm">
            <div class="mb-2">
                <label>Project Name</label>
                <input class="form-control" id="projectName" value="${project?.projectName || ""}" required>
            </div>
            <div class="mb-2">
                <label>Description</label>
                <textarea class="form-control" id="projectDescription">${project?.description || ""}</textarea>
            </div>
            <button class="btn btn-success">${project ? "Update" : "Add"}</button>
            <button type="button" class="btn btn-secondary" id="cancelProjectBtn">Cancel</button>
        </form>
        <div id="projectMessage" class="mt-2"></div>
    `;

    document.getElementById("cancelProjectBtn").addEventListener("click", () => loadProjects(token, contentContainer));

    document.getElementById("projectForm").addEventListener("submit", async e => {
        e.preventDefault();
        const newProject = {
            projectName: document.getElementById("projectName").value.trim(),
            description: document.getElementById("projectDescription").value.trim()
        };

        try {
            if (project) {
                await fetch(`https://localhost:7170/api/Project/${project.id}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                    body: JSON.stringify({ ...project, ...newProject })
                });
                document.getElementById("projectMessage").innerHTML = `<span class="text-success">Updated successfully!</span>`;
            } else {
                await fetch(`https://localhost:7170/api/Project`, {
                    method: "POST",
                    headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                    body: JSON.stringify(newProject)
                });
                document.getElementById("projectMessage").innerHTML = `<span class="text-success">Added successfully!</span>`;
            }
            loadProjects(token, contentContainer);
        } catch (err) {
            document.getElementById("projectMessage").innerHTML = `<span class="text-danger">Error: ${err.message}</span>`;
        }
    });
}
