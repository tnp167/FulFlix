import { useDispatch, useSelector } from "react-redux";
import { RootState } from "@/redux/store";
import { logout } from "@/redux/userSlice";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { LogOut, User } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { FaTicketAlt } from "react-icons/fa";

const UserMenu = () => {
  const dispatch = useDispatch();
  const navigate = useNavigate();
  const user = useSelector((state: RootState) => state.user.user);

  console.log("User data in UserMenu:", user);

  const handleLogout = () => {
    dispatch(logout());
  };

  return (
    <DropdownMenu modal={false}>
      <DropdownMenuTrigger>
        <div className="relative flex items-center">
          <Avatar className="w-12 h-12">
            {user?.picture && (
              <AvatarImage
                src={user.picture}
                alt={user.firstName}
                className="rounded-full"
              />
            )}
            {!user?.picture && (
              <AvatarFallback className="bg-primary/10">
                <div className="font-bold">
                  {user?.firstName?.charAt(0).toUpperCase()}
                </div>
              </AvatarFallback>
            )}
          </Avatar>
        </div>
      </DropdownMenuTrigger>
      <DropdownMenuContent className="w-64 p-6 text-white" align="end">
        <div className="mb-2 p-2 flex flex-col items-center rounded-lg gap-2 bg-primary/25">
          <Avatar className="w-12 h-12">
            {user?.picture && (
              <AvatarImage
                src={user.picture}
                alt={user.firstName}
                className="rounded-full"
              />
            )}
            {!user?.picture && (
              <AvatarFallback className="bg-primary/10">
                <div className="font-bold">
                  {user?.firstName?.charAt(0).toUpperCase()}
                </div>
              </AvatarFallback>
            )}
          </Avatar>
          <p className="font-bold text-white text-sm">
            {user?.firstName} {user?.lastName}
          </p>
        </div>
        <DropdownMenuSeparator className="my-2 mb-5" />
        <DropdownMenuItem
          onClick={() => navigate("/my-account/profile")}
          className="group py-2 font-medium cursor-pointer focus:text-[#5e2a8b]"
        >
          <User
            size={18}
            className="mr-3 group-focus:-translate-y-1 transition-all duration-300"
          />
          Profile
        </DropdownMenuItem>
        <DropdownMenuItem
          onClick={() => navigate("/my-account/tickets")}
          className="group py-2 font-medium cursor-pointer focus:text-[#5e2a8b]"
        >
          <FaTicketAlt
            size={18}
            className="mr-3 group-focus:rotate-180 transition-all duration-300"
          />
          My tickets
        </DropdownMenuItem>
        <DropdownMenuItem
          onClick={handleLogout}
          className="group py-2 font-medium cursor-pointer focus:bg-destructive/20 focus:text-destructive"
        >
          <LogOut
            size={18}
            className=" mr-3 group-focus:translate-x-1 transition-all duration-300 "
          />
          Logout
        </DropdownMenuItem>
      </DropdownMenuContent>
    </DropdownMenu>
  );
};

export default UserMenu;
