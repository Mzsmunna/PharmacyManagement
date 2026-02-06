var meds = [];
var selectedMedId = "";
var selectedMed = null;
var selectedBatchId = "";
var selectedBatch = null;
const invoiceForm = document.getElementById("invoiceForm");
invoiceForm.style.display = "none";
const invoiceExpIV = document.getElementById("invoiceExpIV");
invoiceExpIV.style.display = "none";
var invoiceItems = [];

var invoicePayload = {
  items: [],
  //discount: 0,
  //currency: "",
  customerName: "",
  customerPhone: ""
};

async function getMedicines() {
    res = await apiRequest("GET", apiUrl + "Includes/''");
    meds = await res.json();
    //meds = Array.isArray(med) && med.length ? meds : [];
    console.log("medicine:", meds);
    if (meds && meds.length) {
        LoadInventoryTable();
    }
}
getMedicines();

function LoadInvMedBatchesDD() {
    invoiceForm.style.display = "none";
    const select = document.getElementById("invoiceBatches");

    // clear existing options
    select.innerHTML = "";

    selectedMed.Batches.forEach((item, index) => {
      const option = document.createElement("option");
      option.value = item.Id;
      option.textContent = item.No;

      // select first item
      if (index === 0) 
      {
        option.selected = true;
        selectedBatchId = item.Id;
        selectedBatch = selectedMed.Batches.find(b => b.Id === selectedBatchId);
        document.getElementById("invoiceQt").max = selectedBatch.Quantity;
      }
      
      invoiceForm.style.display = "block";
      select.appendChild(option);
    });

    select.addEventListener("change", (e) => {
        selectedBatchId = e.target.value;
        selectedBatch = selectedMed.Batches.find(b => b.Id === selectedBatchId);
        document.getElementById("invoiceQt").max = selectedBatch.Quantity;
        console.log(selectedBatch);
        if (selectedBatch && selectedBatch.length) invoiceForm.style.display = "block";
        else return;
    });
}

function LoadInvoiceMedsDD() {

    const select = document.getElementById("invoiceMeds");

    // clear existing options
    select.innerHTML = "";

    meds.forEach((item, index) => {
      const option = document.createElement("option");
      option.value = item.Id;
      option.textContent = item.Name;     

      // select first item
      if (index === 0)
      {
        option.selected = true;
      }
      
      select.appendChild(option);
    });

    select.addEventListener("change", (e) => {
      selectedMedId = e.target.value;
      selectedMed = meds.find(b => b.Id === selectedMedId);
      console.log(selectedMed);
      LoadInvMedBatchesDD();
    });
}

function LoadInventoryTable() {
    const tbody = document.getElementById("meds-inv-tbody");
    tbody.innerHTML = "";

    if (meds && meds.length)
    {
        let count = 0;
        meds.forEach((med, index) => {
            if (index == 0)
            {
                selectedMedId = med.Id;
                selectedMed = med;
                LoadInvMedBatchesDD();
            }
            
            const totalQt = med.Batches.reduce((sum, item) => sum + item.Quantity, 0);
            count++;
            //if (!med) return;
            const row = `
                <tr>
                    <td><span class="price-cell">${count}</span></td>
                    <td>
                        <div class="coin-cell">
                            <div class="coin-icon btc">M</div>
                            <div>
                                <div class="coin-name">${med.Name}</div>
                            </div>
                        </div>
                    </td>
                    <td class="change-cell positive">${med.Type}</td>
                    <td class="price-cell">${med.Description}</td>
                    <td class="change-cell positive">${totalQt}</td>
                    <td>
                        <div class="btn-group">
                            <button class="btn primary meds-inv" data-id="${med.Id}">Edit / View</button>
                            <button class="btn danger meds-inv-del" data-id="${med.Id}">Delete</button>
                        </div>
                    </td>
                </tr>
            `;
            tbody.insertAdjacentHTML("beforeend", row);
        });
        
        document.querySelectorAll(".meds-inv").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                window.location.href = '/Dashboard/Index?medId=' + id;
                //https://localhost:7069/Dashboard/Index?medId=019c31bb-5a3b-7589-b3b1-b0f72ffaae22
            });
        });

        document.querySelectorAll(".meds-inv-del").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                if (!id) return;
                const res = await apiRequest("DELETE", apiUrl + id, {});
                if (!res.ok) {
                    alert("Something went wrong");
                    return;
                }
                const status = await res.text();
                if (status) await getMedicines();
            });
        });
    }
    LoadInvoiceMedsDD();
}

function LoadInvoiceList() {
    const tbody = document.getElementById("invoices-tbody");
    tbody.innerHTML = "";
    if (invoiceItems && invoiceItems.length)
    {
        let totalQt = 0;
        invoiceItems.forEach(medBatch => {
            //if (!medBatch) return;
            if (medBatch.Quantity) totalQt += medBatch.Quantity;
            const row = `
                <tr>
                    <td><span class="price-cell">${medBatch.BatchNo}</span></td>
                    <td>
                        <div class="coin-cell">
                            <div class="coin-icon btc">M</div>
                            <div>
                                <div class="coin-name">${medBatch.MedicineName} | ${medBatch.MedicineType}</div>
                                <div class="coin-symbol">${medBatch.MedicineDesc}</div>
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
                            <button class="btn danger med-batches-del" data-id="${medBatch.Id}">Delete</button>
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

        document.querySelectorAll(".med-batches-del").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                if (!id) return;
                const res = await apiRequest("DELETE", batchApiUrl + id, {});
                if (!res.ok) {
                    alert("Something went wrong");
                    return;
                }
                const status = await res.text();
                if (status) await getMedicine(medId);
            });
        });
    }
}

invoiceForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const invoiceUp = document.getElementById("invoiceUp").value;
    const invoiceQt = document.getElementById("invoiceQt").value;
    const invoiceCurr = document.getElementById("invoiceCurr").value;
    const invoiceDisc = document.getElementById("invoiceDisc").value;
    let invoiceExp = document.getElementById("invoiceExp").value;
    if (invoiceExp) invoiceExp = new Date(invoiceExp);
    if (isNaN(invoiceExp.getTime()) 
    || invoiceExp < new Date())
    {
        invoiceExpIV.style.display = "inline";
        return;
    }
    else invoiceExpIV.style.display = "none";
    const httpMethod = "POST";
    var invMed = {
      MedicineId: selectedMedId,
      MedicineName: selectedMed.Name,
      MedicineType: selectedMed.Type,
      MedicineDesc: selectedMed.Description,
      BactchId: selectedBatch.Id,
      BatchNo: selectedBatch.No,
      Quantity: Number(invoiceQt),
      UnitPrice: Number(invoiceUp),
      Currency: invoiceCurr,
      Discount: Number(invoiceDisc),
      ExpiryDate: invoiceExp,
    };

    if (invMed.Quantity <= 0) return;

    selectedBatch.Quantity -= invMed.Quantity;
    document.getElementById("invoiceQt").max = selectedBatch.Quantity;
    document.getElementById("invoiceQt").value = selectedBatch.Quantity;
  
    console.log("InvoicePayload:", invMed);
    if (invMed) {
        let match = invoiceItems.find(b => b.BactchId = invMed.BactchId);
        if (match)
        {
            invMed.Quantity += match.Quantity;
            invoiceItems = invoiceItems.filter(b => b.BactchId != invMed.BactchId);
        }
        invoiceItems.push(invMed);
    }
    console.log("InvoiceItems:", invoiceItems);
    LoadInvoiceList();
});

