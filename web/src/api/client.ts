const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

type RequestOptions= {
    method?: "GET" | "PUT" | "POST" | "DELETE";
    token?: string | null;
    body?: unknown;
}

export async function apiFetch<TResponse>(path: string, options: RequestOptions = {}): Promise<TResponse>{
    const { method = "GET", token, body } = options

    const headers: HeadersInit = {
        "Content-Type": "application/json",
    };

    if (token){
        headers.Authorization = `Bearer ${token}`;
    }

    const response = await fetch(`${API_BASE_URL}${path}`, { 
        method, 
        headers,
        body: body !== undefined ? JSON.stringify(body) : undefined
    });

    if (!response.ok){
        const errorBody = await response.json().catch(() => null as any);
        const message = errorBody?.detail || errorBody?.title || `Request failed: ${response.status}`;

        throw new Error(message);
    }

    const text = await response.text();

    if (!text){
        return undefined as TResponse;
    }

    return JSON.parse(text) as TResponse;
}