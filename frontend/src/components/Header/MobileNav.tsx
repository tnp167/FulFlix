import { Link, useLocation } from "react-router-dom";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { LogOut, Menu, User } from "lucide-react";
import AuthModal from "../Auth/AuthModal";
import { useSelector } from "react-redux";
import { RootState } from "@/redux/store";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { FaTicketAlt } from "react-icons/fa";
import { Separator } from "@/components/ui/separator";

type linkProps = {
  name: string;
  path: string;
};

const MobileNav = ({ links }: { links: linkProps[] }) => {
  const location = useLocation();
  const user = useSelector((state: RootState) => state.user.user);

  return (
    <Sheet>
      <SheetTrigger>
        <Menu size={32} className="text-secondary" />
      </SheetTrigger>
      <SheetContent>
        {user && (
          <div className="mb-6 p-4 flex flex-col sm:flex-row w-full items-center rounded-lg gap-5 sm:gap-12 bg-[#5e2a8b]">
            <Avatar className="w-12 h-12">
              {user.picture && (
                <AvatarImage
                  src={user.picture}
                  alt={user.firstName}
                  className="rounded-full"
                />
              )}
              {!user.picture && (
                <AvatarFallback className="bg-primary/10">
                  <div className="font-bold">
                    {user.firstName?.charAt(0).toUpperCase()}
                  </div>
                </AvatarFallback>
              )}
            </Avatar>
            <div className="flex flex-col justify-center">
              <p className="font-semibold text-white text-sm text-center">
                {user.firstName} {user.lastName}
              </p>
              <p className="font-semibold text-white text-sm">{user.email}</p>
            </div>
          </div>
        )}
        <nav className="text-secondary flex flex-col justify-center items-center gap-[50px] text-xl">
          {links.map((link, index) => (
            <Link
              to={link.path}
              key={index}
              className={`${
                link.path === location.pathname &&
                "border-b-3 border-primary hover:text-primary transition-all"
              } px-6 py-2 bg-primary rounded-[6px] text-secondary hover:bg-secondary hover:text-primary transition-all duration-200`}
            >
              {link.name}
            </Link>
          ))}
        </nav>
        <Separator className="my-10" />
        <div>
          {user ? (
            <div className="flex flex-col justify-center items-center gap-[50px] text-xl">
              <Link
                to="/my-account/profile"
                className="group flex items-center gap-2 px-6 py-2 bg-primary rounded-[6px] text-secondary hover:bg-secondary hover:text-primary transition-all duration-100"
              >
                <User
                  size={20}
                  className="mr-3 group-hover:-translate-y-1 transition-all duration-300"
                />
                Profile
              </Link>
              <Link
                to="/my-account/tickets"
                className="group flex items-center gap-2 px-6 py-2 bg-primary rounded-[6px] text-secondary hover:bg-secondary hover:text-primary transition-all duration-100"
              >
                <FaTicketAlt
                  size={20}
                  className="mr-3 group-hover:rotate-180 transition-all duration-300"
                />
                My Tickets
              </Link>
              <Link
                to="/my-account/profile"
                className="group flex items-center gap-2 px-6 py-2 bg-primary rounded-[6px] text-secondary hover:bg-destructive hover:text-white transition-all duration-100"
              >
                <LogOut
                  size={20}
                  className=" mr-3 group-hover:translate-x-1 transition-all duration-300 "
                />
                Logout
              </Link>
            </div>
          ) : (
            <AuthModal showUserMenu={false} />
          )}
        </div>
      </SheetContent>
    </Sheet>
  );
};

export default MobileNav;
