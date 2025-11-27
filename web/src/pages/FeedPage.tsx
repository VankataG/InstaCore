import { useEffect, useState } from "react";
import { getFeed, type PostResponse } from "../api/posts";



export default function FeedPage() {

    const [posts, setPosts] = useState< PostResponse[] | null>(null);
    const [error, setError] = useState< string | null>(null);
    const [loading, setLoading] = useState(true);
    const [page, setPage] = useState<number>(1);

    useEffect(() => {
        async function loadFeed() {
            setError(null);
            setLoading(true);

            const token = localStorage.getItem("token");
            if (!token) {
                setError("No token found. Please login first.");
                setLoading(false);
                return;
            }

            try {
                const feed = await getFeed(page, 10);
                setPosts(feed);
            } catch (err: any) {
                setError(err.message || "Failed to load feed.")
            } finally {
                setLoading(false);
            }
        }

        loadFeed();
    }, [page]);

    if (loading) {
        return <div style={{ padding: "2rem"}}>Loading your profile...</div>;
    }

    if (error) {
        return (
            <div style= {{ padding: "2rem"}}>
                <p style={{ color: "red"}}>{error}</p>
            </div>
        );
    }

    if (posts?.length === 0) {
        return <div style={{ padding: "2rem"}}>No posts in your feed yet.</div>;
    }

    return (
        <>
            {posts?.map(post => (
                <div key={post.id} style={{ padding: "2rem"}}>
                    <p><strong>Username:</strong> {post.username}</p>
                    <p>{post.caption}</p>
                    <p>{post.imageUrl}</p>
                    <p>{post.totalLikes}</p>
                    <p>{post.totalComments}</p>
                    <p>{post.createdAt}</p>
                </div>
            ))}
            <div style={{ display: "flex", gap: "10px" }}>
                <button onClick={() => setPage( page - 1)} disabled={page <= 1}>Previous</button>
                <button onClick={() => setPage( page + 1)}>Next</button>
            </div>
        </>
    );
}