const loginLink = document.querySelector('nav a[href="/pages/login.html"]');
const signupLink = document.querySelector('nav a[href="/pages/signup.html"]');
const logoutLink = document.createElement("a");
logoutLink.onclick = () => {
    fetch("/api/users/logout", {
        method: "POST"
    })
        .then((response) => {
            if (!response.ok) {
                throw new Error(`Unable to logout (${response.status}).`);
            }

            return response.json();
        })
        .then(() => {
            window.location.href = "/";
        })
        .catch((error) => {
            console.error("Could not logout:", error);
        });
}
logoutLink.textContent = "Logout";
logoutLink.href = "/";

if (loginLink && signupLink) {
    fetch("/api/users/me")
        .then(async (response) => {
            if (response.status === 401) {
                return null;
            }

            if (!response.ok) {
                throw new Error(`Unable to load current user (${response.status}).`);
            }

            return response.json();
        })
        .then((user) => {
            if (!user) {
                return;
            }

            const username = document.createElement("span");
            username.className = "nav-user";
            username.textContent = user.username;

            loginLink.replaceWith(username);
            signupLink.replaceWith(logoutLink);
        })
        .catch((error) => {
            console.error("Could not update navigation for the signed-in user:", error);
        });
}
