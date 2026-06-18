import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { authApi } from "../api/auth";
import { useAuthStore } from "../store/authStore";
import { isAxiosError } from "axios";
import { toast } from "../store/toastStore";

export default function RegisterPage() {
  const navigate = useNavigate();
  const setAuth = useAuthStore((s) => s.setAuth);

  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const data = await authApi.register(name, email, password);
      setAuth(data.token, { name: data.name, email: data.email });
      toast.success("Conta criada com sucesso!");
      navigate("/");
    } catch (err) {
      if (isAxiosError(err) && err.response?.data?.error) {
        setError(err.response.data.error);
      } else {
        setError("Erro ao criar conta.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-sm">
        <div className="mb-8 text-center">
          <div className="mb-2 text-5xl">🛒</div>
          <h1 className="text-2xl font-bold text-gray-800">Criar conta</h1>
          <p className="mt-1 text-sm text-gray-500">
            Comece a organizar suas compras
          </p>
        </div>

        <form onSubmit={handleSubmit} className="space-y-4">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Nome
            </label>
            <input
              type="text"
              required
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 text-gray-800 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
              placeholder="Seu nome"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              E-mail
            </label>
            <input
              type="email"
              required
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 text-gray-800 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
              placeholder="seu@email.com"
            />
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Senha
            </label>
            <input
              type="password"
              required
              minLength={6}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 text-gray-800 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
              placeholder="Mínimo 6 caracteres"
            />
          </div>

          {error && (
            <p className="rounded-lg bg-red-50 px-3 py-2 text-sm text-red-600">
              {error}
            </p>
          )}

          <button
            type="submit"
            disabled={loading}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white transition hover:bg-green-700 disabled:opacity-50"
          >
            {loading ? "Criando..." : "Criar conta"}
          </button>
        </form>

        <p className="mt-6 text-center text-sm text-gray-500">
          Já tem uma conta?{" "}
          <Link
            to="/login"
            className="font-medium text-green-600 hover:underline"
          >
            Entrar
          </Link>
        </p>
      </div>
    </div>
  );
}
