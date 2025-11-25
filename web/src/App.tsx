import { useState } from "react";
import { login } from "./api/auth"
import { getMe, getPublicProfile } from "./api/users";

function App(){
  const [username, setUsername] = useState("");
  const [publicProfile, setPublicProfile] = useState<string | null>(null);

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [meProfile, setMeProfile] = useState<string | null>(null);


  const [error, setError] = useState<string | null>(null);


  async function handleLoadProfile() {
    setError(null);
    setPublicProfile(null);

    if (!username.trim()){
      setError("Please enter a username.");
      return;
    }

    try{
      const data = await getPublicProfile(username);
      setPublicProfile(JSON.stringify(data, null, 2));
    } catch (err: any) {
      setError(err.message ?? "Failed to load public profile.");
    }
  }

  async function handleLogin() {
    setError(null);
    setMeProfile(null);

    if (!email.trim() || !password.trim()) {
      setError("Please enter email and password.");
      return;
    }

    try {
      const result = await login(email, password);
      localStorage.setItem("token", result.token);

      const me = await getMe(result.token);
      setMeProfile(JSON.stringify(me, null, 2));
    } catch (err: any) {
      setError(err.message ?? "Login or /me failed.");
    }
  }

  async function handleLoadMe() {
    setError(null);
    setMeProfile(null);

    const token = localStorage.getItem("token");
    if (!token) {
      setError("No token found. Please login first.");
      return;
    }

    try {
      const me = await getMe(token);
      setMeProfile(JSON.stringify(me, null, 2));
    } catch (err: any) {
      setError(err.message ?? "Failed to load /me.");
    }
  }

   return (
    <div style={{ fontFamily: "sans-serif", padding: "2rem" }}>
      <h1>InstaCore Test Page</h1>

      {/* PUBLIC PROFILE SECTION */}
      <section style={{ marginBottom: "2rem" }}>
        <h2>Public Profile Lookup</h2>
        <p>Enter a username and load its public profile from the API.</p>

        <input
          type="text"
          placeholder="username (e.g. vankata)"
          value={username}
          onChange={(e) => setUsername(e.target.value)}
          style={{ padding: "0.5rem", minWidth: "250px" }}
        />

        <button
          onClick={handleLoadProfile}
          style={{ marginLeft: "0.5rem", padding: "0.5rem 1rem" }}
        >
          Load profile
        </button>

        {publicProfile && (
          <pre
            style={{
              marginTop: "1rem",
              padding: "1rem",
              background: "#f5f5f5",
              borderRadius: "4px",
              maxWidth: "600px",
              overflowX: "auto",
            }}
          >
            {publicProfile}
          </pre>
        )}
      </section>

      {/* AUTH + /ME SECTION */}
      <section style={{ marginBottom: "2rem" }}>
        <h2>Login & Me</h2>

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

        <button
          onClick={handleLogin}
          style={{ padding: "0.5rem 1rem", marginRight: "0.5rem" }}
        >
          Login & load /me
        </button>

        <button
          onClick={handleLoadMe}
          style={{ padding: "0.5rem 1rem" }}
        >
          Load /me with saved token
        </button>

        {meProfile && (
          <pre
            style={{
              marginTop: "1rem",
              padding: "1rem",
              background: "#e5f5ff",
              borderRadius: "4px",
              maxWidth: "600px",
              overflowX: "auto",
            }}
          >
            {meProfile}
          </pre>
        )}
      </section>

      {/* GLOBAL ERROR SECTION */}
      {error && (
        <div style={{ marginTop: "1rem", color: "red" }}>
          <strong>Error:</strong> {error}
        </div>
      )}
    </div>
  );
}


export default App;