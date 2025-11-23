/* Notes Edit Script */
(function () {
    // Solo ejecutar si estamos en la página Edit
    var editForm = document.getElementById('editNoteForm');
    if (!editForm) return;

    var titleInput = document.querySelector('input[name="Title"]');
    var contentTextarea = document.querySelector('textarea[name="Content"]');
    var titleCount = document.getElementById('titleCount');
    var contentCount = document.getElementById('contentCount');
    var wordCount = document.getElementById('wordCount');

    var originalTitle = titleInput ? titleInput.value : '';
    var originalContent = contentTextarea ? contentTextarea.value : '';
    var formModified = false;

    function checkModification() {
        var currentTitle = titleInput ? titleInput.value : '';
        var currentContent = contentTextarea ? contentTextarea.value : '';
        formModified = (currentTitle !== originalTitle || currentContent !== originalContent);
    }

    // Inicializar contadores
    if (titleInput && titleCount) {
        titleCount.textContent = titleInput.value.length;
    }

    if (contentTextarea && contentCount) {
        contentCount.textContent = contentTextarea.value.length;
    }

    if (contentTextarea && wordCount) {
        var initialWords = contentTextarea.value.trim().split(/\s+/).filter(function (w) {
            return w.length > 0;
        });
        wordCount.textContent = initialWords.length;
    }

    // Contador de caracteres del título
    if (titleInput && titleCount) {
        titleInput.addEventListener('input', function () {
            titleCount.textContent = this.value.length;
            var previewTitle = document.getElementById('previewTitle');
            if (previewTitle) {
                previewTitle.textContent = this.value || 'Sin título';
            }
            checkModification();
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

            checkModification();
        });
    }

    // Validación antes de enviar
    editForm.addEventListener('submit', function (e) {
        var title = titleInput ? titleInput.value.trim() : '';
        var content = contentTextarea ? contentTextarea.value.trim() : '';

        if (!title || !content) {
            e.preventDefault();
            alert('Por favor, completa todos los campos obligatorios.');
            return false;
        }

        if (!confirm('¿Deseas guardar los cambios realizados en esta nota?')) {
            e.preventDefault();
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