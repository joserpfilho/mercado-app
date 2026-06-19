import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGroups, useCreateGroup } from "../hooks/useGroups";
import { useGroupStore } from "../store/groupStore";
import { useAuthStore } from "../store/authStore";
import Modal from "../components/Modal";
import { toast } from "../store/toastStore";
import ListCardSkeleton from "../components/ListCardSkeleton";
import Skeleton from "../components/Skeleton";

export default function GroupsPage() {
  const navigate = useNavigate();
  const { data: groups, isLoading } = useGroups();
  const createGroup = useCreateGroup();
  const setActiveGroup = useGroupStore((s) => s.setActiveGroup);
  const logout = useAuthStore((s) => s.logout);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [name, setName] = useState("");

  const handleSelectGroup = (id: string, groupName: string) => {
    setActiveGroup(id, groupName);
    navigate("/");
  };

  const handleCreateGroup = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) return;

    const group = await createGroup.mutateAsync(name);
    setActiveGroup(group.id, group.name);
    toast.success(`Grupo "${group.name}" criado!`);
    setIsModalOpen(false);
    setName("");
    navigate("/");
  };

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 px-4 py-8">
        <div className="mx-auto max-w-sm space-y-2">
          <Skeleton className="h-6 w-32 mb-4" />
          <ListCardSkeleton />
          <ListCardSkeleton />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 px-4 py-8">
      <div className="mx-auto max-w-sm">
        <div className="mb-6 flex items-center justify-between">
          <h1 className="text-xl font-bold text-gray-800">Seus grupos</h1>
          <button
            onClick={logout}
            className="text-sm text-gray-400 hover:text-gray-600"
          >
            Sair
          </button>
        </div>

        <div className="space-y-2">
          {groups?.map((group) => (
            <button
              key={group.id}
              onClick={() => handleSelectGroup(group.id, group.name)}
              className="flex w-full items-center justify-between rounded-xl border border-gray-200 bg-white px-4 py-3 text-left transition hover:border-green-500"
            >
              <span className="font-medium text-gray-800">{group.name}</span>
              <span className="text-gray-400">→</span>
            </button>
          ))}
        </div>

        {groups?.length === 0 && (
          <p className="mt-4 text-center text-sm text-gray-400">
            Você ainda não tem grupos. Crie um para começar!
          </p>
        )}

        <button
          onClick={() => setIsModalOpen(true)}
          className="mt-4 w-full rounded-xl border-2 border-dashed border-gray-300 py-3 text-sm font-medium text-gray-500 hover:border-green-500 hover:text-green-600"
        >
          + Criar novo grupo
        </button>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Novo grupo"
      >
        <form onSubmit={handleCreateGroup} className="space-y-4">
          <input
            type="text"
            autoFocus
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Ex: Família Silva"
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
          />
          <button
            type="submit"
            disabled={createGroup.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {createGroup.isPending ? "Criando..." : "Criar grupo"}
          </button>
        </form>
      </Modal>
    </div>
  );
}
