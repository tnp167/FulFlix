import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useMutation } from "@apollo/client";
import { VERIFY_EMAIL } from "@/graphql/mutations/authMutation";
import { Vortex } from "@/components/ui/vortex";
import Lottie from "lottie-react";
import success from "@/assets/success.json";
import { AnimatePresence, motion } from "framer-motion";
import { useDispatch, useSelector } from "react-redux";
import { setUser } from "@/redux/userSlice";
import { RootState } from "@/redux/store";

const VerifyEmail = () => {
  const [isVerified, setIsVerified] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const location = useLocation();
  const navigate = useNavigate();
  const [countdown, setCountdown] = useState(5);

  const dispatch = useDispatch();
  const currentUser = useSelector((state: RootState) => state.user.user);

  const [verifyEmail] = useMutation(VERIFY_EMAIL, {
    onCompleted() {
      setIsVerified(true);
      if (currentUser) {
        dispatch(setUser({ ...currentUser, emailVerified: true }));
      }
    },
    onError(error) {
      setErrorMessage(error.message);
    },
  });

  useEffect(() => {
    const searchParams = new URLSearchParams(location.search);
    const token = searchParams.get("token");
    if (token) {
      verifyEmail({ variables: { token } });
    } else {
      setErrorMessage("Invalid verification link");
    }
  }, [location, verifyEmail]);

  useEffect(() => {
    if (isVerified && countdown > 0) {
      const timer = setTimeout(() => setCountdown(countdown - 1), 1000);
      return () => clearTimeout(timer);
    } else if (isVerified && countdown === 0) {
      navigate("/");
    }
  }, [isVerified, countdown, navigate]);

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
          <div className="bg-primary p-10 rounded-lg shadow-md w-full max-w-md text-center">
            {isVerified ? (
              <AnimatePresence>
                <motion.div
                  initial={{ opacity: 0 }}
                  animate={{ opacity: 1 }}
                  transition={{ duration: 0.75 }}
                  exit={{ opacity: 0 }}
                >
                  <Lottie className="h-48 pt-8" animationData={success} />
                  <h1 className="pt-10 pb-10 text-3xl font-normal text-[#7ee857]">
                    Email Verified Successfully!
                  </h1>
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
            ) : (
              <div>
                <h1 className="text-2xl font-semibold text-secondary mb-4">
                  Verifying Email
                </h1>
                {errorMessage ? (
                  <p className="text-red-500">{errorMessage}</p>
                ) : (
                  <p className="text-secondary">Please wait...</p>
                )}
              </div>
            )}
          </div>
        </Vortex>
      </div>
    </>
  );
};

export default VerifyEmail;
