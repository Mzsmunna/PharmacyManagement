console.log("Auth script loaded.");
setJwtToken("");

const loginForm = document.getElementById("loginForm");
const registerForm = document.getElementById("registerForm");
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

registerForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const name = document.getElementById("regName").value;
    const email = document.getElementById("regEmail").value;
    const phone = document.getElementById("regPhone").value;
    const address = document.getElementById("regAddrs").value;
    const password = document.getElementById("regPassword").value;
    const confirmPassword = document.getElementById("regConPassword").value;

    console.log("Password:", password);
    console.log("ConfirmPassword:", confirmPassword);

    if (!password || !confirmPassword) return;

    if (password == confirmPassword) {      
        var payload = {
            name: name,
            email: email,
            phone: phone,
            address: address,
            password: password,
            confirmPassword: confirmPassword,
        };
        const res = await apiRequest("POST", authUrl + "Register", payload);
        if (!res.ok) {
            alert("Invalid email or password");
            return;
        }
        const token = await res.text(); //.json();
        setJwtToken(token);
        isAuthenticated();  
        // redirect if needed
        //window.location.href = "/Dashboard/Index";
    }
});

