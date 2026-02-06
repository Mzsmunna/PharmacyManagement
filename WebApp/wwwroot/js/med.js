console.log("Medicine script loaded.");
const medsForm = document.getElementById("medsForm");
const apiUrl = "https://localhost:7000/api/Medicines/";

let medId = "";
let med = null;
async function getMedicine(id) {
    document.getElementById("medCardTitle").textContent = "New Medicine";
    document.getElementById("medAddEdit").textContent = "+ Add Medicine";
    if (id)
    {
        res = await apiRequest("GET", apiUrl + id);
        med = await res.json();
        console.log("medicine:", med);
        if (med) {
            document.getElementById("medCardTitle").textContent = "Edit Medicine";
            document.getElementById("medAddEdit").textContent = "Save Changes";
            medsBatchForm.style.display = "block";
            document.getElementById("medName").value = med.Name;
            document.getElementById("medType").value = med.Type;
            document.getElementById("medDesc").value = med.Description;
        }
    }
}

const medsBatchForm = document.getElementById("medsBatchForm");
medsBatchForm.style.display = "none";

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
    let res = await apiRequest("POST", apiUrl, payload);
    if (!res.ok) {
        alert("Something went wrong");
        return;
    }
    medId = await res.text();
    getMedicine(medId);
  }
});

medsBatchForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const medBatchNo = document.getElementById("medBatchNo").value;
    const medBatchUp = document.getElementById("medBatchUp").value;
    const medBatchQt = document.getElementById("medBatchQt").value;
    const medBatchCurr = document.getElementById("medBatchCurr").value;
    const medBatchDisc = document.getElementById("medBatchDisc").value;
    let medBatchExp = document.getElementById("medBatchExp").value;
    if (medBatchExp) medBatchExp = new Date(medBatchExp);
  
    console.log("medBatchNo:", medBatchNo);
    console.log("medBatchUp:", medBatchUp);
    console.log("medBatchQt:", medBatchQt);
    console.log("medBatchCurr:", medBatchCurr);
    console.log("medBatchDisc:", medBatchDisc);
    console.log("medBatchExp:", medBatchExp);
    console.log("medId:", medId);

    if (medId && medBatchNo && medBatchUp && medBatchQt && medBatchExp) {      
        var payload = {
            No: medBatchNo,
            MedicineId: medId,
            Quantity: medBatchQt,
            UnitPrice: medBatchUp,
            Discount: medDesc,
            Currency: medBatchCurr,
            ExpiryDate: medBatchExp,
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