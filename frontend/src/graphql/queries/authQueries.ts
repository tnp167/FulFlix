import { gql } from "@apollo/client";

export const GET_USER_PROFILE = gql`
  query GetUserProfile {
    user {
      id
      email
      firstName
      lastName
      picture
      emailVerified
      roles
      location
      birthDate
      phone
      privacyConsent
    }
  }
`;
