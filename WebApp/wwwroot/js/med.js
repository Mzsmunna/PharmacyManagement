console.log("Medicine script loaded.");
const medsForm = document.getElementById("medsForm");
const apiUrl = "https://localhost:7000/api/Medicines/Includes/";
const batchApiUrl = "https://localhost:7000/api/MedicineBatches/";


async function getMedicine(id) {
    document.getElementById("medCardTitle").textContent = "New Medicine";
    document.getElementById("medAddEdit").textContent = "+ Add Medicine";
    if (id)
    {
        res = await apiRequest("GET", apiUrl + id);
        med = await res.json();
        med = Array.isArray(med) && med.length ? med[0] : med;
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

let med = null;
let medId = new URLSearchParams(location.search).get("medId");
if (medId && isValidGuidV7(medId)) {
  console.log("Valid Medicine GUID v7:", medId);
  getMedicine(medId);
}
else {
    medId = "";
    med = null;
} 

const medsBatchForm = document.getElementById("medsBatchForm");
medsBatchForm.style.display = "none";
const medBatchExpIV = document.getElementById("medBatchExpIV");
medBatchExpIV.style.display = "none";

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
    await getMedicine(medId);
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
    if (isNaN(medBatchExp.getTime()) 
    || medBatchExp < new Date())
    {
        medBatchExpIV.style.display = "inline";
        return;
    }
    else medBatchExpIV.style.display = "none";
  
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
            Discount: medBatchDisc,
            Currency: medBatchCurr,
            ExpiryDate: medBatchExp,
        };
        const res = await apiRequest("POST", batchApiUrl, payload);
        if (!res.ok) {
            alert("Something went wrong");
            return;
        }
        medBatchId = await res.text();
        if (medBatchId)
        {
            //med = await apiRequest("GET", apiUrl + medId);
            //console.log("medicine:", med);
        }
    }
});