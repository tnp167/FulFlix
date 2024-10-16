import { useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@/redux/store";
import { logout } from "@/redux/userSlice";

const UserMenu = () => {
  const [isOpen, setIsOpen] = useState(true);
  const dispatch = useDispatch();
  const user = useSelector((state: RootState) => state.user.user);

  console.log("User data in UserMenu:", user);

  const handleLogout = () => {
    dispatch(logout());
    setIsOpen(false);
  };

  const toggleMenu = () => setIsOpen(!isOpen);

  return (
    <div className="relative flex items-center">
      <img
        src={user?.picture || "/default-avatar.png"}
        alt="Avatar"
        className="w-12 h-12 rounded-full cursor-pointer"
        onClick={toggleMenu}
      />

      <div className="absolute right-0 mt-2 w-48 bg-primary rounded-md shadow-lg py-1">
        <div className="px-4 py-2 text-sm text-secondary">
          {user?.firstName} {user?.lastName}
        </div>
        <button
          onClick={handleLogout}
          className="block w-full text-left px-4 py-2 text-sm text-secondary hover:bg-secondary hover:text-primary"
        >
          Logout
        </button>
      </div>
    </div>
  );
};

export default UserMenu;
