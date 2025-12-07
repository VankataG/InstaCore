import styles from "./PostCard.module.css";

import { type PostResponse } from "../../api/posts";
import { useEffect, useState } from "react";
import { likePost, unlikePost } from "../../api/likes";
import { CommentSection } from "../CommentSection/CommentSection";

type Props = {
    post: PostResponse;
};


export function PostCard({post}: Props){
    const caption = (post as any).caption ?? "";
    const imageUrl = (post as any).imageUrl ?? null;
    const username = (post as any).username ?? "User not found";
    const commentCount = (post as any).comments ?? 0;
    const createdAt = (post as any).createdAt ? new Date((post as any).createdAt).toLocaleString() : null;

    const [isLiked, setIsLiked] = useState<boolean>(post.isLikedByCurrentUser);
    const [likeCount, setLikeCount] = useState<number>(post.likes);

    const [viewComments, setViewComments] = useState<boolean>(false);    

    useEffect(() => {
      setIsLiked(post.isLikedByCurrentUser);
      setLikeCount(post.likes);
    }, [post.id, post.isLikedByCurrentUser, post.likes])

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

    

    return (
    <article className={styles.card}>
      {username}  
      {imageUrl && (
        <div className={styles.imageWrapper}>
          <img src={imageUrl} alt={caption || "Post image"} />
        </div>
      )}

      <div className={styles.content}>
        {caption && <p className={styles.caption}>{caption}</p>}

        <div className={styles.meta}>
          {createdAt && <span className={styles.date}>{createdAt}</span>}
          <span className={styles.stats}>
            <button onClick={handleToggleLike}>
              {isLiked ? "♥" : "♡"} {likeCount}
            </button>
            <button onClick={() => setViewComments(prev => !prev)}>
              💬 {commentCount}
            </button> 
          </span>
        </div>
        {viewComments && (
          <CommentSection postId={post.id} />
        )}
      </div>
    </article>
  );
}
