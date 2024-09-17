import { Link, useLocation } from "react-router-dom";

type linkProps = {
  name: string;
  path: string;
};

const Nav = ({ links }: { links: linkProps[] }) => {
  const location = useLocation();
  return (
    <nav className="text-secondary flex gap-[50px] text-lg">
      {links?.map((link, index) => {
        const isLink = link.path === "/films" || link.path === "/cinemas";

        return isLink ? (
          <Link
            to={link.path}
            key={index}
            className={`${
              link.path === location.pathname &&
              "border-b-2 border-secondary hover:text-primary transition-all"
            }font-medium hover:text-accent transition-all`}
          >
            {link.name}
          </Link>
        ) : (
          <span
            key={index}
            className="cursor-pointer font-medium hover:text-accent transition-all"
          >
            {link.name}
          </span>
        );
      })}
    </nav>
  );
};

export default Nav;
