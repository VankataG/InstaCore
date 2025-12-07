import { useEffect, useState } from "react";
import { type CommentResponse, getPostComments } from "../../api/comments";

import styles from "./CommentSection.module.css";

type CommentSectionProps = {
    postId: string;
}

export function CommentSection({ postId }: CommentSectionProps) {
    const [comments, setComments] = useState<CommentResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        async function loadComments(){
            setError(null);
            setLoading(true);

            try{
                const comments = await getPostComments(postId);
                setComments(comments);
            } catch (err: any) {
                setError(err.message ?? "Failed to load comments");
            } finally {
                setLoading(false);
            }
        }

        loadComments();
    }, [ postId ]);

    if (loading) {
      return <div className={styles.skeleton}>Loading comments...</div>;
    }

    if (error) {
      return <div className={styles.errorBox}>{error}</div>;
    }

    if (comments.length === 0) {
      return <div className={styles.emptyText}>No comments yet.</div>;
    }

    return (
      <div className={styles.commentsWrapper}>
        {comments.map((comment) => (
          <div key={comment.id} className={styles.comment}>
            <div className={styles.commentHeader}>
              <span className={styles.username}>{comment.username}</span>
              <span className={styles.date}>
                {new Date(comment.createdAt).toLocaleString()}
              </span>
            </div>
            <p className={styles.content}>{comment.text}</p>
          </div>
        ))}
      </div>
    );
}
