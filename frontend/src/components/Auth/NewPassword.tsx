import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { Form } from "@/components/ui/form";
import CustomFormField from "../CustomFormfield";
import { FormFieldType } from "@/types/formFieldType";
import { NewPasswordSchema } from "@/types/newPassword-schema";
import { useMutation } from "@apollo/client";
import { RESET_PASSWORD } from "@/graphql/mutations/authMutation";
import { Eye, EyeOff } from "lucide-react";
import { ModalContent } from "@/components/ui/animated-modal";

const NewPassword = ({
  setModalType,
}: {
  setModalType: React.Dispatch<
    React.SetStateAction<
      "signup" | "login" | "resetPassword" | "newPassword" | null
    >
  >;
}) => {
  const [errorMessage, setErrorMessage] = useState("");
  const [type, setType] = useState<"text" | "password">("password");
  const [icon, setIcon] = useState(<Eye />);

  const token = new URLSearchParams(location.search).get("token");

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
      setModalType("login");
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

  return (
    <ModalContent>
      <h1 className="text-2xl font-semibold mb-6">Set New Password</h1>
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-6">
          <div className="flex relative">
            <CustomFormField
              fieldType={FormFieldType.INPUT}
              control={form.control}
              name="password"
              label="New Password"
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
            label="Confirm New Password"
            type="password"
            placeholder="***********"
          />
          <button
            type="submit"
            className="w-full bg-purple-600 text-white py-2 rounded-md hover:bg-purple-700 transition-colors"
            disabled={loading}
          >
            {loading ? "Resetting..." : "Reset Password"}
          </button>
          {errorMessage && (
            <p className="text-red-500 text-center">{errorMessage}</p>
          )}
        </form>
      </Form>
    </ModalContent>
  );
};

export default NewPassword;
