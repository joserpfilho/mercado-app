import { Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import GroupsPage from "./pages/GroupsPage";
import DashboardPage from "./pages/DashboardPage";
import ListPage from "./pages/ListPage";
import HistoryPage from "./pages/HistoryPage";
import ProtectedRoute from "./components/ProtectedRoute";
import ToastContainer from "./components/ToastContainer";
import GroupSettingsPage from "./pages/GroupSettingsPage";

function App() {
  return (
    <>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />

        <Route element={<ProtectedRoute />}>
          <Route path="/groups" element={<GroupsPage />} />
          <Route path="/" element={<DashboardPage />} />
          <Route path="/lists/:id" element={<ListPage />} />
          <Route path="/history" element={<HistoryPage />} />
          <Route path="/settings" element={<GroupSettingsPage />} />
        </Route>
      </Routes>
      <ToastContainer />
    </>
  );
}

export default App;
