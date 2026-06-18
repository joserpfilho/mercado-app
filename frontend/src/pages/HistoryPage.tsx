import { useNavigate } from "react-router-dom";
import { useGroupStore } from "../store/groupStore";
import { useShoppingLists } from "../hooks/useShoppingLists";
import { ListStatus } from "../types";

export default function HistoryPage() {
  const navigate = useNavigate();
  const activeGroupId = useGroupStore((s) => s.activeGroupId);
  const { data: lists, isLoading } = useShoppingLists(
    activeGroupId,
    ListStatus.Archived,
  );

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="sticky top-0 z-10 border-b border-gray-200 bg-white px-4 py-3">
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate("/")}
            className="text-gray-400 hover:text-gray-600"
          >
            ← Voltar
          </button>
          <h1 className="font-semibold text-gray-800">Histórico</h1>
        </div>
      </header>

      <div className="px-4 py-4 space-y-2">
        {isLoading && <p className="text-sm text-gray-400">Carregando...</p>}

        {lists?.map((list) => (
          <button
            key={list.id}
            onClick={() => navigate(`/lists/${list.id}`)}
            className="flex w-full items-center justify-between rounded-xl border border-gray-200 bg-white px-4 py-3 text-left"
          >
            <div>
              <p className="font-medium text-gray-800">{list.name}</p>
              <p className="text-xs text-gray-400">
                {new Date(list.createdAt).toLocaleDateString("pt-BR")} •{" "}
                {list.checkedItems}/{list.totalItems} itens
              </p>
            </div>
            <span className="rounded-full bg-gray-100 px-2 py-0.5 text-xs text-gray-500">
              Arquivada
            </span>
          </button>
        ))}

        {lists?.length === 0 && !isLoading && (
          <p className="mt-8 text-center text-sm text-gray-400">
            Nenhuma lista arquivada ainda.
          </p>
        )}
      </div>
    </div>
  );
}
