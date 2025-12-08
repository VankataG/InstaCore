import { useEffect, useState } from "react";
import { type CommentResponse, deleteComment, getPostComments } from "../../api/comments";

import styles from "./CommentSection.module.css";
import CreateCommentForm from "../CreateCommentForm/CreateCommentForm";

type CommentSectionProps = {
    postId: string;
    onCommentCreated: (comment: CommentResponse) => void;
}

export function CommentSection({ postId, onCommentCreated}: CommentSectionProps) {
    const [comments, setComments] = useState<CommentResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [isDeleting, setIsDeleting] = useState<boolean>(false);
    const [deleteError, setDeleteError] = useState<string | null>(null);

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

    async function handleDelete(commentId: string){
        setDeleteError(null);

        const token = localStorage.getItem("token");
        if (!token){
            setDeleteError("You can't delete this comment.");
            return;
        }

        setIsDeleting(true);

        try {
            await deleteComment(token, postId, commentId);
            setComments(prev => prev.filter(c => c.commentId !== commentId));
        } catch (err: any) {
            setDeleteError(err.message ?? `Something went wrong, try again later.`)
        } finally {
            setIsDeleting(false);
        }
    };

    if (loading) {
      return <div className={styles.skeleton}>Loading comments...</div>;
    }

    if (error) {
      return <div className={styles.errorBox}>{error}</div>;
    }

    return (
      <div className={styles.commentsWrapper}>
        <CreateCommentForm 
            postId={postId} 
            onCommentCreated={(comment) => {
                setComments(prev => [comment, ...prev]);

                if(onCommentCreated){
                    onCommentCreated(comment)
                }
            }}
        />
        {comments.map((comment) => (
          <div key={comment.commentId} className={styles.comment}>
            <div className={styles.commentHeader}>
              <span className={styles.username}>{comment.username}</span>
              <span className={styles.date}>
                {new Date(comment.createdAt).toLocaleString()}
              </span>
              <button 
                onClick={() => handleDelete(comment.commentId)}
                disabled={isDeleting}
              >
                X
              </button>
            </div>
            {deleteError && <div className={styles.errorBox}>{error}</div>}
            <p className={styles.content}>{comment.text}</p>
          </div>
        ))}
      </div>
    );
}
