import { createContext, useEffect, useState } from "react";
import { getMe, type MeResponse } from "../api/users";


export type UserContextValue = {
    user: MeResponse | null;
    loadingUser: boolean;
    refreshUser: () => Promise<void>;
    logout: () => void;
};

export const UserContext = createContext<UserContextValue | undefined>(undefined);


export function UserProvider({ children }: { children: React.ReactNode}){
    const [user, setUser] = useState<MeResponse | null>(null);
    const [loadingUser, setLoadingUser] = useState<boolean>(true);

    useEffect(() => {
        async function loadUser(){

            refreshUser();
        }

        loadUser();
    }, []);

    async function refreshUser(){
        const token = localStorage.getItem("token");
        if (!token){
            setUser(null);
            setLoadingUser(false);
            return;
        }

        setLoadingUser(true);

        try {
            const currentUser = await getMe(token);
            setUser(currentUser);
        } catch {
            setUser(null);
        } finally {
            setLoadingUser(false);
        }
    }

    function logout(){
        localStorage.removeItem("token");
        setUser(null);
    }

    const context: UserContextValue = {
        user,
        loadingUser,
        refreshUser,
        logout, 
    };

    return <UserContext.Provider value={context}>{children}</UserContext.Provider>
}