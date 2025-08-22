let secretNumber = Math.floor(Math.random() * 20) + 1;
let attempts = 5;

function checkGuess() {
  let userGuess = parseInt(document.getElementById("userGuess").value);
  let result = document.getElementById("result");

  if (attempts <= 0) {
    result.textContent = "💀 Game Over! No attempts left.";
    return;
  }

  if (userGuess === secretNumber) {
    result.textContent = "🎉 Correct! You guessed the number!";
  } else if (userGuess > secretNumber) {
    attempts--;
    result.textContent = `Too high! Attempts left: ${attempts}`;
  } else {
    attempts--;
    result.textContent = `Too low! Attempts left: ${attempts}`;
  }

  if (attempts === 0 && userGuess !== secretNumber) {
    result.textContent = `💀 Game Over! The number was ${secretNumber}.`;
  }
}
