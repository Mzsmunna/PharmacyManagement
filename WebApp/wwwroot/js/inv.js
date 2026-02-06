//const apiUrl = "https://localhost:7000/api/Medicines/";

async function getMedicines() {
    res = await apiRequest("GET", apiUrl + "Includes/''");
    meds = await res.json();
    //meds = Array.isArray(med) && med.length ? meds : [];
    console.log("medicine:", meds);
    if (meds) {


        //LoadInventoryTable();
    }
}
getMedicines();