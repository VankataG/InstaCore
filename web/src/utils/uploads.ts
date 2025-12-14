const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;



export async function uploadPostImage(file: File, token: string): Promise<{ url: string}>{
    const formData = new FormData();

    formData.append("file", file);

    const response = await fetch(`${API_BASE_URL}/api/uploads/post-image`, {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`
            },
        body: formData
        
    });

    return response.json();
}

export async function uploadAvatar(file: File, token: string): Promise<{ url: string}> {
    const formData = new FormData();

    formData.append("file", file);

    const response = await fetch(`${API_BASE_URL}/api/uploads/avatar`, {
        method: "POST",
        headers: {
            'Authorization': `Bearer ${token}`
        },
        body: formData
    });

    return response.json();
}