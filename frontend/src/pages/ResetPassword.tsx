import { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import CustomFormField from "@/components/CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { NewPasswordSchema } from "@/types/newPassword-schema";
import { useMutation } from "@apollo/client";
import { RESET_PASSWORD } from "@/graphql/mutations/authMutation";
import { Eye, EyeOff } from "lucide-react";
import { useLocation, useNavigate } from "react-router-dom";
import { Vortex } from "@/components/ui/vortex";
import { AnimatePresence, motion } from "framer-motion";
import Lottie from "lottie-react";
import success from "@/assets/success.json";

const ResetPassword = () => {
  const [errorMessage, setErrorMessage] = useState("");
  const [type, setType] = useState<"text" | "password">("password");
  const [icon, setIcon] = useState(<Eye />);
  const [isReset, setIsReset] = useState(false);
  const location = useLocation();
  const navigate = useNavigate();
  const [token, setToken] = useState("");
  const [countdown, setCountdown] = useState(5);

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const urlToken = searchParams.get("token");
    if (urlToken) {
      setToken(urlToken);
    } else {
      navigate("/");
    }
  }, [location, navigate]);

  useEffect(() => {
    if (isReset && countdown > 0) {
      const timer = setTimeout(() => setCountdown(countdown - 1), 1000);
      return () => clearTimeout(timer);
    } else if (isReset && countdown === 0) {
      navigate("/");
    }
  }, [isReset, countdown, navigate]);

  const handleToggle = () => {
    if (type === "password") {
      setIcon(<EyeOff />);
      setType("text");
    } else {
      setIcon(<Eye />);
      setType("password");
    }
  };

  const form = useForm<z.infer<typeof NewPasswordSchema>>({
    resolver: zodResolver(NewPasswordSchema),
    defaultValues: {
      password: "",
      confirmPassword: "",
    },
  });

  const [resetPassword, { loading }] = useMutation(RESET_PASSWORD, {
    onCompleted() {
      console.log("Reset Password successfully");
      setIsReset(true);
    },
    onError(error) {
      setErrorMessage(error.message);
    },
  });

  const onSubmit = async (data: z.infer<typeof NewPasswordSchema>) => {
    try {
      await resetPassword({
        variables: {
          token,
          newPassword: data.password,
        },
      });
    } catch (error) {
      console.error(error);
      setErrorMessage("Failed to reset password");
    }
  };

  const passwordErrors = form.formState.errors.password?.message;

  const handleGoToHomepage = () => {
    navigate("/");
  };

  return (
    <>
      <div className="w-screen h-screen overflow-hidden">
        <Vortex
          backgroundColor="black"
          rangeY={800}
          particleCount={500}
          className="flex items-center justify-center w-full h-full"
        >
          <div className="bg-primary p-10 rounded-lg shadow-md w-full max-w-md ">
            {!isReset ? (
              <>
                <h1 className="text-2xl font-semibold text-secondary text-center mb-12">
                  Set New Password
                </h1>
                <Form {...form}>
                  <form onSubmit={form.handleSubmit(onSubmit)}>
                    <div className="space-y-10">
                      <div className="relative">
                        <CustomFormField
                          fieldType={FormFieldType.INPUT}
                          control={form.control}
                          name="password"
                          label="New Password"
                          passwordErrors={passwordErrors}
                          type={type}
                          placeholder="Enter new password"
                        />
                        <span
                          className="absolute right-3 top-[38px] text-white/70 cursor-pointer"
                          onClick={handleToggle}
                        >
                          {icon}
                        </span>
                      </div>
                      <CustomFormField
                        fieldType={FormFieldType.INPUT}
                        control={form.control}
                        name="confirmPassword"
                        label="Confirm New Password"
                        type="password"
                        placeholder="Confirm new password"
                      />
                    </div>
                    <div className="mt-12">
                      <button
                        type="submit"
                        className="w-full inline-flex h-14 animate-shimmer items-center justify-center rounded-md border border-none bg-[linear-gradient(110deg,#5e2a8b,45%,#7d3f8c,55%,#5e2a8b)] bg-[length:200%_100%] px-6 font-medium text-secondary focus:outline-none transition-all custom-box"
                        disabled={loading}
                      >
                        {loading ? "Resetting..." : "Reset Password"}
                      </button>
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
                    Reset password successful!
                  </h1>
                  <p className="text-lg text-center text-secondary">
                    You can now log in with your new password
                  </p>
                  <div className="flex flex-col items-center mt-14 space-y-4">
                    <button
                      onClick={handleGoToHomepage}
                      className="px-10 py-3 mb-6 rounded-full bg-[#5e2a8b] font-semibold text-white tracking-widest uppercase transform hover:scale-105 hover:bg-[#7d3f8c] transition-all duration-200 ease-in-out"
                    >
                      Go to Homepage
                    </button>
                    <p className="text-secondary">
                      Redirecting in {countdown} seconds...
                    </p>
                  </div>
                </motion.div>
              </AnimatePresence>
            )}
          </div>
        </Vortex>
      </div>
    </>
  );
};

export default ResetPassword;
