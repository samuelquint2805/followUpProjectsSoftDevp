document.addEventListener('DOMContentLoaded', function () {
    // Variables del cronómetro
    let startTime = 0;
    let elapsedTime = 0;
    let timerInterval = null;
    let isRunning = false;
    let laps = [];
    let lastLapTime = 0;

    // Elementos del DOM
    const hoursEl = document.getElementById('hours');
    const minutesEl = document.getElementById('minutes');
    const secondsEl = document.getElementById('seconds');
    const millisecondsEl = document.getElementById('milliseconds');
    const displayEl = document.getElementById('display');

    const btnStart = document.getElementById('btnStart');
    const btnPause = document.getElementById('btnPause');
    const btnReset = document.getElementById('btnReset');
    const btnLap = document.getElementById('btnLap');
    const btnClearLaps = document.getElementById('btnClearLaps');

    const lapsContainer = document.getElementById('lapsContainer');
    const lapsList = document.getElementById('lapsList');
    const lapCount = document.getElementById('lapCount');
    const statsContainer = document.getElementById('statsContainer');

    // Formatear tiempo
    function formatTime(ms) {
        const totalSeconds = Math.floor(ms / 1000);
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;
        const milliseconds = Math.floor((ms % 1000) / 10);

        return {
            hours: hours.toString().padStart(2, '0'),
            minutes: minutes.toString().padStart(2, '0'),
            seconds: seconds.toString().padStart(2, '0'),
            milliseconds: milliseconds.toString().padStart(2, '0'),
            formatted: `${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}.${milliseconds.toString().padStart(2, '0')}`,
            full: `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}:${seconds.toString().padStart(2, '0')}`
        };
    }

    // Actualizar display
    function updateDisplay() {
        const time = formatTime(elapsedTime);
        hoursEl.textContent = time.hours;
        minutesEl.textContent = time.minutes;
        secondsEl.textContent = time.seconds;
        millisecondsEl.textContent = time.milliseconds;
    }

    // Iniciar cronómetro
    function start() {
        if (!isRunning) {
            startTime = Date.now() - elapsedTime;
            timerInterval = setInterval(function () {
                elapsedTime = Date.now() - startTime;
                updateDisplay();
            }, 10);
            isRunning = true;

            btnStart.disabled = true;
            btnPause.disabled = false;
            btnReset.disabled = false;
            btnLap.disabled = false;

            displayEl.classList.remove('paused');
            displayEl.classList.add('running');

            btnStart.innerHTML = '<i class="bi bi-play-fill"></i> Iniciar';
        }
    }

    // Pausar cronómetro
    function pause() {
        if (isRunning) {
            clearInterval(timerInterval);
            isRunning = false;

            btnStart.disabled = false;
            btnPause.disabled = true;
            btnLap.disabled = true;

            displayEl.classList.remove('running');
            displayEl.classList.add('paused');

            btnStart.innerHTML = '<i class="bi bi-play-fill"></i> Continuar';
        }
    }

    // Reiniciar cronómetro
    function reset() {
        clearInterval(timerInterval);
        isRunning = false;
        elapsedTime = 0;
        laps = [];
        lastLapTime = 0;

        updateDisplay();

        btnStart.disabled = false;
        btnPause.disabled = true;
        btnReset.disabled = true;
        btnLap.disabled = true;

        displayEl.classList.remove('running', 'paused');

        btnStart.innerHTML = '<i class="bi bi-play-fill"></i> Iniciar';

        lapsList.innerHTML = '';
        lapsContainer.style.display = 'none';
        statsContainer.style.display = 'none';
        lapCount.textContent = '0';
    }

    // Limpiar solo las vueltas
    function clearLaps() {
        laps = [];
        lastLapTime = 0;
        lapsList.innerHTML = '';
        lapsContainer.style.display = 'none';
        statsContainer.style.display = 'none';
        lapCount.textContent = '0';
    }

    // Registrar vuelta
    function recordLap() {
        const lapTime = elapsedTime - lastLapTime;
        lastLapTime = elapsedTime;

        laps.push({
            number: laps.length + 1,
            lapTime: lapTime,
            totalTime: elapsedTime
        });

        updateLapsDisplay();
    }

    // Actualizar estadísticas
    function updateStats() {
        if (laps.length < 2) {
            statsContainer.style.display = 'none';
            return;
        }

        statsContainer.style.display = 'flex';

        const lapTimes = laps.map(l => l.lapTime);
        const best = Math.min(...lapTimes);
        const worst = Math.max(...lapTimes);
        const avg = lapTimes.reduce((a, b) => a + b, 0) / lapTimes.length;

        document.getElementById('bestLap').textContent = formatTime(best).formatted;
        document.getElementById('worstLap').textContent = formatTime(worst).formatted;
        document.getElementById('avgLap').textContent = formatTime(avg).formatted;
    }

    // Actualizar lista de vueltas
    function updateLapsDisplay() {
        if (laps.length === 0) {
            lapsContainer.style.display = 'none';
            return;
        }

        lapsContainer.style.display = 'block';
        lapCount.textContent = laps.length;

        // Encontrar mejor y peor vuelta
        const lapTimes = laps.map(l => l.lapTime);
        const bestLap = Math.min(...lapTimes);
        const worstLap = Math.max(...lapTimes);

        lapsList.innerHTML = '';

        // Mostrar en orden inverso (más reciente primero)
        [...laps].reverse().forEach(lap => {
            const lapDiv = document.createElement('div');
            lapDiv.className = 'lap-item';

            if (laps.length > 1) {
                if (lap.lapTime === bestLap) lapDiv.classList.add('best');
                if (lap.lapTime === worstLap) lapDiv.classList.add('worst');
            }

            const time = formatTime(lap.lapTime);
            const totalTime = formatTime(lap.totalTime);

            lapDiv.innerHTML = `
                        <span class="lap-number">Vuelta ${lap.number}</span>
                        <span class="lap-time">${time.formatted}</span>
                        <span class="lap-diff text-muted">
                            <small>Total: ${totalTime.full}</small>
                        </span>
                    `;

            lapsList.appendChild(lapDiv);
        });

        updateStats();
    }

    // Event Listeners
    btnStart.addEventListener('click', start);
    btnPause.addEventListener('click', pause);
    btnReset.addEventListener('click', reset);
    btnLap.addEventListener('click', recordLap);
    btnClearLaps.addEventListener('click', clearLaps);

    // Atajos de teclado
    document.addEventListener('keydown', function (e) {
        if (e.target.tagName === 'INPUT' || e.target.tagName === 'TEXTAREA') return;

        switch (e.code) {
            case 'Space':
                e.preventDefault();
                if (isRunning) pause();
                else if (!btnStart.disabled) start();
                break;
            case 'KeyR':
                if (!btnReset.disabled) reset();
                break;
            case 'KeyL':
                if (!btnLap.disabled) recordLap();
                break;
        }
    });
});