(function () {
    var lengthRange = document.getElementById('lengthRange');
    var lengthInput = document.getElementById('Lenght');
    var generatedPasswordInput = document.getElementById('GeneratedPassword');
    var lengthDisplay = document.getElementById('lengthDisplay');
    var previewBtn = document.getElementById('previewBtn');
    var copyPreviewBtn = document.getElementById('copyPreviewBtn');
    var previewSection = document.getElementById('previewSection');
    var previewPassword = document.getElementById('previewPassword');
    var warningAlert = document.getElementById('warningAlert');
    var passwordForm = document.getElementById('passwordForm');
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));
    var deleteForm = document.getElementById('deleteForm');

    lengthRange.addEventListener('input', function () {
        lengthInput.value = this.value;
        lengthDisplay.textContent = this.value;
    });

    previewBtn.addEventListener('click', function () {
        var length = parseInt(lengthInput.value);
        var upper = document.getElementById('IncludeUppercase').checked;
        var lower = document.getElementById('IncludeLowercase').checked;
        var numbers = document.getElementById('IncludeNumbers').checked;
        var special = document.getElementById('IncludeSpecialCharacters').checked;

        if (!upper && !lower && !numbers && !special) {
            warningAlert.classList.remove('d-none');
            return;
        }
        warningAlert.classList.add('d-none');

        var chars = '';
        if (upper) chars += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
        if (lower) chars += 'abcdefghijklmnopqrstuvwxyz';
        if (numbers) chars += '0123456789';
        if (special) chars += '!*+-_.';

        var password = '';
        for (var i = 0; i < length; i++) {
            password += chars.charAt(Math.floor(Math.random() * chars.length));
        }

        previewPassword.value = password;
        generatedPasswordInput.value = password;
        previewSection.classList.remove('d-none');
    });

    copyPreviewBtn.addEventListener('click', function () {
        previewPassword.select();
        document.execCommand('copy');
        alert('Contraseña copiada');
    });

    passwordForm.addEventListener('submit', function (e) {
        var upper = document.getElementById('IncludeUppercase').checked;
        var lower = document.getElementById('IncludeLowercase').checked;
        var numbers = document.getElementById('IncludeNumbers').checked;
        var special = document.getElementById('IncludeSpecialCharacters').checked;

        if (!upper && !lower && !numbers && !special) {
            e.preventDefault();
            warningAlert.classList.remove('d-none');
            return;
        }

        if (generatedPasswordInput.value === '') {
            var length = parseInt(lengthInput.value);
            var chars = '';
            if (upper) chars += 'ABCDEFGHIJKLMNOPQRSTUVWXYZ';
            if (lower) chars += 'abcdefghijklmnopqrstuvwxyz';
            if (numbers) chars += '0123456789';
            if (special) chars += '!*+-_.';

            var password = '';
            for (var i = 0; i < length; i++) {
                password += chars.charAt(Math.floor(Math.random() * chars.length));
            }
            generatedPasswordInput.value = password;
        }
    });

    document.querySelectorAll('.btn-toggle-visibility').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var targetId = this.getAttribute('data-target');
            var field = document.querySelector('input[data-id="' + targetId + '"]');
            if (field.type === 'password') {
                field.type = 'text';
                this.textContent = 'Ocultar';
            } else {
                field.type = 'password';
                this.textContent = 'Mostrar';
            }
        });
    });

    document.querySelectorAll('.btn-copy').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var targetId = this.getAttribute('data-target');
            var field = document.querySelector('input[data-id="' + targetId + '"]');
            var originalType = field.type;
            field.type = 'text';
            field.select();
            document.execCommand('copy');
            field.type = originalType;
            alert('Contraseña copiada');
        });
    });

    document.querySelectorAll('.btn-delete').forEach(function (btn) {
        btn.addEventListener('click', function () {
            var id = this.getAttribute('data-id');
            deleteForm.action = '@Url.Action("Delete", "PasswordHash")/' + id;
            deleteModal.show();
        });
    });
})();