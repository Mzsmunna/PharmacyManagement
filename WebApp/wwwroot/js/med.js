const medsForm = document.getElementById("medsForm");
const apiUrl = "https://localhost:7000/api/Medicines/";

medsForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const medName = document.getElementById("medName").value;
  const medType = document.getElementById("medType").value;
  const medDesc = document.getElementById("medDesc").value;

  console.log("medName:", medName);
  console.log("medType:", medType);
  console.log("medDesc:", medDesc);

  if (medName && medType && medDesc) {      
        var payload = {
            Name: medName,
            Type: medType,
            Description: medDesc,
            Image: "",
        };
        const res = await apiRequest("POST", authUrl, payload);
        if (!res.ok) {
            alert("Invalid email or password");
            return;
        }
        const data = await res.json();
  }
});