import "./App.css";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Header from "./components/Header/Header";
import Home from "./pages/Home";
import { Provider } from "react-redux";
import { ApolloProvider } from "@apollo/client";
import client, { clearApolloCache } from "./apolloClient";
import store from "./redux/store";
import VerifyEmail from "./pages/VerifyEmail";
import ResetPassword from "./pages/ResetPassword";

const AppWithApollo = () => {
  store.subscribe(() => {
    if (!store.getState().user.isAuthenticated) {
      clearApolloCache();
    }
  });

  return (
    <Router>
      <Header />
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/reset-password" element={<ResetPassword />} />
        <Route path="/verify-email" element={<VerifyEmail />} />
      </Routes>
    </Router>
  );
};

function App() {
  return (
    <Provider store={store}>
      <ApolloProvider client={client}>
        <AppWithApollo />
      </ApolloProvider>
    </Provider>
  );
}

export default App;
