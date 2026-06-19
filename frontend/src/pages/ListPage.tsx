import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useGroupStore } from "../store/groupStore";
import {
  useShoppingList,
  useUpdateListItem,
  useAddListItem,
  useArchiveList,
} from "../hooks/useShoppingList";
import {
  useDepartments,
  useItems,
  useCreateItem,
  useCreateDepartment,
} from "../hooks/useDepartments";
import { ItemUnit, ItemUnitLabel } from "../types";
import type { ListItemResponse } from "../types";
import Modal from "../components/Modal";
import { toast } from "../store/toastStore";
import ConfirmModal from "../components/ConfirmModal";
import ShoppingItemSkeleton from "../components/ShoppingItemSkeleton";
import Skeleton from "../components/Skeleton";

export default function ListPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const activeGroupId = useGroupStore((s) => s.activeGroupId);

  const { data: list, isLoading } = useShoppingList(id!);
  const { data: departments } = useDepartments(activeGroupId);
  const { data: items } = useItems(activeGroupId);

  const updateItem = useUpdateListItem(id!);
  const addItem = useAddListItem(id!);
  const archiveList = useArchiveList();
  const createItem = useCreateItem(activeGroupId);
  const createDepartment = useCreateDepartment(activeGroupId);

  const [isAddItemOpen, setIsAddItemOpen] = useState(false);
  const [isNewItemOpen, setIsNewItemOpen] = useState(false);
  const [isNewDeptOpen, setIsNewDeptOpen] = useState(false);

  const [selectedItemId, setSelectedItemId] = useState("");
  const [selectedDeptId, setSelectedDeptId] = useState("");
  const [quantity, setQuantity] = useState("1");

  const [newItemName, setNewItemName] = useState("");
  const [newItemUnit, setNewItemUnit] = useState<ItemUnit>(ItemUnit.Unidade);

  const [newDeptName, setNewDeptName] = useState("");
  const [newDeptIcon, setNewDeptIcon] = useState("🛍️");

  const [isArchiveOpen, setIsArchiveOpen] = useState(false);

  const handleToggleCheck = (item: ListItemResponse) => {
    updateItem.mutate({ listItemId: item.id, isChecked: !item.isChecked });
  };

  const handleAddItem = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedItemId || !selectedDeptId) return;

    await addItem.mutateAsync({
      itemId: selectedItemId,
      departmentId: selectedDeptId,
      quantity: parseFloat(quantity),
    });

    toast.success("Item adicionado!");

    setIsAddItemOpen(false);
    setSelectedItemId("");
    setSelectedDeptId("");
    setQuantity("1");
  };

  const handleCreateItem = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newItemName.trim()) return;

    const item = await createItem.mutateAsync({
      name: newItemName,
      unit: newItemUnit,
    });

    toast.success(`Item "${newItemName}" criado!`);

    setSelectedItemId(item.id);
    setIsNewItemOpen(false);
    setNewItemName("");
    setIsAddItemOpen(true);
  };

  const handleCreateDept = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newDeptName.trim()) return;

    const dept = await createDepartment.mutateAsync({
      name: newDeptName,
      icon: newDeptIcon,
    });

    toast.success(`Departamento "${newDeptName}" criado!`);

    setSelectedDeptId(dept.id);
    setIsNewDeptOpen(false);
    setNewDeptName("");
    setIsAddItemOpen(true);
  };

  const handleArchive = async () => {
    if (!confirm("Arquivar esta lista?")) return;
    await archiveList.mutateAsync(id!);
    toast.success("Lista arquivada!");
    navigate("/");
  };

  const checkedCount = (list?.items ?? []).filter(
    (i: ListItemResponse) => i.isChecked,
  ).length;
  const totalCount = list?.items.length ?? 0;
  const progress = totalCount > 0 ? (checkedCount / totalCount) * 100 : 0;

  const itemsByDepartment = (list?.items ?? []).reduce(
    (acc: Record<string, ListItemResponse[]>, item: ListItemResponse) => {
      const key = item.departmentId;
      if (!acc[key]) acc[key] = [];
      acc[key].push(item);
      return acc;
    },
    {} as Record<string, ListItemResponse[]>,
  );

  if (isLoading) {
    return (
      <div className="min-h-screen bg-gray-50 pb-24">
        <header className="sticky top-0 z-10 border-b border-gray-200 bg-white px-4 py-3">
          <Skeleton className="h-5 w-40 mx-auto" />
        </header>
        <div className="px-4 py-4 space-y-3">
          <Skeleton className="h-3 w-20" />
          <ShoppingItemSkeleton />
          <ShoppingItemSkeleton />
          <ShoppingItemSkeleton />
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 pb-24">
      {/* Header */}
      <header className="sticky top-0 z-10 border-b border-gray-200 bg-white px-4 py-3">
        <div className="flex items-center justify-between">
          <button
            onClick={() => navigate("/")}
            className="text-gray-400 hover:text-gray-600"
          >
            ← Voltar
          </button>
          <h1 className="font-semibold text-gray-800">{list?.name}</h1>
          <button
            onClick={() => setIsArchiveOpen(true)}
            className="text-sm text-gray-400 hover:text-red-500"
          >
            Arquivar
          </button>
        </div>

        {/* Barra de progresso */}
        <div className="mt-2">
          <div className="flex justify-between text-xs text-gray-400 mb-1">
            <span>
              {checkedCount}/{totalCount} itens
            </span>
            <span>{Math.round(progress)}%</span>
          </div>
          <div className="h-1.5 w-full rounded-full bg-gray-200">
            <div
              className="h-1.5 rounded-full bg-green-500 transition-all"
              style={{ width: `${progress}%` }}
            />
          </div>
        </div>
      </header>

      {/* Itens agrupados por departamento */}
      <div className="px-4 py-4 space-y-5">
        {itemsByDepartment &&
          (
            Object.entries(itemsByDepartment) as [string, ListItemResponse[]][]
          ).map(([deptId, deptItems]) => {
            const dept = departments?.find((d) => d.id === deptId);
            return (
              <div key={deptId}>
                <h3 className="mb-2 text-xs font-semibold uppercase tracking-wide text-gray-400">
                  {dept?.icon} {dept?.name ?? "Departamento"}
                </h3>
                <div className="space-y-1">
                  {deptItems.map((item: ListItemResponse) => (
                    <button
                      key={item.id}
                      onClick={() => handleToggleCheck(item)}
                      className={`flex w-full items-center gap-3 rounded-xl border px-4 py-3 text-left transition ${
                        item.isChecked
                          ? "border-green-200 bg-green-50"
                          : "border-gray-200 bg-white hover:border-green-300"
                      }`}
                    >
                      <div
                        className={`flex h-5 w-5 shrink-0 items-center justify-center rounded-full border-2 transition ${
                          item.isChecked
                            ? "border-green-500 bg-green-500"
                            : "border-gray-300"
                        }`}
                      >
                        {item.isChecked && (
                          <svg
                            className="h-3 w-3 text-white"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke="currentColor"
                          >
                            <path
                              strokeLinecap="round"
                              strokeLinejoin="round"
                              strokeWidth={3}
                              d="M5 13l4 4L19 7"
                            />
                          </svg>
                        )}
                      </div>
                      <span
                        className={`flex-1 font-medium ${item.isChecked ? "text-gray-400 line-through" : "text-gray-800"}`}
                      >
                        {item.itemName}
                      </span>
                      <span className="text-sm text-gray-400">
                        {item.quantity} {ItemUnitLabel[item.unit]}
                      </span>
                    </button>
                  ))}
                </div>
              </div>
            );
          })}

        {totalCount === 0 && (
          <p className="mt-8 text-center text-sm text-gray-400">
            Nenhum item na lista. Adicione um!
          </p>
        )}
      </div>

      {/* Botão flutuante de adicionar */}
      <button
        onClick={() => setIsAddItemOpen(true)}
        className="fixed bottom-6 right-6 flex h-14 w-14 items-center justify-center rounded-full bg-green-600 text-2xl text-white shadow-lg hover:bg-green-700"
      >
        +
      </button>

      {/* Modal: adicionar item à lista */}
      <Modal
        isOpen={isAddItemOpen}
        onClose={() => setIsAddItemOpen(false)}
        title="Adicionar item"
      >
        <form onSubmit={handleAddItem} className="space-y-3">
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Item
            </label>
            <div className="flex gap-2">
              <select
                value={selectedItemId}
                onChange={(e) => setSelectedItemId(e.target.value)}
                className="flex-1 rounded-lg border border-gray-300 px-3 py-2.5 text-gray-800 focus:border-green-500 focus:outline-none"
              >
                <option value="">Selecione...</option>
                {items?.map((item) => (
                  <option key={item.id} value={item.id}>
                    {item.name} ({ItemUnitLabel[item.unit]})
                  </option>
                ))}
              </select>
              <button
                type="button"
                onClick={() => {
                  setIsAddItemOpen(false);
                  setIsNewItemOpen(true);
                }}
                className="rounded-lg border border-gray-300 px-3 text-gray-500 hover:border-green-500 hover:text-green-600"
              >
                +
              </button>
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Departamento
            </label>
            <div className="flex gap-2">
              <select
                value={selectedDeptId}
                onChange={(e) => setSelectedDeptId(e.target.value)}
                className="flex-1 rounded-lg border border-gray-300 px-3 py-2.5 text-gray-800 focus:border-green-500 focus:outline-none"
              >
                <option value="">Selecione...</option>
                {departments?.map((dept) => (
                  <option key={dept.id} value={dept.id}>
                    {dept.icon} {dept.name}
                  </option>
                ))}
              </select>
              <button
                type="button"
                onClick={() => {
                  setIsAddItemOpen(false);
                  setIsNewDeptOpen(true);
                }}
                className="rounded-lg border border-gray-300 px-3 text-gray-500 hover:border-green-500 hover:text-green-600"
              >
                +
              </button>
            </div>
          </div>

          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Quantidade
            </label>
            <input
              type="number"
              min="0.1"
              step="0.1"
              value={quantity}
              onChange={(e) => setQuantity(e.target.value)}
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none"
            />
          </div>

          <button
            type="submit"
            disabled={!selectedItemId || !selectedDeptId || addItem.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {addItem.isPending ? "Adicionando..." : "Adicionar"}
          </button>
        </form>
      </Modal>

      {/* Modal: novo item */}
      <Modal
        isOpen={isNewItemOpen}
        onClose={() => setIsNewItemOpen(false)}
        title="Novo item"
      >
        <form onSubmit={handleCreateItem} className="space-y-3">
          <input
            type="text"
            autoFocus
            value={newItemName}
            onChange={(e) => setNewItemName(e.target.value)}
            placeholder="Nome do item"
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none"
          />
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Unidade
            </label>
            <select
              value={newItemUnit}
              onChange={(e) =>
                setNewItemUnit(Number(e.target.value) as ItemUnit)
              }
              className="w-full rounded-lg border border-gray-300 px-3 py-2.5 focus:border-green-500 focus:outline-none"
            >
              {Object.entries(ItemUnitLabel).map(([value, label]) => (
                <option key={value} value={value}>
                  {label}
                </option>
              ))}
            </select>
          </div>
          <button
            type="submit"
            disabled={createItem.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {createItem.isPending ? "Criando..." : "Criar item"}
          </button>
        </form>
      </Modal>

      {/* Modal: novo departamento */}
      <Modal
        isOpen={isNewDeptOpen}
        onClose={() => setIsNewDeptOpen(false)}
        title="Novo departamento"
      >
        <form onSubmit={handleCreateDept} className="space-y-3">
          <input
            type="text"
            autoFocus
            value={newDeptName}
            onChange={(e) => setNewDeptName(e.target.value)}
            placeholder="Nome do departamento"
            className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none"
          />
          <div>
            <label className="mb-1 block text-sm font-medium text-gray-700">
              Ícone
            </label>
            <input
              type="text"
              value={newDeptIcon}
              onChange={(e) => setNewDeptIcon(e.target.value)}
              placeholder="Ex: 🍳"
              className="w-full rounded-lg border border-gray-300 px-4 py-2.5 focus:border-green-500 focus:outline-none"
            />
          </div>
          <button
            type="submit"
            disabled={createDepartment.isPending}
            className="w-full rounded-lg bg-green-600 py-2.5 font-medium text-white hover:bg-green-700 disabled:opacity-50"
          >
            {createDepartment.isPending ? "Criando..." : "Criar departamento"}
          </button>
        </form>
      </Modal>

      <ConfirmModal
        isOpen={isArchiveOpen}
        onClose={() => setIsArchiveOpen(false)}
        onConfirm={handleArchive}
        title="Arquivar lista"
        message={`Deseja arquivar "${list?.name}"? Ela será movida para o histórico.`}
        confirmLabel="Arquivar"
        danger
        loading={archiveList.isPending}
      />
    </div>
  );
}
