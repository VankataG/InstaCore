import { apiFetch } from "./client";


export type CommentResponse = {
    id: string;
    postId: string;
    username: string;
    text: string;
    createdAt: string;
}

export async function getPostComments(postId: string): Promise<CommentResponse[]> {
    return apiFetch(`/api/comments/${postId}/comments`);
}
