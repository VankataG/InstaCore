import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";

import { login } from "../../api/auth"
import { useUser } from "../../hooks/useUser";

import styles from "./LoginPage.module.css";

export default function LoginPage() {
    const navigate = useNavigate();
    const { user, loadingUser, refreshUser } = useUser();

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
      if (!loadingUser && user){
        navigate("/me");
      }
    }, [ loadingUser, user, navigate]);

    async function handleSubmit(e: React.FormEvent) {
        e.preventDefault();
        setError(null);

        if (!email.trim() || !password.trim()) {
            setError("Please enter email and password.");
            return;
        }

        try {
            const response = await login(email, password);
            localStorage.setItem("token", response.token);

            await refreshUser();

            navigate("/feed");
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
                id="email"
                type="email"
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

            <button 
              type="submit" 
              className={styles.button}>Log in</button>

          </form>

          <p className={styles.footerText}>
            Don&apos;t have an account yet?{" "}
            <button 
              onClick={ () => navigate("/register") }
              className={styles.link}>
                Sign up
            </button>
          </p>
        </div>
      </div>
    );
}