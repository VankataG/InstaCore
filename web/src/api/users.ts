import { apiFetch } from "./client";


 export type PublicUserResponse = {
    username: string;
    bio: string | null;
    avatarUrl: string | null;
    followers: number;
    following: number;
    isFollowedByCurrentUser: boolean;
 };
 
 export async function getPublicProfile(username:string, token?: string | null): Promise<PublicUserResponse> {
    return apiFetch<PublicUserResponse>(`/api/users/${username}`, {
      method: "GET",
      token: token ?? undefined,
    })
 }


 export type MeResponse = {
    id: string;
    username: string;
    bio: string | null;
    avatarUrl: string | null;
    followers: number;
    following: number;
 };

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


 export async function followUser(token: string, username: string) {
   return apiFetch(`/api/users/${username}/follow`, {
      method: "POST",
      token,
   });
 }

 export async function unfollowUser(token: string, username: string) {
   return apiFetch(`/api/users/${username}/follow`, {
      method: "DELETE",
      token,
   });
 }