import { apiFetch } from "./client";


export type CommentResponse = {
    commentId: string;
    postId: string;
    username: string;
    text: string;
    createdAt: string;
}

export async function getPostComments(postId: string): Promise<CommentResponse[]> {
    return apiFetch(`/api/comments/${postId}/comments`);
}


export type CreateCommentRequest = {
    text: string;
};

export async function addComment(token: string, postId: string, request: CreateCommentRequest): Promise<CommentResponse> {
    return apiFetch(`/api/comments/${postId}/comments`, {
        method: "POST",
        token,
        body: request
    });
} 

export async function deleteComment(token: string, postId: string, commentId: string){
    return apiFetch(`/api/comments/${postId}/comments/${commentId}`, {
        method: "DELETE",
        token,
    });
}