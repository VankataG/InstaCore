import { apiFetch } from "./client";

 const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

 export type LoginResponse = {
    token: string;
    userId: string;
    username: string;
 };

 export async function login(email: string, password: string): Promise<LoginResponse> {
    return apiFetch(`/api/auth/login`, {
        method: "POST",
        body: { email, password},
    });
 }