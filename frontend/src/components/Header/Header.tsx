import { Link } from "react-router-dom";
// import Nav from "./Nav";
import MobileNav from "./MobileNav";
import Nav from "./Nav";

const links = [
  {
    name: "Films",
    path: "/films",
  },

  {
    name: "Cinemas",
    path: "/cinemas",
  },
  {
    name: "Sign Up",
    path: "/",
  },
  {
    name: "Login",
    path: "/",
  },
];

const Header = () => {
  return (
    <header className="py-6 xl:py-7 bg-primary text-secondary">
      <div className="container mx-auto flex justify-between items-center">
        <Link to="/">
          <h1 className="text-4xl font-semibold">LOGO</h1>
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
