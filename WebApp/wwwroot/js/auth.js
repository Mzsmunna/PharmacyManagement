console.log("Auth script loaded.");
setJwtToken("");

const loginForm = document.getElementById("loginForm");
const passwordInput = document.getElementById("loginPassword");
const authUrl = "https://localhost:7000/api/Auth/";

loginForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const email = document.getElementById("email").value;
  const password = passwordInput.value;

  console.log("Email:", email);
  console.log("Password:", password);

  if (email && password) {      
        var payload = {
            email: email,
            password: password
        };
        const res = await apiRequest("POST", authUrl + "login", payload);
        if (!res.ok) {
            alert("Invalid email or password");
            return;
        }
        const token = await res.text(); //.json();
        setJwtToken(token);
        isAuthenticated();  
        // redirect if needed
        window.location.href = "/Dashboard/Index";
  }
});

