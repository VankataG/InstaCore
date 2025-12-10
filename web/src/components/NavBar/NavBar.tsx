import { NavLink, useNavigate } from "react-router-dom";
import { useUser } from "../../hooks/useUser";

import styles from "./NavBar.module.css";

export default function NavBar(){
    const { user, logout} = useUser();
    const navigate = useNavigate();

    function handleLogoClick(){
        if(user) 
            navigate("/feed");
        else 
            navigate("/login");
    }

    function handleLogout(){
        logout();
        navigate("/login");
    }

    return(
        <header className={styles.header}>
            <div 
                className={styles.logo}
                onClick={handleLogoClick}
            >
              InstaCore
            </div>
        <nav className={styles.nav}>
            {user ? (
                <>
                    <NavLink
                        to={"/feed"}
                        className={({ isActive }) =>
                            isActive ? styles.activeLink : styles.link
                        }
                    >
                        Feed
                    </NavLink>
                    <NavLink
                        to={"/me"}
                        className={({ isActive }) =>
                            isActive ? styles.activeLink : styles.link
                        }
                    >
                        My Profile
                    </NavLink>
                    <button
                        type="button"
                        className={styles.logoutButton}
                        onClick={handleLogout}
                    >
                        Log out
                    </button>
                </>
            ) : (
                <>
                    <NavLink
                        to={"/login"}
                        className={({ isActive }) =>
                            isActive ? styles.activeLink : styles.link
                        }
                    >
                        Log in
                    </NavLink>
                    <NavLink
                        to={"/register"}
                        className={({ isActive }) =>
                            isActive ? styles.activeLink : styles.link
                        }
                    >
                        Register
                    </NavLink>
                </>
            )}

        </nav>
      </header>
    )
}