import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getMe, type MeResponse } from "../api/users";


export default function MePage() {
    const navigate = useNavigate();
    const [profile, setProfile] = useState<MeResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadMe() {
            setError(null);
            setLoading(true);

            const token = localStorage.getItem("token");
            if (!token) {
                setError("No token found. Please login first.");
                setLoading(false);
                return;
            }

            try {
                const me = await getMe(token);
                setProfile(me);
            } catch (err: any) {
                setError(err.message ?? "Failed to load /me.");
            } finally {
                setLoading(false);
            }            
        }

        loadMe();
    }, []);

    function handleLogout() {
        localStorage.removeItem("token");
        navigate("/login");
    }

    if (loading) {
        return <div style={{ padding: "2rem"}}>Loading your profile...</div>;
    }

    if (error) {
        return (
            <div style= {{ padding: "2rem"}}>
                <p style={{ color: "red"}}>{error}</p>
                <button onClick={() => navigate("/login")}>Go to login</button>
            </div>
        );
    }

    if (!profile) {
        return (
          <div style={{ padding: "2rem" }}>
            <p>No profile loaded.</p>
            <button onClick={() => navigate("/login")}>Go to login</button>
          </div>
        );
    }

    return (
        <div style={{ padding: "2rem" }}>
          <h2>My Profile</h2>
          <p><strong>Username:</strong> {profile.username}</p>
          <p><strong>Bio:</strong> {profile.bio ?? "(no bio yet)"}</p>
          <p><strong>Avatar:</strong> {profile.avatarUrl ?? "(no avatar yet)"}</p>

          <button onClick={handleLogout} style={{ marginTop: "1rem" }}>
            Logout
          </button>
        </div>
    );
}