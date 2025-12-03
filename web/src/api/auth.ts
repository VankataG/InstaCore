import { apiFetch } from "./client";

 export type AuthResponse = {
    token: string;
    userId: string;
    username: string;
 };

 export async function login(email: string, password: string): Promise<AuthResponse> {
    return apiFetch(`/api/auth/login`, {
        method: "POST",
        body: { email, password},
    });
 }

 export type RegisterRequest = {
    username: string;
    email: string;
    password: string;
 };

 export async function register(request: RegisterRequest): Promise<AuthResponse>{
    return apiFetch(`/api/auth/register`, {
        method: "POST",
        body: request,
    });
 }

