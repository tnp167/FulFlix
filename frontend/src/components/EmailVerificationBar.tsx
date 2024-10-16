import { useSelector } from "react-redux";
import { RootState } from "@/redux/store";
import { useMutation } from "@apollo/client";
import { SEND_VERIFICATION_EMAIL } from "@/graphql/mutations/authMutation";
import { toast } from "sonner";

const EmailVerificationBar = () => {
  const user = useSelector((state: RootState) => state.user.user);

  const [sendVerificationEmail] = useMutation(SEND_VERIFICATION_EMAIL);

  const handleSendVerificationEmail = async () => {
    const toastId = toast.loading("Sending verification email...");
    try {
      await sendVerificationEmail({
        variables: {
          email: user?.email,
        },
      });
      toast.success("Verification email sent", { id: toastId });
    } catch (error) {
      console.error("Error sending verification email:", error);
      toast.error("Failed to send verification email", { id: toastId });
    }
  };

  if (!user || user.emailVerified) {
    return null;
  }

  return (
    <div className="bg-secondary text-primary py-2 text-center sticky top-0 z-50">
      Please verify your email address. Check your inbox for a verification
      link.
      <span className="block mt-2">
        Haven't received the email yet?
        <button
          type="button"
          className="text-primary font-bold underline ml-1"
          onClick={handleSendVerificationEmail}
        >
          Resend verification email
        </button>
      </span>
    </div>
  );
};

export default EmailVerificationBar;
