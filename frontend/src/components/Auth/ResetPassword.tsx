import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import CustomFormField from "../CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { useMutation } from "@apollo/client";
import { SEND_RESET_PASSWORD } from "@/graphql/mutations/authMutation";
import { ResetPasswordSchema } from "@/types/resetPassword-schema";
import { ModalContent } from "@/components/ui/animated-modal";
import { ArrowLeft, CheckCircle2 } from "lucide-react";
import { AnimatePresence, motion } from "framer-motion";
const ResetPassword = ({
  setModalType,
}: {
  setModalType: React.Dispatch<
    React.SetStateAction<
      "signup" | "login" | "resetPassword" | "newPassword" | null
    >
  >;
}) => {
  const [isEmailSent, setIsEmailSent] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");

  const form = useForm<z.infer<typeof ResetPasswordSchema>>({
    resolver: zodResolver(ResetPasswordSchema),
    defaultValues: {
      email: "",
    },
  });

  const [sendResetPassword, { loading }] = useMutation(SEND_RESET_PASSWORD, {
    onCompleted() {
      setIsEmailSent(true);
    },
    onError(error) {
      setErrorMessage(error.message);
    },
  });

  const onSubmit = (data: z.infer<typeof ResetPasswordSchema>) => {
    try {
      sendResetPassword({ variables: data });
    } catch (error) {
      console.error("Error sending reset password email:", error);
    }
  };

  return (
    <ModalContent>
      <h1 className="text-2xl uppercase mx-auto font-semibold mb-10">
        Reset Password
      </h1>

      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
          <CustomFormField
            fieldType={FormFieldType.INPUT}
            control={form.control}
            name="email"
            label="Email"
            placeholder="Enter your email"
          />
          <button
            type="submit"
            className="inline-flex w-full h-14 animate-shimmer items-center justify-center rounded-md border border-none bg-[linear-gradient(110deg,#5e2a8b,45%,#7d3f8c,55%,#5e2a8b)] bg-[length:200%_100%] px-6 font-medium text-secondary  focus:outline-none transition-all custom-box"
            disabled={loading}
          >
            {loading ? "Sending..." : "Send Reset Link"}
          </button>
          {errorMessage && (
            <p className="text-red-500 text-center">{errorMessage}</p>
          )}
          {isEmailSent && (
            <AnimatePresence>
              <motion.div
                className="bg-green-400 border-0 flex text-xs font-medium items-center my-4 gap-2 text-secondary-foreground p-3"
                initial={{ opacity: 0 }}
                animate={{ opacity: 1 }}
                transition={{ duration: 0.5 }}
              >
                <CheckCircle2 className="w-4 h-4" />
                <p>
                  An email has been sent to {form.getValues("email")}. Check the
                  inbox to reset password
                </p>
              </motion.div>
            </AnimatePresence>
          )}
        </form>
      </Form>
      <button
        onClick={() => setModalType("login")}
        className="mt-4 text-secondary"
      >
        <div className="flex text-base mt-5 gap-3 justify-center items-center">
          <ArrowLeft /> Back to Login
        </div>
      </button>
    </ModalContent>
  );
};

export default ResetPassword;
