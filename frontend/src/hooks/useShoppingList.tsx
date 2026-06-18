import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { shoppingListsApi } from "../api/shoppingLists";

export function useShoppingList(listId: string) {
  return useQuery({
    queryKey: ["list", listId],
    queryFn: () => shoppingListsApi.getById(listId),
    enabled: !!listId,
  });
}

export function useAddListItem(listId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: {
      itemId: string;
      departmentId: string;
      quantity: number;
    }) =>
      shoppingListsApi.addItem(
        listId,
        payload.itemId,
        payload.departmentId,
        payload.quantity,
      ),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["list", listId] });
    },
  });
}

export function useUpdateListItem(listId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: {
      listItemId: string;
      isChecked?: boolean;
      quantity?: number;
    }) =>
      shoppingListsApi.updateItem(listId, payload.listItemId, {
        isChecked: payload.isChecked,
        quantity: payload.quantity,
      }),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["list", listId] });
    },
  });
}

export function useArchiveList() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (listId: string) => shoppingListsApi.archive(listId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lists"] });
    },
  });
}
