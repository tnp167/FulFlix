import { ModalContent } from "../ui/animated-modal";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import CustomFormField from "../CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { Eye, EyeOff } from "lucide-react";
import { useState } from "react";
import { Button } from "../ui/button";
import { Link } from "react-router-dom";
import { LoginSchema } from "@/types/login-schema";

const Login = ({
  setModalType,
}: {
  setModalType: React.Dispatch<React.SetStateAction<"signup" | "login" | null>>;
}) => {
  const [type, setType] = useState("password");
  const [icon, setIcon] = useState(<Eye />);

  const handleToggle = () => {
    if (type === "password") {
      setIcon(<EyeOff />);
      setType("text");
    } else {
      setIcon(<Eye />);
      setType("password");
    }
  };

  const handleRegisterClick = () => {
    setModalType("signup");
  };

  const form = useForm<z.infer<typeof LoginSchema>>({
    resolver: zodResolver(LoginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  const passwordErrors = form.formState.errors.password?.message;

  const onSubmit = async (data: z.infer<typeof LoginSchema>) => {
    try {
      console.log("Form submitted:", data);
    } catch (error) {
      console.error("Form submission error:", error);
    }
  };

  return (
    <ModalContent>
      <h1 className="pt-10 pb-16 text-2xl font-semibold uppercase text-center">
        login to fulflix
      </h1>
      <Form {...form}>
        <form className="space-y-8" onSubmit={form.handleSubmit(onSubmit)}>
          <CustomFormField
            fieldType={FormFieldType.INPUT}
            control={form.control}
            name="email"
            label="Email"
            placeholder="johndoe@gmail.com"
          />

          <div className="flex relative">
            <CustomFormField
              fieldType={FormFieldType.INPUT}
              control={form.control}
              name="password"
              label="Password"
              passwordErrors={passwordErrors}
              type={type}
              placeholder="***********"
            />
            <span
              className="flex justify-around items-center absolute right-2 top-10 text-white/70"
              onClick={handleToggle}
            >
              {icon}
            </span>
          </div>

          <Button className="flex justify-end  px-0 text-secondary" asChild>
            <Link to="">Forgot your password?</Link>
          </Button>
          <button
            type="submit"
            className=" inline-flex w-full h-14 animate-shimmer items-center justify-center rounded-md border border-none bg-[linear-gradient(110deg,#5e2a8b,45%,#7d3f8c,55%,#5e2a8b)] bg-[length:200%_100%] px-6 font-medium text-secondary  focus:outline-none transition-all custom-box"
          >
            Enter
          </button>
          <div
            onClick={handleRegisterClick}
            className="mt-10 text-lg text-center text-secondary cursor-pointer border border-transparent hover:underline"
          >
            Not yet a fulflix member?
          </div>
        </form>
      </Form>
    </ModalContent>
  );
};

export default Login;
