const MAX_ATTEMPTS = 6;
const min = 1, max = 10;
const correctNumber = Math.floor(Math.random() * (max - min + 1)) + min;

let attempts = 0;
let gameOver = false;
let ballStatuses = {}; // Track the state (color) of each guessed ball

const guessSlider = document.getElementById('guessSlider');
const numberDisplay = document.getElementById('numberDisplay');
const attemptDisplay = document.getElementById('attemptDisplay');
const resultDisplay = document.getElementById('resultDisplay');
let balls = [];

function renderBalls() {
    numberDisplay.innerHTML = '';
    balls = [];
    for (let i = min; i <= max; i++) {
        const ball = document.createElement('div');
        ball.className = 'number-ball';
        ball.textContent = i;
        balls.push(ball);
        numberDisplay.appendChild(ball);
    }
}
renderBalls();

function updateBallColors() {
    balls.forEach((ball, idx) => {
        ball.className = 'number-ball'; // Reset
        let n = idx + 1;
        if (ballStatuses[n]) {
            ball.classList.add(ballStatuses[n]); // Applies .yellow, .red, or .green
        }
    });
}

function endGame(message) {
    resultDisplay.innerHTML = message;
    gameOver = true;
    guessSlider.disabled = true;
}

function handleGuess() {
    if (gameOver) return;
    const guess = parseInt(guessSlider.value, 10);
    attempts++;

    let status = '';
    if (guess < correctNumber) {
        status = 'yellow';
    } else if (guess > correctNumber) {
        status = 'red';
    } else {
        status = 'green';
    }
    ballStatuses[guess] = status;
    updateBallColors();

    attemptDisplay.innerText = `Attempt No. ${attempts}`;

    if (status === 'green') {
        endGame(`You Win! Correct number guessed in ${attempts} attempts !`);
    } else if (attempts >= MAX_ATTEMPTS) {
        endGame(`Game Over! Correct number was ${correctNumber}.`);
    }
}

// Highlight current selection with border (not color) as slider moves
guessSlider.addEventListener('input', () => {
    updateBallColors();
    if (!gameOver) {
        balls.forEach((ball, idx) => {
            if (parseInt(guessSlider.value, 10) === idx + 1) {
                ball.style.border = '3px solid #555';
            } else {
                ball.style.border = '';
            }
        });
    }
});

// Color the ball on slider release (change event)
guessSlider.addEventListener('change', () => {
    balls.forEach(ball => ball.style.border = '');
    handleGuess();
});

guessSlider.value = min;
updateBallColors();
