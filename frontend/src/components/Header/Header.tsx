import { Link } from "react-router-dom";
// import Nav from "./Nav";
import MobileNav from "./MobileNav";
import Nav from "./Nav";
import fullflix from "@/assets/logo/fulflix.png";
const links = [
  {
    name: "Films",
    path: "/films",
  },

  {
    name: "Cinemas",
    path: "/cinemas",
  },
];

const Header = () => {
  return (
    <header className="py-4 xl:py-5 bg-primary text-secondary">
      <div className="mx-2 sm:container sm:mx-auto flex justify-between items-center ">
        <Link to="/">
          <img src={fullflix} className="w-36 h-auto" />
        </Link>

        <div className="hidden lg:flex items-center gap-8">
          <Nav links={links} />
        </div>

        <div className="lg:hidden">
          <MobileNav links={links} />
        </div>
      </div>
    </header>
  );
};

export default Header;
