import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { getPublicProfile, type PublicUserResponse } from "../api/users";
import { getUserPosts, type PostResponse } from "../api/posts";

export default function PublicProfilePage() {
    const { username } = useParams<{ username: string}>();
    const [profile, setProfile] = useState<PublicUserResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    const [userPosts, setUserPosts] = useState<PostResponse[] | null>(null);
    const [page, setPage] = useState<number>(1);
    const [postsLoading, setPostsLoading] = useState<boolean>(true);
    const [postsError, setPostsError] = useState<string | null>(null);

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

    useEffect(() => {
        async function loadPosts() {
            if (!username) return;

            setPostsError(null);
            setPostsLoading(true);

            try {
                const posts = await getUserPosts(username, page, 6);
                setUserPosts(posts);
            } catch (err: any) {
                setPostsError(err.message ?? "Failed to load user posts.");
            } finally {
                setPostsLoading(false);
            }
        }

        loadPosts();
    }, [username, page]);

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
                No profile matches the username: {username}.
            </div>
        );
    }

    return (
        <div style={{ padding: "2rem" }}>
          <h2>Profile: {profile.username}</h2>
          <p><strong>Bio:</strong> {profile.bio ?? "(no bio)"}</p>
          <p><strong>Avatar:</strong> {profile.avatarUrl ?? "(no avatar)"}</p>

          <section style={{ marginTop: "2rem"}}>
            <h2>Posts</h2>
            {postsLoading && (
                <p>Loading posts...</p>
            )}

            {!postsLoading && postsError && (
                <p style={{ color: "red"}}>{postsError}</p>
            )}

            {!postsLoading && !postsError && userPosts && userPosts.length === 0 && (
                <p>No posts yet.</p>
            )}

            {!postsLoading && !postsError && userPosts && userPosts.length > 0 && (
                <>
                    <div>
                      {userPosts.map(post => (
                        <div key={post.id} style={{ padding: "1rem 0", borderBottom: "1px solid #eee" }}>
                          <p><strong>{post.username}</strong></p>
                          <p>{post.caption}</p>
                          <p>Likes: {post.totalLikes} | Comments: {post.totalComments}</p>
                          <p style={{ fontSize: "0.8rem", color: "#666" }}>{post.createdAt}</p>
                        </div>
                      ))}
                    </div>
                  
                    <div style={{ marginTop: "1rem", display: "flex", gap: "0.5rem" }}>
                      <button onClick={() => setPage(page - 1)} disabled={page <= 1}>
                        Previous
                      </button>
                      <button onClick={() => setPage(page + 1)}>
                        Next
                      </button>
                    </div>
                </>
            )}
          </section>
        </div>
    );
}