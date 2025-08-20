const addBtn = document.getElementById('addBtn');
const todoInput = document.getElementById('todoInput');
const todoList = document.getElementById('todoList');

// Add Task
addBtn.addEventListener('click', () => {
  const task = todoInput.value.trim();
  if (task !== "") {
    const li = document.createElement('li');
    li.className = "todo-item";

    li.innerHTML = `
      <span>${task}</span>
      <button class="delete-btn">❌</button>
    `;

    // Delete button functionality
    li.querySelector('.delete-btn').addEventListener('click', () => {
      li.remove();
    });

    todoList.appendChild(li);
    todoInput.value = "";
  } else {
    alert("⚠️ Please enter a task!");
  }
});

// Add task on Enter key
todoInput.addEventListener("keypress", (e) => {
  if (e.key === "Enter") {
    addBtn.click();
  }
});
