import { gql } from "@apollo/client";

export const SIGN_UP = gql`
  mutation SignUp(
    $firstName: String!
    $lastName: String!
    $email: String!
    $password: String!
    $location: String!
    $birthDate: DateTime!
    $phone: String!
    $privacyConsent: Boolean!
  ) {
    createUser(
      createUserDto: {
        firstName: $firstName
        lastName: $lastName
        email: $email
        password: $password
        location: $location
        birthDate: $birthDate
        phone: $phone
        privacyConsent: $privacyConsent
      }
    ) {
      id
      email
      firstName
      lastName
      picture
    }
  }
`;

export const SEND_VERIFICATION_EMAIL = gql`
  mutation SendVerificationEmail($email: String!) {
    sendVerificationEmail(email: $email)
  }
`;

export const LOGIN = gql`
  mutation Login($email: String!, $password: String!) {
    loginUser(loginUserDto: { email: $email, password: $password })
  }
`;
