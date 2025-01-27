document.addEventListener('DOMContentLoaded', () => {
    const container = document.getElementById('collaborators-container');
    const token = document.querySelector('input[name="__RequestVerificationToken"]').value;

    // Event listener for buttons
    container.addEventListener('click', async (e) => {
        const btn = e.target.closest('button');
        if (!btn || !btn.dataset.id) return;

        const action = btn.classList.contains('btn-approve') ? 'approve' : 'reject';
        const id = btn.dataset.id;

        btn.disabled = true;
        btn.innerHTML = `<i class="fa fa-spinner fa-spin"></i> Processing...`;

        try {
            const response = await fetch(`/TodoLists/${action}Collaborator?collaboratorId=${id}`, {
                method: 'POST',
                headers: {
                    'RequestVerificationToken': token,
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) throw new Error('Request failed');

            const result = await response.json();

            if (result.success) {
                const row = document.getElementById(`row-${id}`);
                if (row) row.remove();

                if (!document.querySelector('#collaborators-container tbody tr')) {
                    container.innerHTML = `
                        <div class="text-center py-12 bg-gray-50 rounded-lg">
                            <i class="fas fa-check-circle text-4xl text-green-500 mb-4"></i>
                            <h3 class="text-lg font-medium text-gray-900">No pending requests</h3>
                        </div>
                    `;
                }
            }
        } catch (error) {
            console.error('Error:', error);
            showNotification('Action failed. Please try again.', 'error');
        } finally {
            btn.disabled = false;
            btn.innerHTML = action === 'approve' ? 'Approve' : 'Reject';
        }
    });
    function showNotification(message, type) {
        const notification = document.createElement('div');
        notification.className = `fixed top-4 right-4 p-4 rounded-lg text-white ${type === 'success' ? 'bg-green-500' : 'bg-red-500'
            }`;
        notification.textContent = message;
        document.body.appendChild(notification);

        setTimeout(() => notification.remove(), 3000);
    }
});
    function showNotification(message, type) {
        const notification = document.createElement('div');
        notification.className = `fixed top-4 right-4 p-4 rounded-lg text-white ${type === 'success' ? 'bg-green-500' : 'bg-red-500'
            }`;
        notification.textContent = message;
        document.body.appendChild(notification);

        setTimeout(() => notification.remove(), 3000);
    }
