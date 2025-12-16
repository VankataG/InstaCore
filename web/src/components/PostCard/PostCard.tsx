import styles from "./PostCard.module.css";

import { deletePost, type PostResponse } from "../../api/posts";
import { useEffect, useState } from "react";
import { likePost, unlikePost } from "../../api/likes";
import { CommentSection } from "../CommentSection/CommentSection";
import { useUser } from "../../hooks/useUser";
import { Link } from "react-router-dom";

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

type Props = {
    post: PostResponse;
    onPostDeleted?: (postId: string) => void;
};


export function PostCard({post, onPostDeleted}: Props){
    const userContext = useUser();
    const currentUserUsername = userContext.user?.username;
    
    const username = (post as any).username ?? "User not found";
    const caption = (post as any).caption ?? "";
    const imageUrl = (post as any).imageUrl ?? null;
    const imageSrc = imageUrl && imageUrl.startsWith("http") ? imageUrl : imageUrl ? `${API_BASE_URL}${imageUrl}` : null;
    const createdAt = (post as any).createdAt ? new Date((post as any).createdAt).toLocaleString() : null;

    const userAvatarUrl = (post as any).userAvatarUrl ?? null;
    const avatarSrc = userAvatarUrl && userAvatarUrl.startsWith("http") ? userAvatarUrl : userAvatarUrl ? `${API_BASE_URL}${userAvatarUrl}` : undefined;

    const [isImageOpen, setIsImageOpen] = useState(false);

    const [isLiked, setIsLiked] = useState<boolean>(post.isLikedByCurrentUser);
    const [likeCount, setLikeCount] = useState<number>(post.likes);

    const [viewComments, setViewComments] = useState<boolean>(false);   
    const [commentCount, setCommentCount] = useState<number>(post.comments) 

    const canDelete = currentUserUsername === post.username;
    const [isDeleting, setIsDeleting] = useState<boolean>(false);
    const [deleteError, setDeleteError] = useState<string | null>(null);

    useEffect(() => {
      setIsLiked(post.isLikedByCurrentUser);
      setLikeCount(post.likes);
      setCommentCount(post.comments);
    }, [post.id, post.isLikedByCurrentUser, post.likes, post.comments])

    async function handleToggleLike(){
      const token = localStorage.getItem("token");
      if(!token) return;

      if (isLiked){
        await unlikePost(token, post.id);
        setIsLiked(false);
        setLikeCount(l => Math.max(0, l - 1));
      } else {
        await likePost(token, post.id);
        setIsLiked(true);
        setLikeCount(l => l + 1);
      }
    }

    async function handleDelete() {
      setDeleteError(null);

      const token = localStorage.getItem("token");
      if(!token) {
        setDeleteError("Please login to delete this post.");
        return;
      }

      const confirmDelete = window.confirm("Are you sure you want to delete this post?");
      if (!confirmDelete) return;

      setIsDeleting(true);

      try {
        await deletePost(token, post.id);

        if(onPostDeleted) {
          onPostDeleted(post.id);
        }
      } catch (err: any) {
        setDeleteError(err.message ?? "Failed to delete post.")
      } finally {
        setIsDeleting(false);
      }
    }

    
    return (
    <article className={styles.card}>
      <header className={styles.header}>
        <div className={styles.userBlock}>
          {avatarSrc ? (
            <img 
            src={avatarSrc}
            className={styles.avatar}
            />
          ) : (
            <div className={styles.avatar}>
              {username[0]?.toUpperCase()}
            </div>
          )}

          <div className={styles.userMeta}>
            <Link to={`/u/${username}`}>
              <div className={styles.username}>{username}</div>
            </Link>
            {createdAt && <div className={styles.date}>{createdAt}</div>}
          </div>
        </div>

        {canDelete && (
          <button
            className={styles.iconButtonDanger}
            onClick={handleDelete}
            disabled={isDeleting}
            title="Delete post"
            type="button"
          >
            ❌
          </button>
        )}
      </header>

      {imageSrc && (
        <button
          type="button"
          className={styles.imageButton}
          onClick={ () => setIsImageOpen(true)}
          aria-label="Open image"
        >
          <div className={styles.imageWrapper}>
            <img src={imageSrc} alt={caption || "Post image"} />
          </div>
        </button>
      )}

      <div className={styles.content}>
        {caption && <p className={styles.caption}>{caption}</p>}

        <div className={styles.actionsRow}>
          <div className={styles.actionsLeft}>
            <button
              type="button"
              className={`${styles.iconButton} ${isLiked ? styles.liked : ""}`}
              onClick={handleToggleLike}
            >
              <span className={styles.icon}>{isLiked ? "♥" : "♡"}</span>
              <span className={styles.count}>{likeCount}</span>
            </button>

            <button
              type="button"
              className={styles.iconButton}
              onClick={() => setViewComments(prev => !prev)}
            >
              <span className={styles.icon}>💬</span>
              <span className={styles.count}>{commentCount}</span>
            </button>
          </div>
        </div>

        {viewComments && (
          <CommentSection 
            postId={post.id}
            currentUserUsername={currentUserUsername}
            onCommentCreated={() => {
              setCommentCount( c => c + 1);
            }}
            onCommentDeleted={() => {
              setCommentCount( c => Math.max(0, c - 1));
            }}
          />
        )}
      </div>

      {deleteError && <div className={styles.errorBox}>{deleteError}</div>}

      {isImageOpen && imageSrc && (
        <div
          className={styles.modalOverlay}
          onClick={() => setIsImageOpen(false)}
          role="dialog"
          aria-modal="true"
        >
          <button
            type="button"
            className={styles.modalClose}
            onClick={() => setIsImageOpen(false)}
            aria-label="Close"
          >
            ❌
          </button>

          <div className={styles.modalContent} onClick={(e) => e.stopPropagation()}>
            <img src={imageSrc} alt="Full size" className={styles.modalImage} />
          </div>
        </div>
      )}

    </article>
  );
}
