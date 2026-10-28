
const loginForm = document.getElementById("loginForm");
const loginMessage = document.getElementById("loginMessage");

loginForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    loginMessage.textContent = "Logging in...";

    const formData = new FormData(loginForm);

    const user = {
        username: formData.get("username"),
        password: formData.get("password")
    };

    try {
        const response = await fetch("/api/users/login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(user)
        });

        const result = await response.json();

        if (!response.ok) {
            loginMessage.textContent =
                result.message ?? "Unable to login.";
            return;
        }

        loginMessage.textContent =
            "Login successful! Redirecting...";

        loginForm.reset();

        setTimeout(() => {
            window.location.href = "/";
        }, 1200);

    } catch (error) {
        loginMessage.textContent =
            "Username or password is incorrect. Please try again.";
        console.error("Login request failed:", error);
    }
});
