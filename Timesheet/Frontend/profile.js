async function loadProfile(token, userId, contentContainer) {
    try {
        const res = await fetch(`https://localhost:7170/api/User/${userId}`, {
            headers: { Authorization: `Bearer ${token}` }
        });
        if (!res.ok) { contentContainer.innerHTML = "<p>Error loading profile</p>"; return; }
        const user = await res.json();

        contentContainer.innerHTML = `
            <h3>Profile</h3>
            <form id="profileForm">
                <div class="mb-2">
                    <label>Name</label>
                    <input class="form-control" id="profileName" value="${user.name || ""}" required>
                </div>
                <div class="mb-2">
                    <label>Email</label>
                    <input type="email" class="form-control" id="profileEmail" value="${user.email || ""}" required>
                </div>
                <div class="mb-2">
                    <label>Password</label>
                    <input type="password" class="form-control" id="profilePassword" placeholder="Leave blank to keep existing">
                </div>
                <button class="btn btn-primary mt-2">Update</button>
            </form>
        `;

        document.getElementById("profileForm").addEventListener("submit", async e => {
            e.preventDefault();
            const passwordValue = document.getElementById("profilePassword").value.trim();
            const updated = {
                userId: user.userId,
                name: document.getElementById("profileName").value.trim(),
                email: document.getElementById("profileEmail").value.trim(),
                password: passwordValue || user.password,
                roleId: 1 // EMPLOYEE only
            };

            try {
                const putRes = await fetch(`https://localhost:7170/api/User/${userId}`, {
                    method: "PUT",
                    headers: { "Content-Type": "application/json", Authorization: `Bearer ${token}` },
                    body: JSON.stringify(updated)
                });
                if (putRes.ok) alert("Profile updated successfully!");
                else {
                    const err = await putRes.json();
                    alert("Update failed: " + JSON.stringify(err));
                }
            } catch (error) {
                alert("Update error: " + error.message);
            }
        });

    } catch (error) {
        contentContainer.innerHTML = "<p>Error loading profile</p>";
    }
}
