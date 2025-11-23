/* Notes Create Script */
(function () {
    // Solo ejecutar si estamos en la página Create
    var createForm = document.getElementById('createNoteForm');
    if (!createForm) return;

    var titleInput = document.querySelector('input[name="Title"]');
    var contentTextarea = document.querySelector('textarea[name="Content"]');
    var titleCount = document.getElementById('titleCount');
    var contentCount = document.getElementById('contentCount');
    var wordCount = document.getElementById('wordCount');
    var formModified = false;

    // Contador de caracteres del título
    if (titleInput && titleCount) {
        titleInput.addEventListener('input', function () {
            titleCount.textContent = this.value.length;
            var previewTitle = document.getElementById('previewTitle');
            if (previewTitle) {
                previewTitle.textContent = this.value || 'Sin título';
            }
            formModified = true;
        });
    }

    // Contador de caracteres y palabras del contenido
    if (contentTextarea) {
        contentTextarea.addEventListener('input', function () {
            var text = this.value;

            if (contentCount) {
                contentCount.textContent = text.length;
            }

            if (wordCount) {
                var words = text.trim().split(/\s+/).filter(function (word) {
                    return word.length > 0;
                });
                wordCount.textContent = words.length;
            }

            var previewContent = document.getElementById('previewContent');
            if (previewContent) {
                previewContent.innerHTML = text ? text.replace(/\n/g, '<br>') : 'El contenido aparecerá aquí...';
            }

            formModified = true;
        });
    }

    // Validación antes de enviar
    createForm.addEventListener('submit', function (e) {
        var title = titleInput ? titleInput.value.trim() : '';
        var content = contentTextarea ? contentTextarea.value.trim() : '';

        if (!title || !content) {
            e.preventDefault();
            alert('Por favor, completa todos los campos obligatorios.');
            return false;
        }

        formModified = false;

        var submitBtn = document.getElementById('submitBtn');
        if (submitBtn) {
            submitBtn.disabled = true;
            submitBtn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';
        }
    });

    // Advertencia al salir sin guardar
    window.addEventListener('beforeunload', function (e) {
        if (formModified) {
            e.preventDefault();
            e.returnValue = '';
        }
    });
})();