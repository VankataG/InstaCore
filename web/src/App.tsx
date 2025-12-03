
import { Routes, Route, Link, Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage/LoginPage";
import MePage from "./pages/MePage/MePage";
import PublicProfilePage from "./pages/PublicProfilePage/PublicProfilePage";
import FeedPage from "./pages/FeedPage/FeedPage";
import RegisterPage from "./pages/RegisterPage/RegisterPage";

function App() {
  return (
    <div style={{ fontFamily: "sans-serif" }}>
      
      <header
        style={{
          padding: "1rem 2rem",
          borderBottom: "1px solid #eee",
          marginBottom: "1rem",
          display: "flex",
          justifyContent: "space-between",
        }}
      >
        <div>
          <Link to="/" style={{ textDecoration: "none", fontWeight: 700 }}>
            InstaCore
          </Link>
        </div>
        <nav style={{ display: "flex", gap: "1rem" }}>
          <Link to="/login">Login</Link>
          <Link to="/me">My Profile</Link>
          <Link to="/feed">Feed</Link>
        </nav>
      </header>

      <main>
        <Routes>
          <Route path="/" element={<Navigate to="/register" />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/me" element={<MePage />} />
          <Route path="/u/:username" element={<PublicProfilePage />} />
          <Route path="/feed" element={<FeedPage />} />
          <Route path="/register" element={<RegisterPage />} />
          {/* later: feed, post details, etc. */}
        </Routes>
      </main>
    </div>
  );
}

export default App;
