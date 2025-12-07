import { apiFetch } from "./client";


export type LikeResponse = {
   likeId: string | null;
   postId: string;
   username: string;
   liked: boolean;
   totalLikes: number;
 };

 export async function likePost(token: string, postId: string): Promise<LikeResponse> {
   return apiFetch(`/api/likes/${postId}/like`, {
      method: "POST",
      token,
   });
 }

 export async function unlikePost(token: string, postId: string) : Promise<LikeResponse> {
   return apiFetch(`/api/likes/${postId}/like`, {
      method: "DELETE",
      token,
   });
 }