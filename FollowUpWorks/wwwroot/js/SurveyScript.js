/* ========== SURVEY SCRIPTS ========== */

// ========== INDEX PAGE ==========
function initIndexPage() {
    var searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keyup', function () {
            var filter = this.value.toLowerCase();
            var rows = document.querySelectorAll('#surveysTable tbody tr');
            rows.forEach(function (row) {
                var text = row.textContent.toLowerCase();
                row.style.display = text.includes(filter) ? '' : 'none';
            });
        });
    }
}

// ========== CREATE/EDIT PAGE ==========
var SurveyEditor = {
    questions: [],
    editingIndex: -1,
    modalInstance: null,

    init: function (existingQuestions) {
        var self = this;
        // Normalizar propiedades (puede venir en PascalCase del servidor)
        this.questions = (existingQuestions || []).map(function (q) {
            return {
                id: q.id || q.Id || self.generateId(),
                questionText: q.questionText || q.QuestionText || '',
                type: q.type || q.Type || 'SingleChoice',
                isRequired: q.isRequired !== undefined ? q.isRequired : (q.IsRequired !== undefined ? q.IsRequired : true),
                options: q.options || q.Options || []
            };
        });
        this.bindEvents();
        this.render();
    },

    bindEvents: function () {
        var self = this;

        // Question type change
        var typeSelect = document.getElementById('questionType');
        if (typeSelect) {
            typeSelect.addEventListener('change', function () {
                var optionsSection = document.getElementById('optionsSection');
                var showOptions = this.value === 'SingleChoice' || this.value === 'MultipleChoice';
                optionsSection.style.display = showOptions ? 'block' : 'none';
            });
        }

        // Add option button
        var addOptBtn = document.getElementById('addOptionBtn');
        if (addOptBtn) {
            addOptBtn.addEventListener('click', function () {
                self.addOption();
            });
        }

        // Remove option (delegated)
        var optContainer = document.getElementById('optionsContainer');
        if (optContainer) {
            optContainer.addEventListener('click', function (e) {
                if (e.target.closest('.remove-option')) {
                    var container = document.getElementById('optionsContainer');
                    if (container.children.length > 2) {
                        e.target.closest('.input-group').remove();
                        self.updateOptionNumbers();
                    }
                }
            });
        }

        // Save question button
        var saveBtn = document.getElementById('saveQuestionBtn');
        if (saveBtn) {
            saveBtn.addEventListener('click', function () {
                self.saveQuestion();
            });
        }

        // Modal events
        var modalEl = document.getElementById('questionModal');
        if (modalEl) {
            modalEl.addEventListener('hidden.bs.modal', function () {
                self.resetModal();
                self.cleanupModal();
            });
        }

        // Form submit
        var form = document.getElementById('surveyForm');
        if (form) {
            form.addEventListener('submit', function (e) {
                var json = JSON.stringify(self.questions);
                document.getElementById('questionsJson').value = json;
                console.log('Submitting questions:', json);
            });
        }
    },

    addOption: function () {
        var container = document.getElementById('optionsContainer');
        var count = container.children.length + 1;
        var div = document.createElement('div');
        div.className = 'input-group mb-2';
        div.innerHTML = '<span class="input-group-text">' + count + '</span>' +
            '<input type="text" class="form-control option-input" placeholder="Opción ' + count + '">' +
            '<button type="button" class="btn btn-outline-danger remove-option">' +
            '<i class="bi bi-x"></i></button>';
        container.appendChild(div);
    },

    updateOptionNumbers: function () {
        var groups = document.querySelectorAll('#optionsContainer .input-group');
        groups.forEach(function (g, i) {
            g.querySelector('.input-group-text').textContent = i + 1;
            g.querySelector('input').placeholder = 'Opción ' + (i + 1);
        });
    },

    saveQuestion: function () {
        var text = document.getElementById('questionText').value.trim();
        if (!text) {
            alert('Ingresa el texto de la pregunta');
            return;
        }

        var type = document.getElementById('questionType').value;
        var required = document.getElementById('questionRequired').value === 'true';
        var options = [];

        if (type === 'SingleChoice' || type === 'MultipleChoice') {
            var inputs = document.querySelectorAll('#optionsContainer .option-input');
            inputs.forEach(function (input) {
                var val = input.value.trim();
                if (val) options.push(val);
            });
            if (options.length < 2) {
                alert('Agrega al menos 2 opciones');
                return;
            }
        }

        var question = {
            id: this.editingIndex >= 0 ? this.questions[this.editingIndex].id : this.generateId(),
            questionText: text,
            type: type,
            isRequired: required,
            options: options
        };

        if (this.editingIndex >= 0) {
            this.questions[this.editingIndex] = question;
        } else {
            this.questions.push(question);
        }

        this.editingIndex = -1;
        console.log('Question saved:', question);
        console.log('All questions:', this.questions);

        this.render();
        this.hideModal();
    },

    render: function () {
        var container = document.getElementById('questionsContainer');
        var emptyState = document.getElementById('emptyState');

        if (!container) return;

        if (this.questions.length === 0) {
            if (emptyState) emptyState.style.display = 'block';
            // Limpiar todo excepto emptyState
            var children = Array.from(container.children);
            children.forEach(function (child) {
                if (child.id !== 'emptyState') {
                    container.removeChild(child);
                }
            });
            return;
        }

        if (emptyState) emptyState.style.display = 'none';

        // Crear HTML de las preguntas
        var html = '';
        var self = this;

        this.questions.forEach(function (q, i) {
            var questionText = self.escapeHtml(q.questionText || '');
            var typeLabel = self.getTypeLabel(q.type);
            var requiredBadge = q.isRequired ? '<span class="badge bg-danger ms-2">Obligatoria</span>' : '';

            var optionsHtml = '';
            if (q.options && q.options.length > 0) {
                optionsHtml = '<div class="ps-3 mt-2">';
                q.options.forEach(function (opt) {
                    optionsHtml += '<small class="d-block text-muted"><i class="bi bi-circle me-1"></i>' + self.escapeHtml(opt) + '</small>';
                });
                optionsHtml += '</div>';
            }

            html += '<div class="card mb-3 question-card">' +
                '<div class="card-body">' +
                '<div class="d-flex justify-content-between align-items-start">' +
                '<div class="flex-grow-1">' +
                '<div class="d-flex align-items-center mb-2">' +
                '<span class="badge bg-primary me-2">' + (i + 1) + '</span>' +
                '<h6 class="mb-0">' + questionText + '</h6>' +
                requiredBadge +
                '</div>' +
                '<span class="badge bg-secondary">' + typeLabel + '</span>' +
                optionsHtml +
                '</div>' +
                '<div class="d-flex gap-1">' +
                '<button type="button" class="btn btn-sm btn-outline-primary btn-edit-question" data-index="' + i + '"><i class="bi bi-pencil"></i></button>' +
                '<button type="button" class="btn btn-sm btn-outline-danger btn-delete-question" data-index="' + i + '"><i class="bi bi-trash"></i></button>' +
                '</div></div></div></div>';
        });

        // Actualizar contenedor
        var tempDiv = document.createElement('div');
        tempDiv.innerHTML = html;

        // Remover cards anteriores
        var oldCards = container.querySelectorAll('.question-card');
        oldCards.forEach(function (card) { card.remove(); });

        // Agregar nuevas cards
        while (tempDiv.firstChild) {
            container.appendChild(tempDiv.firstChild);
        }

        // Bind eventos de editar/eliminar
        this.bindQuestionButtons();
    },

    bindQuestionButtons: function () {
        var self = this;

        document.querySelectorAll('.btn-edit-question').forEach(function (btn) {
            btn.onclick = function () {
                var index = parseInt(this.getAttribute('data-index'));
                self.edit(index);
            };
        });

        document.querySelectorAll('.btn-delete-question').forEach(function (btn) {
            btn.onclick = function () {
                var index = parseInt(this.getAttribute('data-index'));
                self.deleteQuestion(index);
            };
        });
    },

    edit: function (index) {
        var q = this.questions[index];
        if (!q) return;

        this.editingIndex = index;

        document.getElementById('questionText').value = q.questionText || '';
        document.getElementById('questionType').value = q.type || 'SingleChoice';
        document.getElementById('questionRequired').value = q.isRequired ? 'true' : 'false';

        // Mostrar/ocultar opciones
        var optionsSection = document.getElementById('optionsSection');
        var showOptions = q.type === 'SingleChoice' || q.type === 'MultipleChoice';
        optionsSection.style.display = showOptions ? 'block' : 'none';

        // Cargar opciones
        var container = document.getElementById('optionsContainer');
        if (q.options && q.options.length > 0) {
            var html = '';
            q.options.forEach(function (opt, i) {
                html += '<div class="input-group mb-2">' +
                    '<span class="input-group-text">' + (i + 1) + '</span>' +
                    '<input type="text" class="form-control option-input" value="' + opt + '">' +
                    '<button type="button" class="btn btn-outline-danger remove-option"><i class="bi bi-x"></i></button>' +
                    '</div>';
            });
            container.innerHTML = html;
        } else {
            this.resetOptionsContainer();
        }

        this.showModal();
    },

    deleteQuestion: function (index) {
        if (confirm('¿Eliminar esta pregunta?')) {
            this.questions.splice(index, 1);
            this.render();
        }
    },

    showModal: function () {
        var modalEl = document.getElementById('questionModal');
        if (!modalEl) return;

        this.cleanupModal();
        this.modalInstance = new bootstrap.Modal(modalEl);
        this.modalInstance.show();
    },

    hideModal: function () {
        if (this.modalInstance) {
            this.modalInstance.hide();
        }
        this.cleanupModal();
    },

    cleanupModal: function () {
        var self = this;
        setTimeout(function () {
            document.querySelectorAll('.modal-backdrop').forEach(function (el) { el.remove(); });
            document.body.classList.remove('modal-open');
            document.body.style.overflow = '';
            document.body.style.paddingRight = '';
        }, 300);
    },

    resetModal: function () {
        this.editingIndex = -1;
        document.getElementById('questionText').value = '';
        document.getElementById('questionType').value = 'SingleChoice';
        document.getElementById('questionRequired').value = 'true';
        document.getElementById('optionsSection').style.display = 'block';
        this.resetOptionsContainer();
    },

    resetOptionsContainer: function () {
        document.getElementById('optionsContainer').innerHTML =
            '<div class="input-group mb-2"><span class="input-group-text">1</span>' +
            '<input type="text" class="form-control option-input" placeholder="Opción 1">' +
            '<button type="button" class="btn btn-outline-danger remove-option"><i class="bi bi-x"></i></button></div>' +
            '<div class="input-group mb-2"><span class="input-group-text">2</span>' +
            '<input type="text" class="form-control option-input" placeholder="Opción 2">' +
            '<button type="button" class="btn btn-outline-danger remove-option"><i class="bi bi-x"></i></button></div>';
    },

    getTypeLabel: function (type) {
        var labels = {
            'SingleChoice': 'Opción Única',
            'MultipleChoice': 'Opción Múltiple',
            'Text': 'Texto Libre',
            'Rating': 'Calificación'
        };
        return labels[type] || type;
    },

    generateId: function () {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0;
            var v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    },

    escapeHtml: function (text) {
        if (!text) return '';
        var div = document.createElement('div');
        div.textContent = text;
        return div.innerHTML;
    }
};

