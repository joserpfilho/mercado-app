import { useToastStore } from "../store/toastStore";

export default function ToastContainer() {
  const { toasts, removeToast } = useToastStore();

  if (toasts.length === 0) return null;

  return (
    <div className="fixed bottom-24 left-0 right-0 z-50 flex flex-col items-center gap-2 px-4">
      {toasts.map((toast) => (
        <button
          key={toast.id}
          onClick={() => removeToast(toast.id)}
          className={`flex w-full max-w-sm items-center gap-3 rounded-xl px-4 py-3 text-left text-sm font-medium text-white shadow-lg transition-all ${
            toast.type === "success"
              ? "bg-green-600"
              : toast.type === "error"
                ? "bg-red-500"
                : "bg-gray-700"
          }`}
        >
          <span className="text-base">
            {toast.type === "success"
              ? "✅"
              : toast.type === "error"
                ? "❌"
                : "ℹ️"}
          </span>
          <span className="flex-1">{toast.message}</span>
        </button>
      ))}
    </div>
  );
}
