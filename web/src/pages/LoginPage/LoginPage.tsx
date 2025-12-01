import { useState } from "react";
import { login } from "../../api/auth"
import { getMe } from "../../api/users";
import { useNavigate } from "react-router-dom";

import styles from "./LoginPage.module.css";


export default function LoginPage() {
    const navigate = useNavigate();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if (!email.trim() || !password.trim()) {
            setError("Please enter email and password.");
            return;
        }

        try {
            const result = await login(email, password);
            localStorage.setItem("token", result.token);

            await getMe(result.token);

            navigate("/me");
        } catch (err: any) {
            setError(err.message ?? "Login failed.");
        }
    }

    return (
      <div className={styles.pageWrapper}>
        <div className={styles.card}>
          <div className={styles.logo}>InstaCore</div>

          <h1 className={styles.title}>Welcome back</h1>
          <p className={styles.subtitle}>Log in to continue to your account.</p>

          {error && <div className={styles.errorBox}>{error}</div>}

          <form className={styles.form} onSubmit={handleSubmit}>
            <div className={styles.field}>
              <label className={styles.label} htmlFor="email">
                Email
              </label>
              <input
                id="username"
                type="text"
                className={styles.input}
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                autoComplete="username"
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

            <button type="submit" className={styles.button}>Log in</button>
          </form>

          <p className={styles.footerText}>
            Don&apos;t have an account yet?{" "}
            <span className={styles.link}>Sign up (soon)</span>
          </p>
        </div>
      </div>
    );

}