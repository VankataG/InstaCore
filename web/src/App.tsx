
import { Routes, Route, Navigate } from "react-router-dom";

import LoginPage from "./pages/LoginPage/LoginPage";
import MePage from "./pages/MePage/MePage";
import PublicProfilePage from "./pages/PublicProfilePage/PublicProfilePage";
import FeedPage from "./pages/FeedPage/FeedPage";
import RegisterPage from "./pages/RegisterPage/RegisterPage";

import NavBar from "./components/NavBar/NavBar";
import styles from "./App.module.css";

function App() {
  return (
    <div className={styles.app}>
      <NavBar />
      

      <main className={styles.main}>
        <Routes>
          <Route path="/" element={<Navigate to="/register" />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/me" element={<MePage />} />
          <Route path="/u/:username" element={<PublicProfilePage />} />
          <Route path="/feed" element={<FeedPage />} />
          <Route path="/register" element={<RegisterPage />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
