var meds = [];
var selectedMedId = "";
var selectedMed = null;
var selectedBatchId = "";
var selectedBatch = null;
const invoiceForm = document.getElementById("invoiceForm");
invoiceForm.style.display = "none";
const invoiceExpIV = document.getElementById("invoiceExpIV");
invoiceExpIV.style.display = "none";
const invHistItems = document.getElementById("invHistItems");
invHistItems.style.display = "none";
var invoiceItems = [];
const invApiUrl = "https://localhost:7000/api/Invoices/";
var invoices = [];
var selectedInvoice = null;

async function getMedicines() {
    res = await apiRequest("GET", apiUrl + "Includes/''");
    meds = await res.json();
    //meds = Array.isArray(med) && med.length ? meds : [];
    console.log("medicine:", meds);
    if (meds && meds.length) {
        //meds = meds.filter(m =>
        //  m.Batches.some(b => b.Quantity > 0)
        //);
        LoadInventoryTable();
    }
}
getMedicines();

async function getInvoices() {
    res = await apiRequest("GET", invApiUrl + "Includes/''");
    invoices = await res.json();
    console.log("invoices:", invoices);
    if (invoices && invoices.length) {
        LoadInvoiceTable();
    }
}
getInvoices();

function LoadInvoiceItems()
{
    if (selectedInvoice && selectedInvoice.InvoiceItems)
    {
        const tbody = document.getElementById("inv-hist-item-tbody");
        tbody.innerHTML = "";
        selectedInvoice.InvoiceItems.forEach((item, index) => {
            if (!item) return;
            var medDetails = meds
                      .flatMap(x => x.Batches)
                      .find(x => x.Id === item.MedicineBatchId);
            var medInfo = meds.find(x => x.Id === medDetails.MedicineId);
            const row = `
                <tr>
                    <td><span class="price-cell">${item.MedicineBatchNo}</span></td>
                    <td>
                        <div class="coin-cell">
                            <div class="coin-icon btc">M</div>
                            <div>
                                <div class="coin-name">${medInfo.Name} | ${medInfo.Type}</div>
                                <div class="coin-symbol">${medInfo.Description}</div>
                            </div>
                        </div>
                    </td>
                    <td class="change-cell positive">${item.Quantity}</td>
                    <td class="price-cell">${item.UnitPrice}${selectedInvoice.Currency}</td>
                    <td class="volume-cell">${item.Discount}%</td>
                    <td class="price-cell">${item.Total}${selectedInvoice.Currency}</td>
                    <td class="change-cell negative">${formatDateGmt(item.ExpiryDate)}</td>
                </tr>
            `;

            tbody.insertAdjacentHTML("beforeend", row);
        });
    }
}

function LoadInvoiceTable()
{
    const tbody = document.getElementById("inv-hist-tbody");
    tbody.innerHTML = "";

    if (invoices && invoices.length)
    {
        invoices.forEach((inv, index) => {
            if (!inv) return;
            const row = `
                <tr>
                    <td><span class="price-cell">${inv.InvoiceNo}</span></td>
                    <td>
                        <div class="coin-cell">
                            <div class="coin-icon btc">C</div>
                            <div>
                                <div class="coin-name">${inv.CustomerName}</div>
                                <div class="coin-symbol">${inv.CustomerPhone}</div>
                            </div>
                        </div>
                    </td>
                    <td class="change-cell positive">${inv.Items}</td>
                    <td class="price-cell">${inv.Total}</td>
                    <td>
                        <div class="btn-group">
                            <button class="btn primary inv-hist" data-id="${inv.Id}">View</button>
                        </div>
                    </td>
                </tr>
            `;

            tbody.insertAdjacentHTML("beforeend", row);
        });
        
        document.querySelectorAll(".inv-hist").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                invHistItems.style.display = "block";
                selectedInvoice = invoices.find(b => b.Id === id);
                if (selectedInvoice) {
                    document.getElementById("inv-no-items").textContent = selectedInvoice.InvoiceNo;
                    LoadInvoiceItems();
                }
            });
        });
    }
}

function LoadInvMedBilling()
{
    //if (!selectedBatch)
    document.getElementById("invoiceQt").max = selectedBatch.Quantity;
    console.log(selectedBatch);
    if (selectedBatch) invoiceForm.style.display = "block";
    else return;
    //document.getElementById("invoiceNo").value = selectedBatch.No;
    document.getElementById("invoiceUp").value = selectedBatch.UnitPrice;
    document.getElementById("invoiceQt").value = 1; //selectedBatch.Quantity;
    document.getElementById("invoiceCurr").value = selectedBatch.Currency;
    document.getElementById("invoiceDisc").value = selectedBatch.Discount;
    document.getElementById("invoiceExp").value = formatDateGmt(selectedBatch.ExpiryDate);
}

function LoadInvMedBatchesDD() {
    invoiceForm.style.display = "none";
    const select = document.getElementById("invoiceBatches");

    // clear existing options
    select.innerHTML = "";

    if (!selectedMed || !selectedMed.Batches) return;

    selectedMed.Batches.forEach((item, index) => {
      const option = document.createElement("option");
      option.value = item.Id;
      option.textContent = item.No;

      // select first item
      if (index === 0) 
      {
        option.selected = true;
        selectedBatchId = item.Id;
        selectedBatch = item;
        LoadInvMedBilling();
      }
      
      //invoiceForm.style.display = "block";
      select.appendChild(option);
    });

    select.addEventListener("change", (e) => {
        selectedBatchId = e.target.value;
        selectedBatch = selectedMed.Batches.find(b => b.Id === selectedBatchId);
        LoadInvMedBilling();
    });
}

