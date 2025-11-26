import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getPublicProfile, type PublicUserResponse } from "../api/users";

export default function PublicProfilePage() {
    const { username } = useParams<{ username: string}>();
    const [profile, setProfile] = useState<PublicUserResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadProfile() {
            if (!username) 
                return;

            setError(null);
            setLoading(true);

            try {
                const profile = await getPublicProfile(username);
                setProfile(profile);
            } catch (err: any) {
                setError(err.message ?? "Failed to load profile.");
            } finally {
                setLoading(false);
            }            
        }

        loadProfile();
    }, [ username ]);

    if (!username) {
        return <div style={{ padding: "2rem" }}>No username provided.</div>;
    }

    if (loading) {
        return <div style={{ padding: "2rem"}}>Loading profile...</div>
    }

    if (error) {
        return (
            <div style={{ padding: "2rem", color: "red" }}>
                <strong>Error:</strong> {error}
            </div>
        );
    }

    if (!profile) {
        return (
            <div style={{ padding: "2rem"}}>
                No profile matches the {username}.
            </div>
        );
    }

    return (
        <div style={{ padding: "2rem" }}>
          <h2>Profile: {profile.username}</h2>
          <p><strong>Bio:</strong> {profile.bio ?? "(no bio)"}</p>
          <p><strong>Avatar:</strong> {profile.avatarUrl ?? "(no avatar)"}</p>
        </div>
    );
}