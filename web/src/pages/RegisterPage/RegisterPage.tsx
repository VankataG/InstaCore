import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { register } from "../../api/auth";
import { getMe } from "../../api/users";

import styles from "./RegisterPage.module.css";

export default function RegisterPage() {
    const navigate = useNavigate();

    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if(!username.trim()) {
            setError("Please enter username.");
            return;
        }

        if (!email.trim()) {
            setError("Please enter email.");
            return;
        }

        if(!password.trim() || !confirmPassword.trim()) {
            setError("Please enter password");
            return;
        }

        if(password !== confirmPassword) {
            setError("Passwords should match.");
            return;
        }

        try {
            const result = await register({ username, email, password});
            localStorage.setItem("token", result.token);

            await getMe(result.token);

            navigate("/me");
        } catch (err: any) {
            setError(err.message ?? "Register failed.");
        }
    }

    return (
      <div className={styles.pageWrapper}>
        <div className={styles.card}>
          <div className={styles.logo}>InstaCore</div>

          <h1 className={styles.title}>Create account</h1>
          <p className={styles.subtitle}>Register to start your journey.</p>

          {error && <div className={styles.errorBox}>{error}</div>}

          <form className={styles.form} onSubmit={handleSubmit}>
            <div className={styles.field}>
              <label className={styles.label} htmlFor="username">
                Username
              </label>
              <input
                id="username"
                type="text"
                className={styles.input}
                value={username}
                onChange={(e) => setUsername(e.target.value)}
                autoComplete="username"
              />
            </div>

            <div className={styles.field}>
              <label className={styles.label} htmlFor="email">
                Email
              </label>
              <input
                id="email"
                type="text"
                className={styles.input}
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="email"
              />
            </div>

            <div className={styles.field}>
              <label className={styles.label} htmlFor="password">
                Password
              </label>
              <input
                id="password"
                type="password"
                className={styles.input}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                autoComplete="current-password"
              />
            </div>

            <div className={styles.field}>
              <label className={styles.label} htmlFor="confirmPassword">
                Confirm Password
              </label>
              <input
                id="confirmPassword"
                type="password"
                className={styles.input}
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                autoComplete="current-password"
              />
            </div>

            <button type="submit" className={styles.button}>Register</button>
          </form>

          <p className={styles.footerText}>
            Already have an account?{" "}
            <button 
              onClick={() => navigate("/login")}
              className={styles.link}
            >
              Log in
            </button>
          </p>
        </div>
      </div>
    );
}