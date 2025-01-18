// Get the URLs from the hidden inputs
const createUrl = document.getElementById('create-url').value;
const updateUrl = document.getElementById('update-url').value;
const deleteUrl = document.getElementById('delete-url').value;

// Function to show a notification message
function showSnackbar(message, type) {
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

// Function to handle creating a Todo List
function submitCreateForm() {
    const form = document.getElementById('create-form');
    const formData = new FormData(form);

    const data = {
        Title: formData.get('Title')
    };

    showSnackbar("Creating todo list...", "success");

    fetch(createUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(result => {
            if (result.success) {
                showSnackbar(result.message || "Todo list created successfully!", "success");
                setTimeout(() => window.location.reload(), 1500); // Reload after 1.5 seconds
            } else {
                showSnackbar(result.message || "Failed to create todo list", "error");
                if (result.errors) {
                    console.error("Validation errors:", result.errors);
                }
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showSnackbar('An error occurred. Please try again.', "error");
        });
}

// Function to handle updating a Todo List
function updateTodoList(id) {
    const titleInput = document.querySelector(`tr[data-id="${id}"] .update-title-input`);
    const newTitle = titleInput.value;

    const data = {
        Id: id,
        Title: newTitle
    };

    showSnackbar("Updating todo list...", "success");

    fetch(updateUrl, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                showSnackbar(result.message || "Todo list updated successfully!", "success");
                setTimeout(() => window.location.reload(), 1500); // Reload after 1.5 seconds
            } else {
                showSnackbar(result.message || "Failed to update todo list", "error");
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showSnackbar('An error occurred while updating. Please try again.', "error");
        });
}

// Function to handle deleting a Todo List
function deleteTodoList(id) {
    if (confirm('Are you sure you want to delete this Todo List?')) {
        const data = {
            Id: id
        };

        showSnackbar("Deleting todo list...", "success");

        fetch(deleteUrl, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify(data)
        })
            .then(response => response.json())
            .then(result => {
                if (result.success) {
                    showSnackbar(result.message || "Todo list deleted successfully!", "success");
                    setTimeout(() => window.location.reload(), 1500); // Reload after 1.5 seconds
                } else {
                    showSnackbar(result.message || "Failed to delete todo list", "error");
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showSnackbar('An error occurred while deleting. Please try again.', "error");
            });
    }
}