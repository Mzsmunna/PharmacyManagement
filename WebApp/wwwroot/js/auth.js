console.log("Auth script loaded.");

const loginForm = document.getElementById("loginForm");
const passwordInput = document.getElementById("loginPassword");


loginForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = passwordInput.value;

  console.log("Email:", email);
  console.log("Password:", password);

  if (!email || !password) {

  }

  //await login(email, password);
});

