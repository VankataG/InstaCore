import { apiFetch } from "./client";

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

    return apiFetch<PostResponse[]>(`/api/feed?page=${page}&pageSize=${pageSize}`, {
        method: "GET",
        token,
    });
 }

 export async function getPostById(id: string): Promise<PostResponse> {

    return apiFetch(`/api/posts/${id}`);
 }

 export async function getUserPosts(username: string, page: number, pageSize: number): Promise<PostResponse[]> {
    return apiFetch(`/api/posts/user/${username}?page=${page}&pageSize=${pageSize}`);
 }

//  export async function createPost(request: CreatePostRequest): Promise<PostResponse> {
//     const token = localStorage.getItem("token");

//     return apiFetch(`/api/posts`, {
//         method: "POST",
//         token,
//         body: request,
//     })
//  }