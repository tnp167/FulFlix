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
