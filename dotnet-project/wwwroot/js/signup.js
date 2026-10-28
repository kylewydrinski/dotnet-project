
const signupForm = document.getElementById("signupForm");
const signupMessage = document.getElementById("signupMessage");

signupForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    signupMessage.textContent = "Creating your account...";

    const formData = new FormData(signupForm);

    const user = {
        username: formData.get("username"),
        email: formData.get("email"),
        password: formData.get("password")
    };

    try {
        const response = await fetch("/api/users/signup", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(user)
        });

        const result = await response.json();

        if (!response.ok) {
            signupMessage.textContent =
                result.message ?? "Unable to create your account.";
            return;
        }

        signupMessage.textContent =
            "Account created successfully! Redirecting to login...";

        signupForm.reset();

        setTimeout(() => {
            window.location.href = "/pages/login.html";
        }, 1200);

    } catch (error) {
        signupMessage.textContent =
            "Unable to connect to the server. Please try again.";
        console.error("Signup request failed:", error);
    }
});
