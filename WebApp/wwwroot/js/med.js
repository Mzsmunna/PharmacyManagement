console.log("Medicine script loaded.");
const medsForm = document.getElementById("medsForm");
const apiUrl = "https://localhost:7000/api/Medicines/";

setTimeout(() => {
const medsBatchForm = document.getElementById("medsBatchForm");
  medsBatchForm.style.display = "none";
}, 2000);

let medId = "";
let med = null;

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
        const res = await apiRequest("POST", apiUrl, payload);
        if (!res.ok) {
            alert("Something went wrong");
            return;
        }
        medId = await res.text();
        if (medId)
        {
            med = await apiRequest("GET", apiUrl + medId);
            console.log("medicine:", med);
            medsBatchForm.style.display = "block";
        }
  }
});

medsBatchForm.addEventListener("submit", async (e) => {
  e.preventDefault();

  const medName = document.getElementById("medBatchNo").value;
  const medType = document.getElementById("medBatchUp").value;
  const medBatchQt = document.getElementById("medBatchQt").value;
  const medBatchCurr = document.getElementById("medBatchCurr").value;
  const medBatchDisc = document.getElementById("medBatchDisc").value;
  
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
        const res = await apiRequest("POST", apiUrl, payload);
        if (!res.ok) {
            alert("Something went wrong");
            return;
        }
        medId = await res.text();
        if (medId)
        {
            med = await apiRequest("GET", apiUrl + medId);
            console.log("medicine:", med);
        }
  }
});