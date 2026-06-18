import { useNavigate } from "react-router-dom";
import { useAuthStore } from "../store/authStore";
import { useGroupStore } from "../store/groupStore";

export default function Header() {
  const navigate = useNavigate();
  const user = useAuthStore((s) => s.user);
  const activeGroupName = useGroupStore((s) => s.activeGroupName);

  return (
    <header className="sticky top-0 z-10 border-b border-gray-200 bg-white px-4 py-3">
      <div className="flex items-center justify-between">
        <div>
          <p className="text-xs text-gray-400">Olá, {user?.name}</p>
          <h1 className="font-semibold text-gray-800">
            {activeGroupName ?? "MercadoApp 🛒"}
          </h1>
        </div>
        <button
          onClick={() => navigate("/settings")}
          className="rounded-lg p-2 text-gray-400 hover:bg-gray-100 hover:text-gray-600"
        >
          ⚙️
        </button>
      </div>
    </header>
  );
}
