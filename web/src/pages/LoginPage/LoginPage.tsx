import { useState } from "react";
import { login } from "../../api/auth"
import { getMe } from "../../api/users";
import { useNavigate } from "react-router-dom";


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
        <div style={{ padding: "2rem" }}>
            <h2>Login</h2>

            <form onSubmit={handleSubmit}>
                <div style={{ marginBottom: "1rem" }}>
                    <input
                        type="email"
                        placeholder="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        style={{ padding: "0.5rem", minWidth: "250px" }}
                    />
                </div>

                <div style={{ marginBottom: "1rem" }}>
                  <input
                    type="password"
                    placeholder="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    style={{ padding: "0.5rem", minWidth: "250px" }}
                  />
                </div>

                <button type="submit" style={{ padding: "0.5rem 1rem" }}>
                  Login
                </button>
            </form>

            {error && (
              <div style={{ marginTop: "1rem", color: "red" }}>
                <strong>Error:</strong> {error}
              </div>
            )}
        </div>
    );
}