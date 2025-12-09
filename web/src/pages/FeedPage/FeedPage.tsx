import { useEffect, useState } from "react";

import { getFeed, type PostResponse } from "../../api/posts";
import { PostCard } from "../../components/PostCard/PostCard";
import CreatePostForm from "../../components/CreatePostForm/CreatePostForm";

import styles from "./FeedPage.module.css";
import { useNavigate } from "react-router-dom";

export default function FeedPage() {
    const navigate = useNavigate();

    const [posts, setPosts] = useState< PostResponse[]>([]);
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

    if(error){
        return (
            <div style= {{ padding: "2rem"}}>
                <p style={{ color: "red"}}>{error}</p>
                <button onClick={() => navigate("/login")}>Go to login</button>
            </div>
        );
    }

    return (
      <div className={styles.pageContainer}>
        <header className={styles.header}>
          <h1 className={styles.title}>Feed</h1>
          <p className={styles.subtitle}>
            See posts from people you follow.
          </p>
        </header>

        <main className={styles.feedContent}>
          <CreatePostForm
            onPostCreated={(post) => {
              setPosts((prev) => [post, ...prev]);
            }}
          />

          {loading && !error && (
            <div className={styles.skeleton}>Loading your feed...</div>
          )}

          {error && (
            <div className={styles.errorBox}>{error}</div>
          )}

          {!loading && !error && posts.length === 0 && (
            <p className={styles.emptyText}>
              No posts in your feed yet.
            </p>
          )}

          {!loading && !error && posts.length > 0 && (
            <>
              <div className={styles.postsList}>
                {posts.map((post) => (
                  <PostCard 
                    key={post.id} 
                    post={post} 
                    onPostDeleted={ id => {
                      setPosts(prev => prev.filter(p => p.id !== id))
                    }}
                  />
                ))}
              </div>

              <div className={styles.pagination}>
                <button
                  className={styles.paginationButton}
                  onClick={() => setPage((p) => Math.max(1, p - 1))}
                  disabled={page <= 1}
                >
                  Previous
                </button>

                <span className={styles.pageInfo}>Page {page}</span>

                <button
                  className={styles.paginationButton}
                  onClick={() => setPage((p) => p + 1)}
                >
                  Next
                </button>
              </div>
            </>
          )}
        </main>
      </div>
    );
}