document.addEventListener('DOMContentLoaded', function () {
    // Filtros
    const searchName = document.getElementById('searchName');
    const filterDishType = document.getElementById('filterDishType');
    const filterIngredientCategory = document.getElementById('filterIngredientCategory');
    const filterIngredient = document.getElementById('filterIngredient');
    const filterTime = document.getElementById('filterTime');
    const btnClearFilters = document.getElementById('btnClearFilters');
    const rows = document.querySelectorAll('.recipe-row');
    const noResultsMessage = document.getElementById('noResultsMessage');

    function applyFilters() {
        const nameValue = searchName.value.toLowerCase();
        const dishTypeValue = filterDishType.value;
        const categoryValue = filterIngredientCategory.value;
        const ingredientValue = filterIngredient.value.toLowerCase();
        const timeValue = filterTime.value ? parseInt(filterTime.value) : null;

        let visibleCount = 0;

        rows.forEach(row => {
            let show = true;

            if (nameValue && !row.dataset.name.includes(nameValue)) show = false;
            if (dishTypeValue && row.dataset.dishtype !== dishTypeValue) show = false;
            if (categoryValue && row.dataset.category !== categoryValue) show = false;
            if (ingredientValue && !row.dataset.ingredients.includes(ingredientValue)) show = false;
            if (timeValue && parseInt(row.dataset.totaltime) > timeValue) show = false;

            row.style.display = show ? '' : 'none';
            if (show) visibleCount++;
        });

        noResultsMessage.style.display = visibleCount === 0 && rows.length > 0 ? 'block' : 'none';
    }

    searchName.addEventListener('input', applyFilters);
    filterDishType.addEventListener('change', applyFilters);
    filterIngredientCategory.addEventListener('change', applyFilters);
    filterIngredient.addEventListener('input', applyFilters);
    filterTime.addEventListener('input', applyFilters);

    btnClearFilters.addEventListener('click', function () {
        searchName.value = '';
        filterDishType.value = '';
        filterIngredientCategory.value = '';
        filterIngredient.value = '';
        filterTime.value = '';
        applyFilters();
    });
    // Expandir/colapsar instrucciones al hacer clic en la fila
    rows.forEach(row => {
        row.addEventListener('click', function (e) {
            // No expandir si se hizo clic en un botón o enlace
            if (e.target.closest('a') || e.target.closest('button')) {
                return;
            }

            const targetId = this.dataset.target;
            const instructionsRow = document.getElementById(targetId);

            if (instructionsRow) {
                const isVisible = instructionsRow.style.display !== 'none';
                instructionsRow.style.display = isVisible ? 'none' : 'table-row';
                this.classList.toggle('expanded', !isVisible);
            }
        });
    });
    // Modal de eliminación
    const deleteModalElement = document.getElementById('deleteModal');
    const deleteModal = new bootstrap.Modal(deleteModalElement);
    const deleteForm = document.getElementById('deleteForm');
    const recipeNameToDelete = document.getElementById('recipeNameToDelete');

    // Obtener la URL base desde el data attribute del modal
    const baseDeleteUrl = deleteModalElement.dataset.deleteUrl;

    document.querySelectorAll('.btn-delete').forEach(btn => {
        btn.addEventListener('click', function () {
            const id = this.dataset.id;
            const name = this.dataset.name;
            recipeNameToDelete.textContent = name;
            deleteForm.action = baseDeleteUrl + '/' + id;
            deleteModal.show();
        });
    });
   
});

document.addEventListener('DOMContentLoaded', function () {
    const container = document.getElementById('ingredientsContainer');
    const btnAdd = document.getElementById('btnAddIngredient');
    const prepTime = document.querySelector('input[name="PreparationTimeMinutes"]');
    const cookTime = document.querySelector('input[name="CookingTimeMinutes"]');
    const totalTimeDisplay = document.getElementById('totalTime');
    const btnDelete = document.getElementById('btnDelete');
    const deleteModal = new bootstrap.Modal(document.getElementById('deleteModal'));

    // Agregar ingrediente
    btnAdd.addEventListener('click', function () {
        const newRow = document.createElement('div');
        newRow.className = 'input-group mb-2 ingredient-row';
        newRow.innerHTML = `
                    <input type="text" name="Ingredients" class="form-control" placeholder="Ej: 100g de queso" />
                    <button type="button" class="btn btn-outline-danger btn-remove-ingredient">
                        <i class="bi bi-x"></i>
                    </button>
                `;
        container.appendChild(newRow);
        updateRemoveButtons();
        newRow.querySelector('input').focus();
    });

    // Eliminar ingrediente
    container.addEventListener('click', function (e) {
        if (e.target.closest('.btn-remove-ingredient')) {
            const row = e.target.closest('.ingredient-row');
            if (container.querySelectorAll('.ingredient-row').length > 1) {
                row.remove();
                updateRemoveButtons();
            }
        }
    });

    function updateRemoveButtons() {
        const rows = container.querySelectorAll('.ingredient-row');
        rows.forEach(row => {
            const btn = row.querySelector('.btn-remove-ingredient');
            btn.disabled = rows.length === 1;
        });
    }

    // Calcular tiempo total
    function updateTotalTime() {
        const prep = parseInt(prepTime.value) || 0;
        const cook = parseInt(cookTime.value) || 0;
        totalTimeDisplay.textContent = prep + cook;
    }

    prepTime.addEventListener('input', updateTotalTime);
    cookTime.addEventListener('input', updateTotalTime);

    // Botón eliminar
    btnDelete.addEventListener('click', function () {
        deleteModal.show();
    });

    // Validación antes de enviar
    document.getElementById('recipeForm').addEventListener('submit', function (e) {
        const ingredients = document.querySelectorAll('input[name="Ingredients"]');
        let hasIngredient = false;
        ingredients.forEach(input => {
            if (input.value.trim()) hasIngredient = true;
        });

        if (!hasIngredient) {
            e.preventDefault();
            alert('Debe agregar al menos un ingrediente.');
        }
    });
});