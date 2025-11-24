document.addEventListener('DOMContentLoaded', function () {
    // ============ CÓDIGO PARA LA LISTA DE RESERVAS (Index) ============
    const searchName = document.getElementById('searchName');
    const filterDate = document.getElementById('filterDate');
    const filterGuests = document.getElementById('filterGuests');
    const filterStatus = document.getElementById('filterStatus');
    const btnClearFilters = document.getElementById('btnClearFilters');
    const rows = document.querySelectorAll('.reservation-row');
    const noResultsMessage = document.getElementById('noResultsMessage');

    // Solo ejecutar si estamos en la página de lista
    if (searchName && rows.length > 0) {
        function applyFilters() {
            const nameValue = searchName.value.toLowerCase();
            const dateValue = filterDate.value;
            const guestsValue = filterGuests.value;
            const statusValue = filterStatus.value;

            let visibleCount = 0;

            rows.forEach(row => {
                let show = true;

                if (nameValue && !row.dataset.name.includes(nameValue)) {
                    show = false;
                }

                if (dateValue && row.dataset.date !== dateValue) {
                    show = false;
                }

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

                if (statusValue && row.dataset.status !== statusValue) {
                    show = false;
                }

                row.style.display = show ? '' : 'none';
                if (show) visibleCount++;
            });

            if (noResultsMessage) {
                noResultsMessage.style.display = visibleCount === 0 && rows.length > 0 ? 'block' : 'none';
            }
        }

        searchName.addEventListener('input', applyFilters);
        if (filterDate) filterDate.addEventListener('change', applyFilters);
        if (filterGuests) filterGuests.addEventListener('change', applyFilters);
        if (filterStatus) filterStatus.addEventListener('change', applyFilters);

        if (btnClearFilters) {
            btnClearFilters.addEventListener('click', function () {
                searchName.value = '';
                if (filterDate) filterDate.value = '';
                if (filterGuests) filterGuests.value = '';
                if (filterStatus) filterStatus.value = '';
                applyFilters();
            });
        }

        // Modal de eliminación en lista
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

        // Animaciones de filas
        rows.forEach((row, index) => {
            row.style.opacity = '0';
            row.style.transform = 'translateY(10px)';

            setTimeout(() => {
                row.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
                row.style.opacity = '1';
                row.style.transform = 'translateY(0)';
            }, index * 50);
        });

        // Highlight de reservas de hoy
        rows.forEach(row => {
            if (row.dataset.status === 'today') {
                row.classList.add('table-success');
            }
        });
    }

    // Auto-cerrar alertas (común para todas las páginas)
    const alerts = document.querySelectorAll('.alert-dismissible');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = bootstrap.Alert.getOrCreateInstance(alert);
            if (bsAlert) {
                bsAlert.close();
            }
        }, 5000);
    });

    // Tooltip inicialización (común para todas las páginas)
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    [...tooltipTriggerList].map(el => new bootstrap.Tooltip(el));

    // ============ CÓDIGO PARA CREATE/EDIT ============
    const nameInput = document.getElementById('Username');
    const dateInput = document.getElementById('reservationDateTime');
    const guestsInput = document.getElementById('guestsInput');
    const btnDecrease = document.getElementById('btnDecrease');
    const btnIncrease = document.getElementById('btnIncrease');

    // Solo ejecutar si estamos en la página de formulario
    if (nameInput && dateInput && guestsInput && btnDecrease && btnIncrease) {

        // Establecer fecha mínima solo en CREATE (no en EDIT)
        const isCreatePage = !document.getElementById('btnDelete');
        if (isCreatePage) {
            const tomorrow = new Date();
            tomorrow.setDate(tomorrow.getDate() + 1);
            tomorrow.setHours(8, 0, 0, 0);
            dateInput.min = tomorrow.toISOString().slice(0, 16);
        }

        // Actualizar resumen en tiempo real
        nameInput.addEventListener('input', function () {
            const summaryName = document.getElementById('summaryName');
            if (summaryName) {
                summaryName.textContent = this.value || '-';
            }
        });

        dateInput.addEventListener('change', function () {
            const date = new Date(this.value);
            const summaryDate = document.getElementById('summaryDate');
            const summaryTime = document.getElementById('summaryTime');

            if (summaryDate) {
                summaryDate.textContent = date.toLocaleDateString('es-ES', {
                    day: '2-digit', month: 'short', year: 'numeric'
                });
            }
            if (summaryTime) {
                summaryTime.textContent = date.toLocaleTimeString('es-ES', {
                    hour: '2-digit', minute: '2-digit'
                });
            }
        });

        guestsInput.addEventListener('change', function () {
            const summaryGuests = document.getElementById('summaryGuests');
            if (summaryGuests) {
                summaryGuests.textContent = this.value;
            }
        });

        // Botones +/- (SOLUCIÓN AL PROBLEMA)
        btnDecrease.addEventListener('click', function () {
            const currentValue = parseInt(guestsInput.value);
            if (currentValue > 1) {
                guestsInput.value = currentValue - 1;
                guestsInput.dispatchEvent(new Event('change'));
            }
        });

        btnIncrease.addEventListener('click', function () {
            const currentValue = parseInt(guestsInput.value);
            if (currentValue < 20) {
                guestsInput.value = currentValue + 1;
                guestsInput.dispatchEvent(new Event('change'));
            }
        });

        // Selección de horario rápido (solo en CREATE)
        const timeSlots = document.querySelectorAll('.time-slot');
        if (timeSlots.length > 0) {
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
        }

        // Botón eliminar (solo en EDIT)
        const btnDelete = document.getElementById('btnDelete');
        const deleteModalElement = document.getElementById('deleteModal');

        if (btnDelete && deleteModalElement) {
            const deleteModal = new bootstrap.Modal(deleteModalElement);
            btnDelete.addEventListener('click', function () {
                deleteModal.show();
            });
        }
    }
});