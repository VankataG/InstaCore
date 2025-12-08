import { useEffect, useState } from "react";
import { type CommentResponse, deleteComment, getPostComments } from "../../api/comments";

import styles from "./CommentSection.module.css";
import CreateCommentForm from "../CreateCommentForm/CreateCommentForm";

type CommentSectionProps = {
    postId: string;
    currentUserUsername: string;
    onCommentCreated: (comment: CommentResponse) => void;
    onCommentDeleted: (commentId: string) => void;
};

export function CommentSection({ postId, currentUserUsername, onCommentCreated, onCommentDeleted}: CommentSectionProps) {
    const [comments, setComments] = useState<CommentResponse[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [deletingId, setDeletingId] = useState<string | null>(null);

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
        const token = localStorage.getItem("token");
        if (!token)  return;
        
        
        setError(null);
        setDeletingId(commentId);

        try {
            await deleteComment(token, postId, commentId);
            setComments(prev => prev.filter(c => c.commentId !== commentId));
            
            if(onCommentDeleted) {
              onCommentDeleted(commentId);
            }
        } catch (err: any) {
            setError(err.message ?? `Something went wrong, try again later.`)
        } finally {
            setDeletingId(null);
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
              {currentUserUsername === comment.username && 
                <button 
                  className={styles.deleteButton}
                  onClick={() => handleDelete(comment.commentId)}
                  disabled={deletingId === comment.commentId}
                >
                  X
                </button>}
            </div>
            <p className={styles.content}>{comment.text}</p>
          </div>
        ))}
      </div>
    );
}
