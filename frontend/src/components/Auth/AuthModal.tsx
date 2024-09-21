import React, { useState } from "react";
import {
  Modal,
  ModalBody,
  ModalFooter,
  ModalTrigger,
} from "@/components/ui/animated-modal";
import Socials from "./Social";
import Login from "./Login";
import SignUp from "./SignUp";

const AuthModal = () => {
  const [type, setType] = useState<"signup" | "login" | null>(null);
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
          onClick={() => setType("signup")}
          className="p-[3px] relative"
        >
          <div className="absolute inset-0 bg-gradient-to-r from-secondary to-orange-500 rounded-lg" />
          <div className="px-6 py-2  bg-primary rounded-[6px] relative group transition duration-200 text-secondary hover:text-primary hover:bg-transparent">
            Login
          </div>
        </ModalTrigger>

        <ModalBody>
          {type && type === "login" && <Login setModalType={setType} />}
          {type && type === "signup" && <SignUp setModalType={setType} />}
          <ModalFooter>
            <Socials />
          </ModalFooter>
        </ModalBody>
      </Modal>
    </div>
  );
};

export default AuthModal;
