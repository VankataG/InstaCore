import { useState } from "react";
import { createPost, type PostResponse } from "../../api/posts";

import styles from "./CreatePostForm.module.css";

type CreatePostFormProps = {
    onPostCreated?: (post: PostResponse) => void;
}

export default function CreatePostForm({ onPostCreated}: CreatePostFormProps) {
    const [caption, setCaption] = useState("");
    const [imageUrl, setImageUrl] = useState("");
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState<boolean>(false);

    async function handleCreate(e: React.FormEvent) {
        e.preventDefault();
        setError(null);
        
        const token = localStorage.getItem("token");
        if (!token) {
            setError(`Please login or register.`);
            return;
        }
        
        if (!caption.trim()){
            setError(`You cannot create empty post.`);
            return;
        }
        
        setIsLoading(true);
        const request = { 
            caption: caption.trim(), 
            imageUrl: imageUrl.trim() !== "" ? imageUrl.trim() : null
        };

        try {
            const response = await createPost(token, request);

            if( onPostCreated) {
                onPostCreated(response);
            }

            setCaption("");
            setImageUrl("");
        } catch (err: any) {
            setError(err.message);
        } finally {
            setIsLoading(false);
        }
    }

    return (
      <form className={styles.formWrapper} onSubmit={handleCreate}>
        <div className={styles.title}>Create a post</div>

        {error && <div className={styles.errorBox}>{error}</div>}

        <div className={styles.field}>
          <label className={styles.label}>Caption</label>
          <textarea
            className={styles.textarea}
            value={caption}
            onChange={(e) => setCaption(e.target.value)}
            placeholder="What's on your mind?"
          />
        </div>

        <div className={styles.field}>
          <label className={styles.label}>Image URL (optional)</label>
          <input
            className={styles.input}
            value={imageUrl}
            onChange={(e) => setImageUrl(e.target.value)}
            placeholder="https://example.com/photo.jpg"
          />
        </div>

        <div className={styles.actions}>
          <button className={styles.buttonPrimary} disabled={isLoading}>
            {isLoading ? "Posting..." : "Post"}
          </button>
        </div>
      </form>
    );

}