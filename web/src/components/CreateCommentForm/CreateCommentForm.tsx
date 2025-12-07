import React, { useState } from "react";

import styles from "./CreateCommentForm.module.css";
import { addComment, type CommentResponse } from "../../api/comments";

type Props = {
    postId: string;
    onCommentCreated?: (comment: CommentResponse) => void;
};

export default function CreateCommentForm({ postId, onCommentCreated }: Props) {
    const [text, setText] = useState<string>("");
    const [error, setError] = useState<string | null>(null);
    const [addingComment, setAddingComment] = useState<boolean>(false);

    async function handleCreate(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        const token = localStorage.getItem("token");
        if (!token) {
            setError(`Please login or register.`);
            return;
        }
        
        if (!text.trim()){
            setError(`You cannot add empty comment.`);
            return;
        }

        setAddingComment(true);
        const request = { text: text.trim()};
        
        try{
            const response = await addComment(token, postId, request)

            if(onCommentCreated) {
                onCommentCreated(response);
            }
            setText("");
        } catch (err: any) {
            setError(err.message ?? "Comment not added.");
        } finally {
            setAddingComment(false);
        }
    }

    return (
      <form className={styles.formContainer} onSubmit={handleCreate}>

        {error && <div className={styles.errorBox}>{error}</div>}

        <textarea
          className={styles.textarea}
          value={text}
          onChange={e => setText(e.target.value)}
          placeholder="Write a comment..."
          rows={2}
        />
    
        <button type="submit" className={styles.submitButton} disabled={addingComment}>
          {addingComment ? "Adding comment..." : "Add comment"}
        </button>
      </form>
    );
}
