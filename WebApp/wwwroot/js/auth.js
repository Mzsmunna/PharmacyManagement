console.log("Auth script loaded.");

const loginForm = document.getElementById("loginForm");
const passwordInput = document.getElementById("loginPassword");
const togglePassword = document.getElementById("togglePassword");

togglePassword.addEventListener("click", () => {
    const isPassword = passwordInput.type === "password";
    passwordInput.type = isPassword ? "text" : "password";
    //togglePassword.textContent = isPassword ? "" : "";
});


loginForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = passwordInput.value;

  console.log("Email:", email);
  console.log("Password:", password);

  //await login(email, password);
});

