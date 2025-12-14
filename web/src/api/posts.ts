import { apiFetch } from "./client";

export type PostResponse = {
    id: string;
    caption: string;
    imageUrl: string | null;
    username: string;
    userAvatarUrl: string | null;
    createdAt: string;
    likes: number;
    comments: number;
    isLikedByCurrentUser: boolean;
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
   const token = localStorage.getItem("token");

    return apiFetch(`/api/posts/user/${username}?page=${page}&pageSize=${pageSize}`, {
      method: "GET",
      token: token ?? undefined,
    });
 }


 export type CreatePostRequest = {
   caption: string | null;
   imageUrl?: string | null;
 };

 export async function createPost(token: string, request: CreatePostRequest): Promise<PostResponse> {
    return apiFetch(`/api/posts`, {
        method: "POST",
        token,
        body: request,
    });
 }

 export async function deletePost(token: string, postId: string) {
   return apiFetch(`/api/posts/${postId}`, {
      method: "DELETE",
      token
   });
 }