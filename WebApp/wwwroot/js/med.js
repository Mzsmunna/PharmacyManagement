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
            const card = document.getElementById("medInfo");
            card.querySelector("h2").textContent = med.Name;
            card.querySelector("p").textContent = med.Description;
            card.querySelector("span").textContent = med.Type;

            document.getElementById("medName").value = med.Name;
            document.getElementById("medType").value = med.Type;
            document.getElementById("medDesc").value = med.Description;

            document.getElementById("medCardTitle").textContent = "Edit Medicine";
            document.getElementById("medAddEdit").textContent = "Save Changes";
            medsBatchForm.style.display = "block";

            LoadBatchTable();
        }
    }
}

async function LoadMedicine(id) {
  if (id && isValidGuidV7(id)) {
      console.log("Valid Medicine GUID v7:", id);
      await getMedicine(id);
    }
    else {
        medId = "";
        med = null;
    }
}

function LoadBatchTable() {
    const tbody = document.getElementById("med-batches-tbody");
    tbody.innerHTML = "";
    if (med && med.Batches && med.Batches.length)
    {
        let totalQt = 0;
        med.Batches.forEach(medBatch => {
            //if (!medBatch) return;
            if (medBatch.Quantity) totalQt += medBatch.Quantity;
            const row = `
                <tr>
                    <td><span class="price-cell">${medBatch.No}</span></td>
                    <td>
                        <div class="coin-cell">
                            <div class="coin-icon btc">M</div>
                            <div>
                                <div class="coin-name">${med.Name} | ${med.Type}</div>
                                <div class="coin-symbol">${med.Description}</div>
                            </div>
                        </div>
                    </td>
                    <td class="change-cell positive">${medBatch.Quantity}</td>
                    <td class="price-cell">${medBatch.UnitPrice}${medBatch.Currency}</td>
                    <td class="volume-cell">${medBatch.Discount}%</td>
                    <td class="change-cell negative">${formatDateGmt(medBatch.ExpiryDate)}</td>
                    <td>
                        <div class="btn-group">
                            <button class="btn primary med-batches" data-id="${medBatch.Id}">Edit / View</button>
                            <button class="btn danger">Delete</button>
                        </div>
                    </td>
                </tr>
              `;
            tbody.insertAdjacentHTML("beforeend", row);        
        });

        document.querySelectorAll(".med-batches").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                await LoadMedicineBatch(id);
            });
        });
    }
}

async function LoadMedicineBatch(id) {
    document.getElementById("medBatchCardTitle").textContent = "New Batch";
    document.getElementById("medBatchAddEdit").textContent = "+ Add Medicine";
    if (id && isValidGuidV7(id)) {
      console.log("Valid Medicine Batch GUID v7:", id);
      if (med && med.Batches && med.Batches.length)
      {
         medBatch = med.Batches.find(b => b.Id === id);
         if (medBatch) {
            document.getElementById("medBatchCardTitle").textContent = "Edit Batch";
            document.getElementById("medBatchAddEdit").textContent = "Save Changes";
            document.getElementById("medBatchNo").value = medBatch.No;
            document.getElementById("medBatchUp").value = medBatch.UnitPrice;
            document.getElementById("medBatchQt").value = medBatch.Quantity;
            document.getElementById("medBatchCurr").value = medBatch.Currency;
            document.getElementById("medBatchDisc").value = medBatch.Discount;
            document.getElementById("medBatchExp").value = formatDateGmt(medBatch.ExpiryDate);
         }
      }
    }
}

let medBatch = null;
let med = null;
let medId = new URLSearchParams(location.search).get("medId");
if (medId) LoadMedicine(medId);

//document.getElementById("medEV").addEventListener("click", async (e) => {
//    const id = e.currentTarget.dataset.id;
//    await LoadMedicine(id);
//});

//document.getElementById("medBatchEV").addEventListener("click", async (e) => {
//    const id = e.currentTarget.dataset.id;
//    await LoadMedicineBatch(id);
//});

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