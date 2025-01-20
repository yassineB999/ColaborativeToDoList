// Get URLs from hidden inputs
const createTaskUrl = document.getElementById('create-task-url').value;
const updateTaskUrl = document.getElementById('update-task-url').value;
const deleteTaskUrl = document.getElementById('delete-task-url').value;

// Snackbar function
function showSnackbar(message, type) {
    const notification = document.createElement("div");
    notification.className = `fixed top-5 right-5 p-4 rounded-lg text-white shadow-lg z-50 ${type === "error" ? "bg-red-500" : "bg-green-500"
        }`;
    notification.textContent = message;
    document.body.appendChild(notification);
    setTimeout(() => document.body.removeChild(notification), 3000);
}

// Create Task
function submitCreateTaskForm() {
    const form = document.getElementById('create-task-form');
    const formData = new FormData(form);

    const data = {
        Description: formData.get('Description'),
        CreatedAt: formData.get('CreatedAt') || new Date().toISOString(),
        EndedAt: formData.get('EndedAt'),
        CategoryName: formData.get('CategoryName'), // Use category name
        todoListId: formData.get('todoListId')
    };

    showSnackbar("Creating task...", "success");

    fetch(createTaskUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(result => {
            if (result.success) {
                showSnackbar(result.message || "Task created successfully!", "success");
                setTimeout(() => window.location.reload(), 1500);
            } else {
                showSnackbar(result.message || "Failed to create task", "error");
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showSnackbar('An error occurred. Please try again.', "error");
        });
}

// Update Task
function updateTask(id) {
    const description = document.querySelector(`tr[data-id="${id}"] .update-description-input`).value;
    const createdAt = document.querySelector(`tr[data-id="${id}"] .update-createdat-input`).value;
    const endedAt = document.querySelector(`tr[data-id="${id}"] .update-endedat-input`).value;
    const categoryName = document.querySelector(`tr[data-id="${id}"] .update-categoryname-input`).value;
    const todoListId = document.getElementById('todoListId').value; // Ensure this is correct

    const data = {
        Id: id,
        Description: description,
        CreatedAt: createdAt,
        EndedAt: endedAt,
        CategoryName: categoryName,
        todoListId: todoListId // Ensure this is included
    };

    showSnackbar("Updating task...", "success");

    fetch(updateTaskUrl, {
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
                showSnackbar(result.message || "Task updated successfully!", "success");
                setTimeout(() => window.location.reload(), 1500);
            } else {
                showSnackbar(result.message || "Failed to update task", "error");
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showSnackbar('An error occurred while updating. Please try again.', "error");
        });
}
// Delete Task
function deleteTask(id) {
    if (confirm('Are you sure you want to delete this task?')) {
        const todoListId = document.getElementById('todoListId').value; // Ensure this is correct

        const data = {
            Id: id,
            todoListId: todoListId // Ensure this is included
        };

        showSnackbar("Deleting task...", "success");

        fetch(deleteTaskUrl, {
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
                    showSnackbar(result.message || "Task deleted successfully!", "success");
                    setTimeout(() => window.location.reload(), 1500);
                } else {
                    showSnackbar(result.message || "Failed to delete task", "error");
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showSnackbar('An error occurred while deleting. Please try again.', "error");
            });
    }
}