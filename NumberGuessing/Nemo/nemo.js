const gameBoard = document.getElementById("game-board");
const message = document.getElementById("message");

let chances = 5;
let totalCircles = 10;

// Random position for Nemo
let nemoIndex = Math.floor(Math.random() * totalCircles);

// Create circles
for (let i = 0; i < totalCircles; i++) {
  let circle = document.createElement("div");
  circle.classList.add("circle");
  circle.dataset.index = i;

  circle.addEventListener("click", () => checkChoice(circle, i));
  gameBoard.appendChild(circle);
}

function checkChoice(circle, index) {
  if (chances <= 0 || circle.innerHTML !== "") return; // prevent extra clicks

  if (index == nemoIndex) {
    circle.innerHTML = `<img src="image.webp" alt="Nemo">`;
    message.textContent = "🎉 Congratulations, You Found Nemo!";
  } else {
    circle.innerHTML = `<span class="cross">X</span>`;
    chances--;
    if (chances > 0) {
      message.textContent = `❌ Wrong! Chances left: ${chances}`;
    } else {
      message.textContent = "💀 Game Over! Nemo escaped.";
    }
  }
}
