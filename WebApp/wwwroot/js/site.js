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
}
