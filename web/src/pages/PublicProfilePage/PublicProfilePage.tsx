import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";

import { getPublicProfile, type PublicUserResponse } from "../../api/users";
import { getUserPosts, type PostResponse } from "../../api/posts";
import { PostCard } from "../../components/PostCard/PostCard";

import styles from "./PublicProfilePage.module.css";

export default function PublicProfilePage() {
    const { username } = useParams<{ username: string}>();
    const [profile, setProfile] = useState<PublicUserResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    const [userPosts, setUserPosts] = useState<PostResponse[]>([]);
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


    if (!profile) {
        return (
            <div style={{ padding: "2rem"}}>
                No profile matches the username: {username}.
            </div>
        );
    }

    return (
    <div className={styles.pageContainer}>
      {/* PROFILE HEADER */}
      <section className={styles.profileHeader}>
        {loading && (
          <div className={styles.skeleton}>Loading profile...</div>
        )}

        {error && <div className={styles.errorBox}>{error}</div>}

        {profile && !loading && !error && (
          <div className={styles.profileHeaderContent}>
            <div className={styles.avatarWrapper}>
              {"avatarUrl" in profile && (profile as any).avatarUrl ? (
                <img
                  src={(profile as any).avatarUrl}
                  alt={profile.username}
                  className={styles.avatarImage}
                />
              ) : (
                <div className={styles.avatarInitials}>
                  {profile.username[0]?.toUpperCase()}
                </div>
              )}
            </div>

            <div className={styles.profileMain}>
              <div className={styles.profileRow}>
                <h1 className={styles.profileUsername}>{profile.username}</h1>
              </div>

              <div className={styles.profileStats}>
                <span>
                  <strong>
                    {"postsCount" in profile && (profile as any).postsCount != null
                      ? (profile as any).postsCount
                      : userPosts.length}
                  </strong>{" "}
                  posts
                </span>
                <span>
                  <strong>
                    {"followersCount" in profile
                      ? (profile as any).followersCount ?? 0
                      : 0}
                  </strong>{" "}
                  followers
                </span>
                <span>
                  <strong>
                    {"followingCount" in profile
                      ? (profile as any).followingCount ?? 0
                      : 0}
                  </strong>{" "}
                  following
                </span>
              </div>

              {"bio" in profile && (profile as any).bio && (
                <p className={styles.profileBio}>{(profile as any).bio}</p>
              )}
            </div>
          </div>
        )}
      </section>

      {/* POSTS SECTION */}
      <section className={styles.postsSection}>
        <h2 className={styles.sectionTitle}>Posts</h2>

        {postsLoading && (
          <div className={styles.skeleton}>Loading posts...</div>
        )}

        {postsError && <div className={styles.errorBox}>{postsError}</div>}

        {!postsLoading && !postsError && userPosts.length === 0 && (
          <p className={styles.emptyText}>This user has no posts yet.</p>
        )}

        {!postsLoading && !postsError && userPosts.length > 0 && (
          <div className={styles.postsGrid}>
            {userPosts.map((post) => (
              <PostCard key={(post as any).id} post={post} />
            ))}
          </div>
        )}

        <div className={styles.pagination}>
          <button
            disabled={page === 1}
            onClick={() => setPage((p) => Math.max(1, p - 1))}
          >
            Previous
          </button>
          <span>Page {page}</span>
          <button
            disabled={userPosts.length < page}
            onClick={() => setPage((p) => p + 1)}
          >
            Next
          </button>
        </div>
      </section>
    </div>
  );
    
}