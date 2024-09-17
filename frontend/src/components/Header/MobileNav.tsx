import { Link, useLocation } from "react-router-dom";
import { Sheet, SheetContent, SheetTrigger } from "@/components/ui/sheet";
import { Menu } from "lucide-react";
type linkProps = {
  name: string;
  path: string;
};

const MobileNav = ({ links }: { links: linkProps[] }) => {
  const location = useLocation();
  return (
    <Sheet>
      <SheetTrigger>
        <Menu size={32} className="text-secondary" />
      </SheetTrigger>
      <SheetContent>
        <nav className="text-secondary flex flex-col justify-center items-center gap-[50px] text-xl">
          {links!.map((link, index) => {
            const isLink = link.path === "/films" || link.path === "/cinemas";

            return isLink ? (
              <Link
                to={link.path}
                key={index}
                className={`${
                  link.path === location.pathname &&
                  "border-b-3 border-primary hover:text-primary transition-all"
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
      </SheetContent>
    </Sheet>
  );
};

export default MobileNav;
