import styles from "./PostCard.module.css";

import type { PostResponse } from "../../api/posts";

type Props = {
    post: PostResponse;
};

export function PostCard({post}: Props){

    //  caption: string;
    // imageUrl: string | null;
    // username: string;
    // createdAt: string;
    // totalLikes: number;
    // totalComments
    const caption = (post as any).caption ?? "";
    const imageUrl = (post as any).imageUrl ?? null;
    const username = (post as any).username ?? "User not found";
    const likeCount = (post as any).totalLikes ?? 0;
    const commentCount = (post as any).totalComments ?? 0;
    const createdAt = (post as any).createdAt ? new Date((post as any).createdAt).toLocaleString() : null;

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
            ❤️ {likeCount} · 💬 {commentCount}
          </span>
        </div>
      </div>
    </article>
  );
}