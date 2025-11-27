
const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export type PostResponse = {
    id: string;
    caption: string;
    imageUrl: string | null;
    username: string;
    createdAt: string;
    totalLikes: number;
    totalComments: number;
 };

 export async function getFeed(page:number, pageSize:number): Promise<PostResponse[]> {
    const token = localStorage.getItem("token");
    
    const response = await fetch(`${API_BASE_URL}/api/feed?page=${page}&pageSize=${pageSize}`, {
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
    });

    if (!response.ok){
        const body = await response.json().catch(() => null);
        const message = body?.detail || body?.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }

 export async function getPostById(id: string): Promise<PostResponse> {
    const response = await fetch(`${API_BASE_URL}/api/posts/${id}`);

    if (!response.ok){
        const body = await response.json().catch(() => null);
        const message = body?.detail || body.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }

 export async function getUserPosts(username: string, page: number, pageSize: number): Promise<PostResponse[]> {
    const response = await fetch(`${API_BASE_URL}/api/posts/user/${username}?page=${page}&pageSize=${pageSize}`);

    if (!response.ok){
        const body = await response.json().catch(() => null);
        const message = body?.detail || body?.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }

 export async function createPost(caption: string, imageUrl: string | null): Promise<PostResponse> {
    const token = localStorage.getItem("token");

    const response = await fetch(`${API_BASE_URL}/api/posts`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ caption, imageUrl })
    });

    if (!response.ok){
        const body = await response.json().catch(() => null);
        const message = body?.detail || body?.title || `Request failed: ${response.status}`;
        throw new Error(message);
    }

    return response.json();
 }