// ========== RESPOND PAGE ==========
var SurveyRespond = {
    surveyId: '',
    totalQuestions: 0,
    submitUrl: '',

    init: function (config) {
        console.log('SurveyRespond.init called with:', config);
        this.surveyId = config.surveyId;
        this.totalQuestions = config.totalQuestions;
        this.submitUrl = config.submitUrl;
        this.bindEvents();
        this.updateProgress();
        console.log('SurveyRespond initialized');
    },

    bindEvents: function () {
        var self = this;

        document.querySelectorAll('.respond-question-card input, .respond-question-card textarea').forEach(function (el) {
            el.addEventListener('change', function () { self.updateProgress(); });
            el.addEventListener('input', function () { self.updateProgress(); });
        });
    },

    updateProgress: function () {
        var answered = 0;
        var self = this;

        document.querySelectorAll('.respond-question-card').forEach(function (card) {
            var type = card.getAttribute('data-type');
            var hasAnswer = false;

            if (type === 'Text') {
                var textarea = card.querySelector('textarea');
                hasAnswer = textarea && textarea.value.trim() !== '';
            } else if (type === 'SingleChoice' || type === 'Rating') {
                hasAnswer = card.querySelector('input[type="radio"]:checked') !== null;
            } else if (type === 'MultipleChoice') {
                hasAnswer = card.querySelector('input[type="checkbox"]:checked') !== null;
            }

            if (hasAnswer) {
                card.classList.add('answered');
                answered++;
            } else {
                card.classList.remove('answered');
            }
        });

        var percent = this.totalQuestions > 0 ? (answered / this.totalQuestions) * 100 : 0;
        var progressBar = document.getElementById('progressBar');
        var answeredCount = document.getElementById('answeredCount');

        if (progressBar) progressBar.style.width = percent + '%';
        if (answeredCount) answeredCount.textContent = answered;
    },

    submit: function () {
        console.log('SurveyRespond.submit called');
        console.log('Submit URL:', this.submitUrl);
        console.log('Survey ID:', this.surveyId);

        var self = this;
        var valid = true;

        // Validate required
        document.querySelectorAll('.respond-question-card').forEach(function (card) {
            card.classList.remove('error');
            var required = card.getAttribute('data-required') === 'true';
            if (!required) return;

            var type = card.getAttribute('data-type');
            var hasAnswer = false;

            if (type === 'Text') {
                var textarea = card.querySelector('textarea');
                hasAnswer = textarea && textarea.value.trim() !== '';
            } else if (type === 'SingleChoice' || type === 'Rating') {
                hasAnswer = card.querySelector('input[type="radio"]:checked') !== null;
            } else if (type === 'MultipleChoice') {
                hasAnswer = card.querySelector('input[type="checkbox"]:checked') !== null;
            }

            if (!hasAnswer) {
                card.classList.add('error');
                valid = false;
            }
        });

        if (!valid) {
            var errorCard = document.querySelector('.respond-question-card.error');
            if (errorCard) {
                errorCard.scrollIntoView({ behavior: 'smooth', block: 'center' });
            }
            alert('Por favor responde todas las preguntas obligatorias');
            return;
        }

        // Collect answers
        var answers = [];
        document.querySelectorAll('.respond-question-card').forEach(function (card) {
            var qId = card.getAttribute('data-question-id');
            var type = card.getAttribute('data-type');
            var answer = {
                questionId: qId,
                selectedOptions: [],
                textAnswer: '',
                ratingValue: null
            };

            if (type === 'Text') {
                var textarea = card.querySelector('textarea');
                answer.textAnswer = textarea ? textarea.value.trim() : '';
            } else if (type === 'SingleChoice') {
                var checked = card.querySelector('input[type="radio"]:checked');
                if (checked) answer.selectedOptions = [parseInt(checked.value)];
            } else if (type === 'MultipleChoice') {
                var checkedBoxes = card.querySelectorAll('input[type="checkbox"]:checked');
                checkedBoxes.forEach(function (cb) {
                    answer.selectedOptions.push(parseInt(cb.value));
                });
            } else if (type === 'Rating') {
                var checkedRating = card.querySelector('input[type="radio"]:checked');
                if (checkedRating) answer.ratingValue = parseInt(checkedRating.value);
            }

            answers.push(answer);
        });

        console.log('Submitting answers:', answers);
        console.log('Answers JSON:', JSON.stringify(answers));

        // Submit
        var btn = document.getElementById('submitBtn');
        if (!btn) {
            console.error('Submit button not found');
            return;
        }

        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Enviando...';

        var tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
        if (!tokenInput) {
            console.error('Token not found');
            alert('Error: Token de seguridad no encontrado');
            btn.disabled = false;
            btn.innerHTML = '<i class="bi bi-send me-2"></i>Enviar Respuestas';
            return;
        }

        var token = tokenInput.value;
        console.log('Token found:', token ? 'Yes' : 'No');
        var body = 'surveyId=' + encodeURIComponent(this.surveyId) +
            '&answersJson=' + encodeURIComponent(JSON.stringify(answers)) +
            '&__RequestVerificationToken=' + encodeURIComponent(token);

        console.log('Sending fetch to:', this.submitUrl);

        fetch(this.submitUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
                'RequestVerificationToken': token
            },
            body: body
        })
            .then(function (response) {
                console.log('Response status:', response.status);
                return response.json();
            })
            .then(function (result) {
                console.log('Response result:', result);
                if (result.success) {
                    window.location.href = result.redirectUrl;
                } else {
                    alert(result.message || 'Error al enviar');
                    btn.disabled = false;
                    btn.innerHTML = '<i class="bi bi-send me-2"></i>Enviar Respuestas';
                }
            })
            .catch(function (err) {
                console.error('Fetch error:', err);
                alert('Error de conexión: ' + err.message);
                btn.disabled = false;
                btn.innerHTML = '<i class="bi bi-send me-2"></i>Enviar Respuestas';
            });
    }
};

