// Function to show a notification message
function showSnackbar(message, type) {
    console.log("Message:", message);

    // Create notification element
    const notification = document.createElement("div");
    notification.style.position = "fixed";
    notification.style.top = "20px";
    notification.style.right = "20px";
    notification.style.padding = "15px 25px";
    notification.style.borderRadius = "5px";
    notification.style.color = "white";
    notification.style.backgroundColor = type === "error" ? "#ef4444" : "#22c55e";
    notification.style.boxShadow = "0 2px 5px rgba(0,0,0,0.2)";
    notification.style.zIndex = "1000";
    notification.textContent = message;

    // Add to body
    document.body.appendChild(notification);

    // Remove after 3 seconds
    setTimeout(() => {
        document.body.removeChild(notification);
    }, 3000);
}

// Function to handle login form submission
function handleLoginFormSubmit(event) {
    event.preventDefault();

    const form = event.target;
    const formData = new FormData(form);

    // Show loading message
    showSnackbar("Logging in... Please wait.", "success");

    // Simulate a 2-second delay before redirecting
    setTimeout(() => {
        fetch(form.action, {
            method: "POST",
            body: formData,
            headers: {
                Accept: "application/json",
            },
        })
            .then((response) => {
                if (response.redirected) {
                    window.location.href = response.url;
                } else {
                    return response.json();
                }
            })
            .then((data) => {
                if (data && data.error) {
                    showSnackbar(data.error, "error");
                }
            })
            .catch((error) => {
                showSnackbar("An error occurred. Please try again.", "error");
            });
    }, 2000);
}

// Attach event listeners to the forms
document.addEventListener("DOMContentLoaded", () => {
    const loginForm = document.querySelector("#loginForm");

    if (loginForm) {
        loginForm.addEventListener("submit", handleLoginFormSubmit);
    }
});