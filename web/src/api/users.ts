 const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;


 export type PublicUserResponse = {
    username: string;
    bio: string | null;
    avatarUrl: string | null;
 };

 export type MeResponse = {
    id: string;
    username: string;
    bio: string | null;
    avatarUrl: string | null;
 };

 export async function getPublicProfile(username:string): Promise<PublicUserResponse> {
    const response = await fetch(`${API_BASE_URL}/api/users/${username}`);

    if (!response.ok) {
        const body = await response.json().catch(() => null);
        const message = body?.detail || body.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }

 export async function getMe(token:string): Promise<MeResponse> {
    const response = await fetch(`${API_BASE_URL}/api/users/me`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
    });
    
    if (!response.ok) {
        const body = await response.json().catch(() => null);
        const message = body?.detail || body?.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }