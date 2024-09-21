import { Link, useLocation } from "react-router-dom";
import AuthModal from "../Auth/AuthModal";

type linkProps = {
  name: string;
  path: string;
};

const Nav = ({ links }: { links: linkProps[] }) => {
  const location = useLocation();
  return (
    <nav className="text-secondary flex gap-[50px] text-lg">
      {links?.map((link, index) => {
        return (
          <Link
            to={link.path}
            key={index}
            className={`${
              link.path === location.pathname &&
              "border-b-2 border-secondary hover:text-white transition-all"
            }font-medium hover:text-accent transition-all flex items-center`}
          >
            {link.name}
          </Link>
        );
      })}
      <div className="ml-5 flex gap-[20px]">
        <AuthModal />
      </div>
    </nav>
  );
};

export default Nav;
