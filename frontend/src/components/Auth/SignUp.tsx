import { ModalContent } from "../ui/animated-modal";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import { SignUpSchema } from "@/types/signUp-schema";
import CustomFormField from "../CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { Eye, EyeOff } from "lucide-react";
import { useState } from "react";
import { useMutation } from "@apollo/client";
import { useDispatch } from "react-redux";
import { setUser } from "../../redux/userSlice";
import {
  SIGN_UP,
  SEND_VERIFICATION_EMAIL,
} from "@/graphql/mutations/authMutation";
import success from "@/assets/success.json";
import Lottie from "lottie-react";
import { AnimatePresence, motion } from "framer-motion";
const SignUp = ({
  setModalType,
}: {
  setModalType: React.Dispatch<
    React.SetStateAction<
      "signup" | "login" | "resetPassword" | "newPassword" | null
    >
  >;
}) => {
  const [type, setType] = useState("password");
  const [icon, setIcon] = useState(<Eye />);
  const dispatch = useDispatch();
  const [errorMessage, setErrorMessage] = useState("");
  const [isRegistered, setIsRegistered] = useState(false);

  const handleToggle = () => {
    if (type === "password") {
      setIcon(<EyeOff />);
      setType("text");
    } else {
      setIcon(<Eye />);
      setType("password");
    }
  };

  const handleLoginClick = () => {
    setModalType("login");
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
      phone: "",
      birthDate: new Date(Date.now()),
      privacyConsent: false,
    },
  });

  const [signUp, { loading }] = useMutation(SIGN_UP, {
    onCompleted(data) {
      console.log("User created successfully:", data.createUser);
      setErrorMessage("");
      dispatch(setUser(data.createUser));
      setIsRegistered(true);
    },
    onError(err) {
      console.error("Error signing up:", err);
      setErrorMessage(
        err.graphQLErrors[0]?.message || "An unexpected error occurred"
      );
    },
  });

  const [sendVerificationEmail] = useMutation(SEND_VERIFICATION_EMAIL, {
    onCompleted() {
      console.log("Verification email sent successfully");
      setIsRegistered(true);
    },
    onError(err) {
      console.error("Error sending verification email:", err);
      setErrorMessage(
        "Failed to send verification email. Please try again later."
      );
    },
  });

  const passwordErrors = form.formState.errors.password?.message;

  const onSubmit = async (data: z.infer<typeof SignUpSchema>) => {
    try {
      console.log("Form submitted:", data);
      const signUpResult = await signUp({
        variables: {
          firstName: data.firstname,
          lastName: data.lastname,
          email: data.email,
          password: data.password,
          location: data.location,
          birthDate: data.birthDate,
          phone: data.phone,
          privacyConsent: data.privacyConsent,
        },
      });

      if (signUpResult.data?.createUser) {
        await sendVerificationEmail({
          variables: {
            email: data.email,
          },
        });
      }
    } catch (error) {
      console.error("Form submission error:", error);
      setErrorMessage("An unexpected error occurred. Please try again.");
      setIsRegistered(false);
    }
  };

  return (
    <ModalContent>
      {!isRegistered ? (
        <>
          <h1 className="pt-10 pb-16 text-2xl font-semibold uppercase text-center">
            Become a fulflix member
          </h1>
          <Form {...form}>
            <form className="space-y-8" onSubmit={form.handleSubmit(onSubmit)}>
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

              <div className="flex flex-col md:flex-row gap-2">
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
                className="inline-flex w-full h-14 animate-shimmer items-center justify-center rounded-md border border-none bg-[linear-gradient(110deg,#5e2a8b,45%,#7d3f8c,55%,#5e2a8b)] bg-[length:200%_100%] px-6 font-medium text-secondary  focus:outline-none transition-all custom-box"
                disabled={loading}
              >
                {loading ? "Signing up..." : "Sign up"}
              </button>
              <div
                onClick={handleLoginClick}
                className="mt-10 text-lg text-center text-secondary cursor-pointer border border-transparent hover:underline"
              >
                Already have an account?
              </div>
              {errorMessage && (
                <p className="text-red-500 text-center">{errorMessage}</p>
              )}
            </form>
          </Form>
        </>
      ) : (
        <AnimatePresence>
          <motion.div
            initial={{ opacity: 0 }}
            animate={{ opacity: 1 }}
            transition={{ duration: 0.75 }}
            exit={{ opacity: 0 }}
          >
            <Lottie className="h-48 pt-8" animationData={success} />
            <h1 className="pt-10 pb-10 text-3xl font-normal text-[#7ee857] text-center">
              Registration successful!
            </h1>
            <p className="text-center">
              Please check your email for a verification link
            </p>
            <div className="flex justify-center mt-14">
              <button
                className="px-10 py-3 rounded-full bg-[#5e2a8b] font-semibold text-white tracking-widest uppercase transform hover:scale-105 hover:bg-[#7d3f8c] transition-all duration-200 ease-in-out"
                onClick={handleLoginClick}
              >
                Go to Login
              </button>
            </div>
          </motion.div>
        </AnimatePresence>
      )}
    </ModalContent>
  );
};

export default SignUp;
