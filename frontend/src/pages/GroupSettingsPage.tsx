import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGroupStore } from "../store/groupStore";
import { useGroupMembers, useAddMember } from "../hooks/useGroupMembers";
import { useGroups } from "../hooks/useGroups";
import { useAuthStore } from "../store/authStore";
import { toast } from "../store/toastStore";
import { isAxiosError } from "axios";
import Modal from "../components/Modal";

export default function GroupSettingsPage() {
  const navigate = useNavigate();
  const activeGroupId = useGroupStore((s) => s.activeGroupId);
  const activeGroupName = useGroupStore((s) => s.activeGroupName);
  const setActiveGroup = useGroupStore((s) => s.setActiveGroup);
  const logout = useAuthStore((s) => s.logout);

  const { data: members, isLoading: loadingMembers } =
    useGroupMembers(activeGroupId);
  const { data: groups } = useGroups();
  const addMember = useAddMember(activeGroupId);

  const [isAddMemberOpen, setIsAddMemberOpen] = useState(false);
  const [isSwitchGroupOpen, setIsSwitchGroupOpen] = useState(false);
  const [email, setEmail] = useState("");

  const handleAddMember = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!email.trim()) return;

    try {
      await addMember.mutateAsync(email);
      toast.success("Membro adicionado!");
      setIsAddMemberOpen(false);
      setEmail("");
    } catch (err) {
      if (isAxiosError(err) && err.response?.data?.error) {
        toast.error(err.response.data.error);
      } else {
        toast.error("Erro ao adicionar membro.");
      }
    }
  };

  const handleSwitchGroup = (id: string, name: string) => {
    setActiveGroup(id, name);
    toast.success(`Grupo "${name}" selecionado!`);
    setIsSwitchGroupOpen(false);
    navigate("/");
  };

  const handleLogout = () => {
    logout();
    navigate("/login");
  };

  return (
    <div className="min-h-screen bg-gray-50 pb-8">
      {/* Header */}
      <header className="sticky top-0 z-10 border-b border-gray-200 bg-white px-4 py-3">
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate("/")}
            className="text-gray-400 hover:text-gray-600"
          >
            ← Voltar
          </button>
          <h1 className="font-semibold text-gray-800">Configurações</h1>
        </div>
      </header>

      <div className="px-4 py-4 space-y-5">
        {/* Grupo ativo */}
        <div className="rounded-xl border border-gray-200 bg-white p-4">
          <p className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-1">
            Grupo ativo
          </p>
          <p className="font-semibold text-gray-800 text-lg">
            {activeGroupName}
          </p>
          <button
            onClick={() => setIsSwitchGroupOpen(true)}
            className="mt-2 text-sm text-green-600 hover:underline"
          >
            Trocar de grupo
          </button>
        </div>

        {/* Membros */}
        <div className="rounded-xl border border-gray-200 bg-white p-4">
          <div className="flex items-center justify-between mb-3">
            <p className="text-xs font-semibold uppercase tracking-wide text-gray-400">
              Membros
            </p>
            <button
              onClick={() => setIsAddMemberOpen(true)}
              className="text-sm font-medium text-green-600 hover:underline"
            >
              + Adicionar
            </button>
          </div>

          {loadingMembers && (
            <p className="text-sm text-gray-400">Carregando...</p>
          )}

          <div className="space-y-2">
            {members?.map(
              (member: {
                userId: string;
                name: string;
                email: string;
                role: string;
              }) => (
                <div
                  key={member.userId}
                  className="flex items-center justify-between"
                >
                  <div>
                    <p className="text-sm font-medium text-gray-800">
                      {member.name}
                    </p>
                    <p className="text-xs text-gray-400">{member.email}</p>
                  </div>
                  <span
                    className={`rounded-full px-2 py-0.5 text-xs font-medium ${
                      member.role === "Owner"
                        ? "bg-green-100 text-green-700"
                        : "bg-gray-100 text-gray-500"
                    }`}
                  >
                    {member.role === "Owner" ? "Dono" : "Membro"}
                  </span>
                </div>
              ),
            )}
          </div>
        </div>

        {/* Conta */}
        <div className="rounded-xl border border-gray-200 bg-white p-4">
          <p className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-3">
            Conta
          </p>
          <button
            onClick={handleLogout}
            className="w-full rounded-lg border border-red-200 py-2.5 text-sm font-medium text-red-500 hover:bg-red-50"
          >
            Sair da conta
          </button>
        </div>
      </div>

      {/* Modal: adicionar membro */}
      <Modal
        isOpen={isAddMemberOpen}
        onClose={() => setIsAddMemberOpen(false)}
        title="Adicionar membro"
      >
        <form onSubmit={handleAddMember} className="space-y-4">
          <input
            type="email"
            autoFocus
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            placeholder="email@exemplo.com"
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
          />
          <button
            type="submit"
            disabled={addMember.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {addMember.isPending ? "Adicionando..." : "Adicionar"}
          </button>
        </form>
      </Modal>

      {/* Modal: trocar de grupo */}
      <Modal
        isOpen={isSwitchGroupOpen}
        onClose={() => setIsSwitchGroupOpen(false)}
        title="Trocar de grupo"
      >
        <div className="space-y-2">
          {groups?.map((group) => (
            <button
              key={group.id}
              onClick={() => handleSwitchGroup(group.id, group.name)}
              className={`flex w-full items-center justify-between rounded-xl border px-4 py-3 text-left transition ${
                group.id === activeGroupId
                  ? "border-green-500 bg-green-50"
                  : "border-gray-200 hover:border-green-300"
              }`}
            >
              <span className="font-medium text-gray-800">{group.name}</span>
              {group.id === activeGroupId && (
                <span className="text-xs text-green-600 font-medium">
                  Ativo
                </span>
              )}
            </button>
          ))}
          <button
            onClick={() => {
              setIsSwitchGroupOpen(false);
              navigate("/groups");
            }}
            className="w-full rounded-xl border-2 border-dashed border-gray-300 py-3 text-sm font-medium text-gray-500 hover:border-green-500 hover:text-green-600"
          >
            + Criar novo grupo
          </button>
        </div>
      </Modal>
    </div>
  );
}