// ========== DASHBOARD PAGE ==========
var SurveyDashboard = {
    init: function (questionStats) {
        if (!questionStats || !questionStats.length) return;
        this.renderCharts(questionStats);
    },

    renderCharts: function (questionStats) {
        var self = this;

        questionStats.forEach(function (q) {
            // Normalizar propiedades
            var questionId = q.questionId || q.QuestionId || '';
            var type = q.type || q.Type || '';
            var optionCounts = q.optionCounts || q.OptionCounts || {};

            var canvasId = 'chart_' + questionId.replace(/-/g, '');
            var canvas = document.getElementById(canvasId);
            if (!canvas) return;

            if (type === 'SingleChoice' || type === 'MultipleChoice') {
                var labels = Object.keys(optionCounts);
                var data = Object.values(optionCounts);
                var colors = self.generateColors(labels.length);

                new Chart(canvas, {
                    type: 'doughnut',
                    data: {
                        labels: labels,
                        datasets: [{
                            data: data,
                            backgroundColor: colors,
                            borderWidth: 2,
                            borderColor: '#fff'
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            legend: { position: 'bottom', labels: { boxWidth: 12, padding: 15 } }
                        }
                    }
                });
            } else if (type === 'Rating') {
                var ratingLabels = ['1 ⭐', '2 ⭐', '3 ⭐', '4 ⭐', '5 ⭐'];
                var ratingData = [1, 2, 3, 4, 5].map(function (i) {
                    return optionCounts[i.toString()] || optionCounts[i] || 0;
                });

                new Chart(canvas, {
                    type: 'bar',
                    data: {
                        labels: ratingLabels,
                        datasets: [{
                            label: 'Respuestas',
                            data: ratingData,
                            backgroundColor: ['#dc3545', '#fd7e14', '#ffc107', '#20c997', '#198754'],
                            borderRadius: 5
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: { legend: { display: false } },
                        scales: { y: { beginAtZero: true, ticks: { stepSize: 1 } } }
                    }
                });
            }
        });
    },

    generateColors: function (count) {
        var baseColors = ['#667eea', '#764ba2', '#11998e', '#38ef7d', '#4facfe',
            '#00f2fe', '#f093fb', '#f5576c', '#ffecd2', '#fcb69f'];
        var colors = [];
        for (var i = 0; i < count; i++) {
            colors.push(baseColors[i % baseColors.length]);
        }
        return colors;
    }
};