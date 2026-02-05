function setJwtToken(token) {
    localStorage.setItem("jwt_token", token);
}

function getJwtToken() {
    return localStorage.getItem("jwt_token");
}

function decodeJwt(token) {
    try {
        const payload = token.split(".")[1];
        const decoded = atob(payload.replace(/-/g, "+").replace(/_/g, "/"));
        return JSON.parse(decoded);
    } 
    catch {
        return null;
    }
}

function isTokenValid(token) {
    if (!token) return false;

    const decoded = decodeJwt(token);
    if (!decoded || !decoded.exp) return false;

    const now = Math.floor(Date.now() / 1000);

    return decoded.exp > now;
}

function isAuthenticated() {
    const token = getJwtToken();
    if (!isTokenValid(token)) window.location.href = "/Auth/Index";
    return true;
}

async function apiRequest(method, url, payload, options = {}) {
    if (!url) throw new Error("URL is required");
    if (!method) method = "GET";
    
    const fetchOptions = {
        method,
        headers: {}
    };
    const token = getJwtToken();

    if (token) {
        fetchOptions.headers.Authorization = `Bearer ${token}`;
    }

    if (payload && method !== "GET") {
        fetchOptions.body = JSON.stringify(payload);
        fetchOptions.headers["Content-Type"] = "application/json";
    }

    Object.assign(fetchOptions, options);
    Object.assign(fetchOptions.headers, options.headers);

    const res = await fetch(url, fetchOptions);
    if (!res.ok) {
        if (res.status === 401) {
            //console.warn("Unauthorized – redirect to login");
            isAuthenticated();
        }
        else {
            const error = await res.json();
            if (error) {

            }
        }
    }
    return res;
}

