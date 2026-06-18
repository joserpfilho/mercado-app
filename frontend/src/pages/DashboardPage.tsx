import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useGroupStore } from "../store/groupStore";
import {
  useShoppingLists,
  useCreateShoppingList,
} from "../hooks/useShoppingLists";
import Header from "../components/Header";
import Modal from "../components/Modal";
import { ListStatus } from "../types";

export default function DashboardPage() {
  const navigate = useNavigate();
  const activeGroupId = useGroupStore((s) => s.activeGroupId);
  const { data: lists, isLoading } = useShoppingLists(
    activeGroupId,
    ListStatus.Active,
  );
  const createList = useCreateShoppingList(activeGroupId);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [name, setName] = useState("");

  if (!activeGroupId) {
    navigate("/groups");
    return null;
  }

  const handleCreateList = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) return;

    const list = await createList.mutateAsync(name);
    setIsModalOpen(false);
    setName("");
    navigate(`/lists/${list.id}`);
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <Header />

      <div className="px-4 py-4">
        <div className="mb-4 flex items-center justify-between">
          <h2 className="font-semibold text-gray-700">Listas ativas</h2>
          <button
            onClick={() => navigate("/history")}
            className="text-sm text-gray-400 hover:text-gray-600"
          >
            Histórico
          </button>
        </div>

        {isLoading && <p className="text-sm text-gray-400">Carregando...</p>}

        <div className="space-y-2">
          {lists?.map((list) => (
            <button
              key={list.id}
              onClick={() => navigate(`/lists/${list.id}`)}
              className="flex w-full items-center justify-between rounded-xl border border-gray-200 bg-white px-4 py-3 text-left transition hover:border-green-500"
            >
              <div>
                <p className="font-medium text-gray-800">{list.name}</p>
                <p className="text-xs text-gray-400">
                  {list.checkedItems}/{list.totalItems} itens
                </p>
              </div>
              <span className="text-gray-400">→</span>
            </button>
          ))}
        </div>

        {lists?.length === 0 && !isLoading && (
          <p className="mt-4 text-center text-sm text-gray-400">
            Nenhuma lista ativa. Crie uma para começar!
          </p>
        )}

        <button
          onClick={() => setIsModalOpen(true)}
          className="mt-4 w-full rounded-xl border-2 border-dashed border-gray-300 py-3 text-sm font-medium text-gray-500 hover:border-green-500 hover:text-green-600"
        >
          + Nova lista
        </button>
      </div>

      <Modal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        title="Nova lista"
      >
        <form onSubmit={handleCreateList} className="space-y-4">
          <input
            type="text"
            autoFocus
            value={name}
            onChange={(e) => setName(e.target.value)}
            placeholder="Ex: Compras da semana"
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none focus:ring-2 focus:ring-green-500/20"
          />
          <button
            type="submit"
            disabled={createList.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {createList.isPending ? "Criando..." : "Criar lista"}
          </button>
        </form>
      </Modal>
    </div>
  );
}
