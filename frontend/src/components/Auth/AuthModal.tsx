import { useState } from "react";
import { Modal, ModalBody, ModalTrigger } from "@/components/ui/animated-modal";
import Login from "./Login";
import SignUp from "./SignUp";
import ResetPassword from "./RequestPasswordReset";

const AuthModal = () => {
  const [type, setType] = useState<"signup" | "login" | "resetPassword" | null>(
    null
  );
  return (
    <div className="bg-primary gap-[20px] text-lg font-medium transition-all flex items-center justify-center">
      <Modal>
        <ModalTrigger
          onClick={() => setType("signup")}
          className="p-[3px] relative"
        >
          <div className="absolute inset-0 bg-gradient-to-r from-purple-700  to-purple-500 rounded-lg" />
          <div className="px-6 py-2  bg-primary rounded-[6px]  relative group transition duration-200 text-secondary hover:bg-transparent">
            Sign Up
          </div>
        </ModalTrigger>

        <ModalTrigger
          onClick={() => setType("login")}
          className="p-[3px] relative"
        >
          <div className="absolute inset-0 bg-gradient-to-r from-secondary to-orange-500 rounded-lg" />
          <div className="px-6 py-2  bg-primary rounded-[6px] relative group transition duration-200 text-secondary hover:text-primary hover:bg-transparent">
            Login
          </div>
        </ModalTrigger>

        <ModalBody>
          {type === "login" && <Login setModalType={setType} />}
          {type === "signup" && <SignUp setModalType={setType} />}
          {type === "resetPassword" && <ResetPassword setModalType={setType} />}
        </ModalBody>
      </Modal>
    </div>
  );
};

export default AuthModal;
