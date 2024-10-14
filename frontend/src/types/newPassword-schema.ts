import { z } from "zod";

export const NewPasswordSchema = z
  .object({
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
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords don't match",
    path: ["confirmPassword"],
  });
