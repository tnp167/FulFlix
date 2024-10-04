import {
  Modal,
  ModalBody,
  ModalContent,
  ModalTrigger,
} from "../ui/animated-modal";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import { SignUpSchema } from "@/types/signUp-schema";
import CustomFormField from "../CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { Eye, EyeOff } from "lucide-react";
import { useState } from "react";

const SignUp = () => {
  const [type, setType] = useState("password");
  const [icon, setIcon] = useState(<EyeOff />);

  const handleToggle = () => {
    if (type === "password") {
      setIcon(<Eye />);
      setType("text");
    } else {
      setIcon(<EyeOff />);
      setType("password");
    }
  };

  const form = useForm<z.infer<typeof SignUpSchema>>({
    resolver: zodResolver(SignUpSchema),
    defaultValues: {
      firstname: "",
      lastname: "",
      email: "",
      password: "",
      confirmPassword: "",
      location: undefined,
      birthDate: new Date(Date.now()),
      privacyConsent: false,
    },
  });

  const passwordErrors = form.formState.errors.password?.message;

  const onSubmit = async (data: z.infer<typeof SignUpSchema>) => {
    try {
      console.log("Form submitted:", data);
    } catch (error) {
      console.error("Form submission error:", error);
    }
  };

  return (
    <div className="bg-primary gap-[50px] text-lg font-medium  transition-all  flex items-center justify-center">
      <Modal>
        <ModalTrigger className="py-0">
          <span className="text-secondary hover:text-accent cursor-pointer  transition-all duration-500">
            Sign up
          </span>
        </ModalTrigger>
        <ModalBody>
          <ModalContent>
            <h1 className="pt-10 pb-16 text-2xl font-semibold uppercase text-center">
              Become a fulflix member
            </h1>
            <Form {...form}>
              <form
                className="space-y-8"
                onSubmit={form.handleSubmit(onSubmit)}
              >
                <div className="flex flex-row gap-10">
                  <CustomFormField
                    fieldType={FormFieldType.INPUT}
                    control={form.control}
                    name="firstname"
                    label="First name"
                    placeholder="John"
                  />
                  <CustomFormField
                    fieldType={FormFieldType.INPUT}
                    control={form.control}
                    name="lastname"
                    label="Last name"
                    placeholder="Doe"
                  />
                </div>

                <CustomFormField
                  fieldType={FormFieldType.REACTSELECT}
                  control={form.control}
                  name="location"
                  label="Primary Fulflix"
                  placeholder="Select a primary fulflix"
                />

                <div className="flex flex-col mdflex-row gap-2">
                  <CustomFormField
                    fieldType={FormFieldType.DATE_PICKER}
                    control={form.control}
                    name="birthDate"
                    label="Date of Birth"
                    dateFormat="dd/mm/yyyy"
                  />
                  <CustomFormField
                    fieldType={FormFieldType.PHONE_INPUT}
                    control={form.control}
                    name="phone"
                    label="Phone"
                    placeholder="(44) 123-4567"
                  />
                </div>

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

                <CustomFormField
                  fieldType={FormFieldType.INPUT}
                  control={form.control}
                  name="confirmPassword"
                  label="Confirm Password"
                  type="password"
                  placeholder="***********"
                />

                <CustomFormField
                  fieldType={FormFieldType.CHECKBOX}
                  control={form.control}
                  name="privacyConsent"
                  label="I agree to FulFlix's terms & conditions and privacy policy"
                />

                <button
                  type="submit"
                  className=" inline-flex w-full h-12 animate-shimmer items-center justify-center rounded-md border border-none bg-[linear-gradient(110deg,#5e2a8b,45%,#7d3f8c,55%,#5e2a8b)] bg-[length:200%_100%] px-6 font-medium text-secondary  focus:outline-none transition-all custom-box"
                >
                  Sign up
                </button>
              </form>
            </Form>
          </ModalContent>
        </ModalBody>
      </Modal>
    </div>
  );
};

export default SignUp;