function LoadInvoiceMedsDD() {

    const select = document.getElementById("invoiceMeds");

    // clear existing options
    select.innerHTML = "";

    availableMed = meds.filter(m =>
        m.Batches.some(b => b.Quantity > 0)
    );
    availableMed.forEach((item, index) => {
      const option = document.createElement("option");
      option.value = item.Id;
      option.textContent = item.Name;     

      // select first item
      if (index === 0)
      {
        option.selected = true;
        selectedMedId = item.Id;
        selectedMed = item;
        LoadInvMedBatchesDD();
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
            if (!meds || !med.Batches) return;
            
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
        let totalUp = 0;
        let totalDisc = 0;
        invoiceItems.forEach(medBatch => {
            if (!medBatch) return;

            if (!medBatch.BatchId && medBatch.BatchNo) {
                const batch = meds
                      .flatMap(x => x.Batches)
                      .find(x => x.No === medBatch.BatchNo);
                if (batch) medBatch.BatchId = batch.Id;
            }
            else if (medBatch.BatchId && !medBatch.BatchNo) {
                const batch = meds
                      .flatMap(x => x.Batches)
                      .find(x => x.Id === medBatch.BatchId);
                if (batch) medBatch.BatchNo = batch.No;
            }

            if (medBatch.Quantity) totalQt += medBatch.Quantity;
            if (medBatch.UnitPrice) totalUp += (medBatch.UnitPrice * medBatch.Quantity);
            if (medBatch.Discount) totalDisc += medBatch.Discount;
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
                            <button class="btn primary inv-med-batches" data-id="${medBatch.BatchId}">Edit / View</button>
                            <button class="btn danger inv-med-batches-del" data-id="${medBatch.BatchId}">Delete</button>
                        </div>
                    </td>
                </tr>
              `;
            tbody.insertAdjacentHTML("beforeend", row);
        });

        const row = `
            <tr>
                <td><span class="price-cell"></span></td>
                <td class="change-cell">Total</td>
                <td class="change-cell">${totalQt}</td>
                <td class="price-cell">${totalUp}</td>
                <td class="volume-cell">${totalDisc}%</td>
                <td class="change-cell"></td>
                <td></td>
            </tr>
        `;
        tbody.insertAdjacentHTML("beforeend", row);   

        document.querySelectorAll(".inv-med-batches").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                let match = invoiceItems.find(b => b.BatchId = id);
                if (match) 
                {
                    selectedBatchId = match.BatchId;
                    selectedBatch = match;
                }
                invoiceItems = invoiceItems.filter(b => b.BatchId != id);
                LoadInvoiceList();
                LoadInvMedBilling();
            });
        });

        document.querySelectorAll(".inv-med-batches-del").forEach(btn => {
            btn.addEventListener("click", async (e) => {
                const id = e.currentTarget.dataset.id;
                if (!id) return;
                let match = invoiceItems.find(b => b.BatchId = id);
                if (match) 
                {
                    const batch = meds
                      .flatMap(x => x.Batches)
                      .find(x => x.Id === id);
                    batch.Quantity += match.Quantity;
                    invoiceItems = invoiceItems.filter(b => b.BatchId != id);
                    LoadInvoiceList();
                    if (selectedBatchId == batch.Id)
                    {
                        LoadInvMedBilling();
                    }
                }
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
      BatchId: selectedBatch.Id,
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
        let match = invoiceItems.find(b => b.BatchId = selectedBatchId.BatchId);
        if (match)
        {
            invMed.Quantity += match.Quantity;
            invoiceItems = invoiceItems.filter(b => b.BatchId != selectedBatchId.BatchId);
        }
        invoiceItems.push(invMed);
    }
    console.log("InvoiceItems:", invoiceItems);
    LoadInvoiceList();
});

document.getElementById("invoiceSave").addEventListener("click", async (e) => {
    e.preventDefault();

    const custName = document.getElementById("custName").value;
    const custNum = document.getElementById("custNum").value;

    if (!invoiceItems || !custName || !custNum) return;

    const disc =
    invoiceItems.reduce((sum, x) => sum + x.Discount, 0) / invoiceItems.length;
    var payload = {
          items: invoiceItems,
          Discount: disc,
          Currency: "$",
          CustomerName: custName,
          CustomerPhone: custNum
    };
    const res = await apiRequest("POST", invApiUrl, payload);
        if (!res.ok) {
            alert("Something went wrong");
            return;
        }
        var invoiceId = await res.text();
        if (invoiceId)
        {
            getMedicines();
            getInvoices();
            const tabs = document.querySelectorAll(".settings-tabs .settings-tab");
            tabs.forEach(tab => tab.classList.remove("active"));
            if (tabs[3]) {
              tabs[3].classList.add("active");
            }
            document.getElementById('preferences').classList.add('active');
            document.getElementById('notifications').classList.remove('active');
            invoiceItems = [];
            const tbody = document.getElementById("invoices-tbody");
            tbody.innerHTML = "";
        }
});

document.getElementById("invItemCancel").addEventListener("click", async (e) => {
    //const id = e.currentTarget.dataset.id;
    invHistItems.style.display = "none";
});

