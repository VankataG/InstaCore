import { apiFetch } from "./client";


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
    return apiFetch<PublicUserResponse>(`/api/users/${username}`)
 }

 export async function getMe(token:string): Promise<MeResponse> {
    return apiFetch<MeResponse>(`/api/users/me`, {
        method: "GET",
        token
    });
 }

 export type UpdateProfileRequest= {
    bio?: string | null;
    avatarUrl?: string | null;
 }

 export async function updateProfile(request:UpdateProfileRequest, token:string): Promise<MeResponse> {
    return apiFetch(`/api/users/me`, {
        method: "PUT",
        token,
        body: request,
    });
 }