import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { updateProfile} from "../../api/users";
import { PostCard } from "../../components/PostCard/PostCard";
import { getUserPosts, type PostResponse } from "../../api/posts";

import styles from "./MePage.module.css";
import CreatePostForm from "../../components/CreatePostForm/CreatePostForm";
import { useUser } from "../../hooks/useUser";
import { uploadAvatar } from "../../utils/uploads";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export default function MePage() {
    const navigate = useNavigate();
    const { user, loadingUser, refreshUser, logout } = useUser();

    const [userPosts, setUserPosts] = useState<PostResponse[]>([]);
    const [postsLoading, setPostsLoading] = useState<boolean>(true);
    const [postsError, setPostsError] = useState<string | null>(null);
    const [page, setPage] = useState<number>(1);

    const [editing, setEditing] = useState<boolean>(false);
    const [editBio, setEditBio] = useState<string | null>(null);
    const [editError, setEditError] = useState<string | null>(null);

    const [selectedAvatar, setSelectedAvatar] = useState<File | null>(null);
    const avatarUrl = user?.avatarUrl ?? null;
    const avatarSrc = avatarUrl && avatarUrl.startsWith("http") ? avatarUrl : avatarUrl ? `${API_BASE_URL}${avatarUrl}` : null;


    useEffect(() => {
        async function loadPosts() {
            if(user){
                setPostsError(null);
                setPostsLoading(true);

                try{
                    const response = await getUserPosts(user.username, page, 6);
                    setUserPosts(response)
                } catch (err: any) {
                    setPostsError(err.message || "Failed to load user posts.");
                } finally {
                    setPostsLoading(false);
                }

            }
        }
        loadPosts();
    }, [user, page]);

    async function editProfile() {
        if(!user) return;

        const token = localStorage.getItem("token");
        if (!token) {
            setEditError("No token found. Please login first.");
            setEditing(false);
            return;
        }

        try{
            setEditError(null);
            setEditing(true);
            
            let uploadedUrl: string | null = null;
            if(selectedAvatar){
              uploadedUrl= (await uploadAvatar(selectedAvatar, token)).url;
            }

            const request = { bio: editBio , avatarUrl: uploadedUrl ?? user.avatarUrl};
            await updateProfile(request, token);
            await refreshUser();
        } catch (err: any) {
            setEditError(err.message ?? "Failed to update profile.");
        } finally {
            setEditing(false);
            setSelectedAvatar(null);
        }
            
    }

    function startEdit(){
        if (!user) return;

        setEditing(true);
        setEditError(null);
        setEditBio(user.bio);
    }

    function cancelEdit(){
        setEditing(false);
        setEditError(null);
    }

    function handleLogout() {
        logout();
        navigate("/login");
    }


    if (loadingUser) {
        return (
          <div style={{ padding: "2rem" }}>
            <p>Loading your profile...</p>
          </div>
        );
    }

    if (!user) {
        return (
          <div style={{ padding: "2rem" }}>
            <p>No profile loaded. Please login.</p>
            <button onClick={() => navigate("/login")}>Go to login</button>
          </div>
        );
    }

    return (
      <div className={styles.pageContainer}>
        <section className={styles.profileSection}>
          <div className={styles.profileCard}>
            <div className={styles.avatarWrapper}>
              {avatarSrc ? (
                <img src={avatarSrc} alt={user.username} className={styles.avatarImage} />
              ) : (
                <div className={styles.avatarInitials}>{user.username[0]?.toUpperCase()}</div>
              )}

            </div>
            <div className={styles.profileMain}>
              <div className={styles.topRow}>
                <h1 className={styles.username}>{user.username}</h1>
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
                    {"postsCount" in user && (user as any).postsCount != null
                      ? (user as any).postsCount
                      : userPosts?.length ?? 0}
                  </strong>{" "}
                  posts
                </span>
                <span>
                  <strong>
                    {user.followers}
                  </strong>{" "}
                   followers
                </span>
                <span>
                  <strong>
                    {user.following}
                  </strong>{" "}
                   following
                </span>
              </div>
              {!editing ? (
                  user.bio ? (
                      <p className={styles.bio}>{user.bio}</p>
                  ) : (
                      <p className={styles.bioPlaceholder}>You don't have a bio yet.</p>
                  )
              ) : (
                <>
                  <div className={styles.editAvatarField}>
                      <label className={styles.label} htmlFor="avatarUrl">Avatar</label>
                      <input id="avatarUrl" 
                          type="file" 
                          className={styles.input}
                          onChange={(e: React.ChangeEvent<HTMLInputElement>) => setSelectedAvatar(e.target.files?.[0] ?? null)}
                      />
                  </div>
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
                  </>
              )}
            </div>
          </div>
        </section>

        <section className={styles.postsSection}>
          <div className={styles.postsHeader}>
            <h2 className={styles.sectionTitle}>My posts</h2>
          </div>
          
            <CreatePostForm
              onPostCreated={ post => {
                setUserPosts(prev => [post, ...prev])
              }}
            />

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
                <PostCard 
                  key={(post as any).id} 
                  post={post}
                  onPostDeleted={ id => {
                      setUserPosts(prev => prev.filter(p => p.id !== id))
                  }}
                />
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