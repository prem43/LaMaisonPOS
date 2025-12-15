// Additional JavaScript functionality can be added here
console.log("La Maison POS System Loaded")

// Keyboard shortcuts
document.addEventListener("keydown", (e) => {
  if (e.key === "F9") {
    e.preventDefault()
    document.querySelector('input[name="searchTerm"]')?.focus()
  }
})
