
document.addEventListener('DOMContentLoaded', function () {
    // === FILTROS ===
    const searchName = document.getElementById('searchName');
    const filterDate = document.getElementById('filterDate');
    const filterGuests = document.getElementById('filterGuests');
    const filterStatus = document.getElementById('filterStatus');
    const btnClearFilters = document.getElementById('btnClearFilters');
    const rows = document.querySelectorAll('.reservation-row');
    const noResultsMessage = document.getElementById('noResultsMessage');

    function applyFilters() {
        const nameValue = searchName.value.toLowerCase();
        const dateValue = filterDate.value;
        const guestsValue = filterGuests.value;
        const statusValue = filterStatus.value;

        let visibleCount = 0;

        rows.forEach(row => {
            let show = true;

            // Filtro por nombre
            if (nameValue && !row.dataset.name.includes(nameValue)) {
                show = false;
            }

            // Filtro por fecha
            if (dateValue && row.dataset.date !== dateValue) {
                show = false;
            }

            // Filtro por número de personas
            if (guestsValue) {
                const guests = parseInt(row.dataset.guests);
                switch (guestsValue) {
                    case '1':
                        if (guests !== 1) show = false;
                        break;
                    case '2':
                        if (guests !== 2) show = false;
                        break;
                    case '3':
                        if (guests < 3 || guests > 4) show = false;
                        break;
                    case '5':
                        if (guests < 5) show = false;
                        break;
                }
            }

            // Filtro por estado
            if (statusValue && row.dataset.status !== statusValue) {
                show = false;
            }

            row.style.display = show ? '' : 'none';
            if (show) visibleCount++;
        });

        // Mostrar mensaje si no hay resultados
        if (noResultsMessage) {
            noResultsMessage.style.display = visibleCount === 0 && rows.length > 0 ? 'block' : 'none';
        }
    }

    // Event listeners para filtros
    if (searchName) searchName.addEventListener('input', applyFilters);
    if (filterDate) filterDate.addEventListener('change', applyFilters);
    if (filterGuests) filterGuests.addEventListener('change', applyFilters);
    if (filterStatus) filterStatus.addEventListener('change', applyFilters);

    // Limpiar filtros
    if (btnClearFilters) {
        btnClearFilters.addEventListener('click', function () {
            if (searchName) searchName.value = '';
            if (filterDate) filterDate.value = '';
            if (filterGuests) filterGuests.value = '';
            if (filterStatus) filterStatus.value = '';
            applyFilters();
        });
    }
    document.addEventListener('DOMContentLoaded', function () {
        // Datos de la reserva confirmada
        const reservationData = {
            name: '@TempData["ConfirmedName"]',
            date: '@TempData["ConfirmedDate"]',
            time: '@TempData["ConfirmedTime"]',
            guests: '@TempData["ConfirmedGuests"]'
        };

        // Llenar los datos en el modal
        document.getElementById('confirmName').textContent = reservationData.name;
        document.getElementById('confirmDate').textContent = reservationData.date;
        document.getElementById('confirmTime').textContent = reservationData.time;
        document.getElementById('confirmGuests').textContent = reservationData.guests;

        // Mostrar el modal
        const confirmModal = new bootstrap.Modal(document.getElementById('confirmationModal'));
        confirmModal.show();
    });
    // === MODAL DE ELIMINACIÓN ===
    const deleteModalElement = document.getElementById('deleteModal');

    if (deleteModalElement) {
        const deleteModal = new bootstrap.Modal(deleteModalElement);
        const deleteForm = document.getElementById('deleteForm');
        const reservationNameToDelete = document.getElementById('reservationNameToDelete');
        const reservationDateToDelete = document.getElementById('reservationDateToDelete');
        const baseDeleteUrl = deleteModalElement.dataset.deleteUrl;

        document.querySelectorAll('.btn-delete').forEach(btn => {
            btn.addEventListener('click', function (e) {
                e.preventDefault();
                e.stopPropagation();

                const id = this.dataset.id;
                const name = this.dataset.name;
                const date = this.dataset.date;

                if (reservationNameToDelete) {
                    reservationNameToDelete.textContent = name;
                }
                if (reservationDateToDelete) {
                    reservationDateToDelete.textContent = date;
                }
                if (deleteForm && baseDeleteUrl) {
                    deleteForm.action = baseDeleteUrl + '/' + id;
                }

                deleteModal.show();
            });
        });
    }

    // === ANIMACIONES DE FILAS ===
    rows.forEach((row, index) => {
        row.style.opacity = '0';
        row.style.transform = 'translateY(10px)';

        setTimeout(() => {
            row.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
            row.style.opacity = '1';
            row.style.transform = 'translateY(0)';
        }, index * 50);
    });

    // === AUTO-CERRAR ALERTAS ===
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            if (bsAlert) {
                bsAlert.close();
            }
        }, 5000);
    });

    // === HIGHLIGHT DE RESERVAS DE HOY ===
    rows.forEach(row => {
        if (row.dataset.status === 'today') {
            row.classList.add('table-success');
        }
    });

    // === TOOLTIP INICIALIZACIÓN ===
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    [...tooltipTriggerList].map(el => new bootstrap.Tooltip(el));

    // === CONFIRMACIÓN RÁPIDA ===
    document.querySelectorAll('.btn-confirm').forEach(btn => {
        btn.addEventListener('click', function () {
            const reservationId = this.dataset.id;
            // Aquí podrías implementar lógica AJAX para confirmar sin recargar
            console.log('Confirmar reserva:', reservationId);
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const nameInput = document.getElementById('Username');
    const dateInput = document.getElementById('reservationDateTime');
    const guestsInput = document.getElementById('guestsInput');
    const btnDecrease = document.getElementById('btnDecrease');
    const btnIncrease = document.getElementById('btnIncrease');
    const timeSlots = document.querySelectorAll('.time-slot');

    // Establecer fecha mínima (mañana)
    const tomorrow = new Date();
    tomorrow.setDate(tomorrow.getDate() + 1);
    tomorrow.setHours(8, 0, 0, 0);
    dateInput.min = tomorrow.toISOString().slice(0, 16);

    // Actualizar resumen en tiempo real
    nameInput.addEventListener('input', function () {
        document.getElementById('summaryName').textContent = this.value || '-';
    });

    dateInput.addEventListener('change', function () {
        const date = new Date(this.value);
        document.getElementById('summaryDate').textContent = date.toLocaleDateString('es-ES', {
            weekday: 'long', year: 'numeric', month: 'long', day: 'numeric'
        });
        document.getElementById('summaryTime').textContent = date.toLocaleTimeString('es-ES', {
            hour: '2-digit', minute: '2-digit'
        });
    });

    guestsInput.addEventListener('change', function () {
        document.getElementById('summaryGuests').textContent = this.value;
    });

    // Botones +/-
    btnDecrease.addEventListener('click', function () {
        if (guestsInput.value > 1) {
            guestsInput.value = parseInt(guestsInput.value) - 1;
            guestsInput.dispatchEvent(new Event('change'));
        }
    });

    btnIncrease.addEventListener('click', function () {
        if (guestsInput.value < 20) {
            guestsInput.value = parseInt(guestsInput.value) + 1;
            guestsInput.dispatchEvent(new Event('change'));
        }
    });

    // Selección de horario rápido
    timeSlots.forEach(slot => {
        slot.addEventListener('click', function () {
            const time = this.dataset.time;
            const today = new Date();
            today.setDate(today.getDate() + 1);
            const dateStr = today.toISOString().split('T')[0];
            dateInput.value = `${dateStr}T${time}`;
            dateInput.dispatchEvent(new Event('change'));

            timeSlots.forEach(s => s.classList.remove('selected'));
            this.classList.add('selected');
        });
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const nameInput = document.getElementById('Username');
    const dateInput = document.getElementById('reservationDateTime');
    const guestsInput = document.getElementById('guestsInput');
    const btnDecrease = document.getElementById('btnDecrease');
    const btnIncrease = document.getElementById('btnIncrease');
    const btnDelete = document.getElementById('btnDelete');
    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));

    // Actualizar resumen en tiempo real
    nameInput.addEventListener('input', function () {
        document.getElementById('summaryName').textContent = this.value || '-';
    });

    dateInput.addEventListener('change', function () {
        const date = new Date(this.value);
        document.getElementById('summaryDate').textContent = date.toLocaleDateString('es-ES', {
            day: '2-digit', month: 'short', year: 'numeric'
        });
        document.getElementById('summaryTime').textContent = date.toLocaleTimeString('es-ES', {
            hour: '2-digit', minute: '2-digit'
        });
    });

    guestsInput.addEventListener('change', function () {
        document.getElementById('summaryGuests').textContent = this.value;
    });

    // Botones +/-
    btnDecrease.addEventListener('click', function () {
        if (guestsInput.value > 1) {
            guestsInput.value = parseInt(guestsInput.value) - 1;
            guestsInput.dispatchEvent(new Event('change'));
        }
    });

    btnIncrease.addEventListener('click', function () {
        if (guestsInput.value < 20) {
            guestsInput.value = parseInt(guestsInput.value) + 1;
            guestsInput.dispatchEvent(new Event('change'));
        }
    });

    // Botón eliminar
    btnDelete.addEventListener('click', function () {
        deleteModal.show();
    });
});