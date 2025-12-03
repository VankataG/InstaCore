import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { getMe, updateProfile, type MeResponse } from "../../api/users";
import { PostCard } from "../../components/PostCard/PostCard";
import { getUserPosts, type PostResponse } from "../../api/posts";

import styles from "./MePage.module.css";

export default function MePage() {
    const navigate = useNavigate();

    const [profile, setProfile] = useState<MeResponse | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(true);

    const [userPosts, setUserPosts] = useState<PostResponse[]>([]);
    const [postsLoading, setPostsLoading] = useState<boolean>(true);
    const [postsError, setPostsError] = useState<string | null>(null);
    const [page, setPage] = useState<number>(1);

    const [editing, setEditing] = useState<boolean>(false);
    const [editBio, setEditBio] = useState<string | null>(null);
    const [editAvatar, setEditAvatar] = useState<string | null>(null);
    const [editError, setEditError] = useState<string | null>(null);

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

    useEffect(() => {
        async function loadPosts() {
            if(profile){
                setPostsError(null);
                setPostsLoading(true);

                try{
                    const response = await getUserPosts(profile.username, page, 6);
                    setUserPosts(response)
                } catch (err: any) {
                    setPostsError(err.message || "Failed to load user posts.");
                } finally {
                    setPostsLoading(false);
                }

            }
        }
        loadPosts();
    }, [profile, page]);

    async function editProfile() {
        if(!profile) return;

        const token = localStorage.getItem("token");
        if (!token) {
            setEditError("No token found. Please login first.");
            setEditing(false);
            return;
        }

        try{
            setEditError(null);
            setEditing(true);
            
            const request = { bio: editBio , avatarUrl: editAvatar};
            const response = await updateProfile(request, token);
            setProfile(response)
        } catch (err: any) {
            setEditError(err.message);
        } finally {
            setEditing(false);
        }
            
    }

    function startEdit(){
        if (!profile) return;

        setEditing(true);
        setEditError(null);
        setEditBio(profile.bio);
        setEditAvatar(profile.avatarUrl);
    }

    function cancelEdit(){
        setEditing(false);
        setEditError(null);
    }

    function handleLogout() {
        localStorage.removeItem("token");
        navigate("/login");
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
      <div className={styles.pageContainer}>
        <section className={styles.profileSection}>
          {loading && (
            <div className={styles.skeleton}>Loading your profile...</div>
          )}

          {error && <div className={styles.errorBox}>{error}</div>}

          {profile && !loading && !error && (
            <div className={styles.profileCard}>
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

                {editing && (
                    <div className={styles.editAvatarField}>
                        <label className={styles.label} htmlFor="avatarUrl">Avatar URL</label>
                        <input id="avatarUrl" 
                            type="text" 
                            className={styles.input}
                            value={editAvatar ?? ""}
                            onChange={(e) => setEditAvatar(e.target.value)}
                        />
                    </div>
                )}
              </div>

              <div className={styles.profileMain}>
                <div className={styles.topRow}>
                  <h1 className={styles.username}>{profile.username}</h1>

                  <div className={styles.actions}>
                    <button
                      type="button"
                      className={styles.buttonSecondary}
                      onClick={startEdit}
                    >
                      Edit profile
                    </button>

                    <button
                      type="button"
                      className={styles.buttonGhost}
                      onClick={handleLogout}
                    >
                      Log out
                    </button>
                  </div>
                </div>

                <div className={styles.stats}>
                  <span>
                    <strong>
                      {"postsCount" in profile && (profile as any).postsCount != null
                        ? (profile as any).postsCount
                        : userPosts?.length ?? 0}
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

                {!editing ? (
                    profile.bio ? (
                        <p className={styles.bio}>{profile.bio}</p>
                    ) : (
                        <p className={styles.bioPlaceholder}>You don't have a bio yet.</p>
                    )
                ) : (
                    <div className={styles.editBioField}>
                      <label className={styles.label} htmlFor="bio">
                        Bio
                      </label>
                      <textarea
                        id="bio"
                        className={styles.textarea}
                        rows={3}
                        value={editBio ?? ""}
                        onChange={(e) => setEditBio(e.target.value)}
                        placeholder="Write something about yourself..."
                      />

                      {editError && <div className={styles.errorBox}>{editError}</div>}

                      <div className={styles.editActions}>
                        <button
                          type="button"
                          className={styles.buttonSecondary}
                          onClick={editProfile}
                        >
                          Save
                        </button>
                        <button
                          type="button"
                          className={styles.buttonGhost}
                          onClick={cancelEdit}
                        >
                          Cancel
                        </button>
                      </div>
                    </div>
                )}
              </div>
            </div>
          )}
        </section>

        <section className={styles.postsSection}>
          <div className={styles.postsHeader}>
            <h2 className={styles.sectionTitle}>My posts</h2>

            {/* <button className={styles.buttonPrimary}>New post</button> */}
          </div>

          {postsLoading && (
            <div className={styles.skeleton}>Loading your posts...</div>
          )}

          {postsError && <div className={styles.errorBox}>{postsError}</div>}

          {!postsLoading && !postsError && userPosts && userPosts.length === 0 && (
            <p className={styles.emptyText}>
              You haven&apos;t created any posts yet.
            </p>
          )}

          {!postsLoading && !postsError && userPosts && userPosts.length > 0 && (
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
            disabled={userPosts.length < 6}
            onClick={() => setPage((p) => p + 1)}
          >
            Next
          </button>
        </div>
        </section>
      </div>
    );

}