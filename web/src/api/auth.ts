 const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

 export type LoginResponse = {
    token: string;
    userId: string;
    username: string;
 };

 export async function login(email: string, password: string): Promise<LoginResponse> {
    const response = await fetch(`${API_BASE_URL}/api/auth/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify({ email, password}),
    });

    if (!response.ok) {
        const body = await response.json().catch(() => null);
        const message = body?.detail || body?.title || `Login failed ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }