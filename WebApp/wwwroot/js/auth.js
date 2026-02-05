console.log("Auth script loaded.");
setJwtToken("");

const loginForm = document.getElementById("loginForm");
const passwordInput = document.getElementById("loginPassword");

loginForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = passwordInput.value;

  console.log("Email:", email);
  console.log("Password:", password);

  if (email && password) {
        const res = await fetch("https://localhost:7000/api/Auth/login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          email,
          password
        })
      });

      if (!res.ok) {
        alert("Invalid email or password");
        return;
      }

      const token = await res.text(); //.json();
      setJwtToken(token);
      
      // redirect if needed
      window.location.href = "/Dashboard/Index";
  }

  //await login(email, password);
});

