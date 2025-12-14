import { useState } from "react";
import { createPost, type PostResponse } from "../../api/posts";

import styles from "./CreatePostForm.module.css";
import { uploadPostImage } from "../../utils/uploads";

type CreatePostFormProps = {
    onPostCreated?: (post: PostResponse) => void;
}

export default function CreatePostForm({ onPostCreated}: CreatePostFormProps) {
    const [caption, setCaption] = useState("");
    const [selectedFile, setSelectedFile] = useState<File | null>(null);

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
        
        if (!caption.trim() && !selectedFile){
            setError(`You cannot create empty post.`);
            return;
        }
        
        setIsLoading(true);

        try {
            let uploadedUrl: string | null = null;
            if(selectedFile){
              uploadedUrl= (await uploadPostImage(selectedFile, token)).url;
            }

            const request = { 
                caption: caption.trim(), 
                imageUrl: uploadedUrl
            };

            const response = await createPost(token, request);

            if( onPostCreated) {
                onPostCreated(response);
            }

            setCaption("");
            setSelectedFile(null);
            
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
          <label className={styles.label}>Upload image (optional)</label>
          <input
            type="file"
            accept="image/*"
            className={styles.input}
            onChange={ (e: React.ChangeEvent<HTMLInputElement>) => setSelectedFile(e.target.files?.[0] ?? null)}
            disabled = {isLoading}
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