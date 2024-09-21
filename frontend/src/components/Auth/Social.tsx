import { Button } from "@/components/ui/button";
import { FcGoogle } from "react-icons/fc";
import { SiFacebook } from "react-icons/si";

const Socials = () => {
  return (
    <div className="flex flex-col justify-center mx-auto w-[90%] mb-4">
      <div className="text-secondary/80 text-center mt-[30px] mb-[50px] border-t-[1px] border-t-secondary">
        <span className="bg-primary px-[10px] absolute transform translate-x-[-50%] translate-y-[-50%]">
          Or
        </span>
      </div>
      <div className="flex flex-col items-center w-full gap-8">
        <Button className="flex h-12 w-full bg-[white]/90 hover:bg-[white]/80 gap-4 transition-all duration-200 ease-in-out">
          <p className="text-lg text-black">Sign in with Google</p>
          <FcGoogle className="w-6 h-6" />
        </Button>
        <Button className="flex h-12 w-full bg-[#1877F2] hover:bg-[#1877F2]/80 gap-4 transition-all duration-200 ease-in-out">
          <p className="text-lg">Sign in with Facebook</p>
          <SiFacebook className="w-6 h-6 rounded-xl bg-white text-[#0866FF] " />
        </Button>
      </div>
    </div>
  );
};

export default Socials;
