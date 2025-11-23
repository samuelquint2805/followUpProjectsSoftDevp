/* Notes Scripts - Solo funciones globales */

// Confirmación de eliminación (usado en Index y Edit)
function confirmDelete(id, title) {
    var noteTitle = document.getElementById('noteTitle');
    var deleteLink = document.getElementById('deleteLink');

    if (noteTitle) {
        noteTitle.textContent = title;
    }

    if (deleteLink) {
        deleteLink.href = '/Notes/Delete/' + id;
    }

    var deleteModal = document.getElementById('deleteModal');
    if (deleteModal && typeof bootstrap !== 'undefined') {
        var modal = new bootstrap.Modal(deleteModal);
        modal.show();
    }
}

// Búsqueda en Index (solo se ejecuta si existe searchInput)
(function () {
    var searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    searchInput.addEventListener('input', function (e) {
        var searchTerm = e.target.value.toLowerCase();
        var noteItems = document.querySelectorAll('.note-item');
        var visibleCount = 0;

        noteItems.forEach(function (item) {
            var title = item.getAttribute('data-title') || '';
            var content = item.getAttribute('data-content') || '';

            if (title.includes(searchTerm) || content.includes(searchTerm)) {
                item.classList.remove('d-none');
                visibleCount++;
            } else {
                item.classList.add('d-none');
            }
        });

        var noResultsMsg = document.getElementById('noResultsMessage');
        if (noResultsMsg) {
            if (visibleCount === 0 && searchTerm !== '') {
                noResultsMsg.classList.remove('d-none');
            } else {
                noResultsMsg.classList.add('d-none');
            }
        }
    });
})();