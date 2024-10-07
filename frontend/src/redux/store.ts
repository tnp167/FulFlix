import { configureStore } from "@reduxjs/toolkit";
import userReducer from "./userSlice";

export const store = configureStore({ reducer: { counter: userReducer } });

export default store;
