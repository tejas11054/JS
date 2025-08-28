const container = document.getElementById("authContainer");

// ------------------------ Login ------------------------
document.getElementById("loginForm").addEventListener("submit", async e => {
    e.preventDefault();

    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    try {
        const res = await fetch("https://localhost:7170/api/Auth/Login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ userName: username, password })
        });

        if (!res.ok) throw new Error("Invalid login");

        const data = await res.json();

        // Default role: EMPLOYEE
        const roleName = "EMPLOYEE";

        localStorage.setItem("token", data.token);
        localStorage.setItem("userId", data.user.userId);
        localStorage.setItem("username", data.user.name);
        localStorage.setItem("role", roleName);

        window.location.href = "dashboard.html";
    } catch (err) {
        alert(err.message);
    }
});

// ------------------------ Register ------------------------
document.getElementById("registerBtn").addEventListener("click", showRegisterForm);
function showRegisterForm() {
    container.innerHTML = `
        <h3 class="text-center mb-3">Register</h3>
        <form id="registerForm">
            <div class="mb-2">
                <label class="form-label">Name</label>
                <input type="text" class="form-control" id="regName" required>
            </div>
            <div class="mb-2">
                <label class="form-label">Username</label>
                <input type="text" class="form-control" id="regUsername" required>
            </div>
            <div class="mb-2">
                <label class="form-label">Email</label>
                <input type="email" class="form-control" id="regEmail" required>
            </div>
            <div class="mb-2">
                <label class="form-label">Password</label>
                <input type="password" class="form-control" id="regPassword" required>
            </div>
            <button class="btn btn-success w-100 mt-2" type="submit">Register</button>
            <button class="btn btn-secondary w-100 mt-2" type="button" id="backBtn">Back to Login</button>
        </form>
    `;

    document.getElementById("backBtn").addEventListener("click", () => {
        window.location.reload();
    });

    document.getElementById("registerForm").addEventListener("submit", async e => {
        e.preventDefault();

        const user = {
            name: document.getElementById("regName").value.trim(),
            userName: document.getElementById("regUsername").value.trim(),
            email: document.getElementById("regEmail").value.trim(),
            password: document.getElementById("regPassword").value.trim(),
            // Default role: EMPLOYEE
            roleId: 2
        };

        try {
            const res = await fetch("https://localhost:7170/api/User/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(user)
            });

            if (!res.ok) {
                const err = await res.json();
                throw new Error(JSON.stringify(err));
            }

            alert("Registration successful! Please login.");
            window.location.reload();
        } catch (err) {
            alert("Registration failed: " + err.message);
        }
    });
}
