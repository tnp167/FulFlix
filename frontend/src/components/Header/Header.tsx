import { Link } from "react-router-dom";
import MobileNav from "./MobileNav";
import Nav from "./Nav";
import fulflix from "@/assets/logo/fulflix.png";
import EmailVerificationBar from "@/components/EmailVerificationBar";

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
    <>
      <EmailVerificationBar />
      <header className="py-4 xl:py-5 bg-primary text-secondary">
        <div className="mx-2 sm:container sm:mx-auto flex justify-between items-center ">
          <Link to="/">
            <img src={fulflix} className="w-36 h-auto" />
          </Link>

          <div className="hidden lg:flex items-center gap-8">
            <Nav links={links} />
          </div>

          <div className="lg:hidden">
            <MobileNav links={links} />
          </div>
        </div>
      </header>
    </>
  );
};

export default Header;
