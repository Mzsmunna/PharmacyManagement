//const apiUrl = "https://localhost:7000/api/Medicines/";
var meds = [];
var invoicePayload = {
  items: [],
  //discount: 0,
  //currency: "",
  customerName: "",
  customerPhone: ""
};

var itemPayload = {
  medicineId: "",
  batchNo: "",
  quantity: 0,
  unitPrice: 0,
  discount: 0
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

function LoadInventoryTable() {
    const tbody = document.getElementById("meds-inv-tbody");
    tbody.innerHTML = "";

    if (meds && meds.length)
    {
        let count = 0;
        meds.forEach(med => {
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
}