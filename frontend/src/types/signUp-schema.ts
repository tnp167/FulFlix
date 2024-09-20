import * as z from "zod";

const validateAge = (dateOfBirth: Date): number => {
  const today = new Date();
  const birthDate = new Date(dateOfBirth);
  const age = today.getFullYear() - birthDate.getFullYear();
  const monthDifference = today.getMonth() - birthDate.getMonth();

  if (
    monthDifference < 0 ||
    (monthDifference === 0 && today.getDate() < birthDate.getDate())
  ) {
    return age - 1;
  }
  return age;
};

export const SignUpSchema = z
  .object({
    firstname: z
      .string()
      .min(2, "First name must be at least 2 characters")
      .max(50, "First name must be at most 50 characters"),
    lastname: z
      .string()
      .min(2, "Last name must be at least 2 characters")
      .max(50, "Last name must be at most 50 characters"),
    email: z.string().email("Invalid email address"),
    password: z
      .string()
      .min(8, { message: "Password must be at least 8 characters long" })
      .superRefine((password, ctx) => {
        const issues = [];

        if (password.length < 8) {
          issues.push("Password must be at least 8 characters long.  ");
        }

        if (!/[A-Z]/.test(password)) {
          issues.push("Password must contain at least one uppercase letter.  ");
        }

        if (!/[a-z]/.test(password)) {
          issues.push("Password must contain at least one lowercase letter.  ");
        }

        if (!/[0-9]/.test(password)) {
          issues.push("Password must contain at least one number.  ");
        }

        if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
          issues.push(
            "Password must contain at least one special character.  "
          );
        }

        if (issues.length > 0) {
          ctx.addIssue({
            code: z.ZodIssueCode.custom,
            message: issues.join(" "),
          });
        }
      }),
    confirmPassword: z.string(),
    location: z
      .enum(
        ["Shoreditch", "Paddington", "Manchester", "Birmingham", "Nottingham"],
        {
          errorMap: () => ({ message: "Please choose your fulflix" }),
        }
      )
      .refine((value) => value !== null, {
        message: "Location is required",
        path: ["location"],
      }),
    phone: z
      .string()
      .min(10, { message: "Phone number must be at least 10 digits long" })
      .max(15, { message: "Phone number must not exceed 15 digits" })
      .regex(/^\+?[1-9]\d{1,14}$/, {
        message: "Invalid phone number format. It should include country code.",
      }),

    birthDate: z.coerce.date().refine((date) => validateAge(date) >= 16, {
      message: `You must be at least 16 years old to sign up.`,
    }),
    privacyConsent: z
      .boolean()
      .default(false)
      .refine((value) => value === true, {
        message: "You must consent to privacy in order to proceed",
      }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
  });
