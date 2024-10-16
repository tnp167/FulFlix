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
import { Toaster } from "sonner";

const Layout = ({ children }: { children: React.ReactNode }) => (
  <>
    <Header />
    {children}
  </>
);

const AppWithApollo = () => {
  store.subscribe(() => {
    if (!store.getState().user.isAuthenticated) {
      clearApolloCache();
    }
  });

  return (
    <Router>
      <Toaster position="bottom-right" richColors toastOptions={{}} />
      <Routes>
        <Route
          path="/"
          element={
            <Layout>
              <Home />
            </Layout>
          }
        />
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
