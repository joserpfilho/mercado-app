import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { shoppingListsApi } from "../api/shoppingLists";
import type { ListStatus } from "../types";

export function useShoppingLists(groupId: string | null, status?: ListStatus) {
  return useQuery({
    queryKey: ["lists", groupId, status],
    queryFn: () => shoppingListsApi.getByGroup(groupId!, status),
    enabled: !!groupId,
  });
}

export function useCreateShoppingList(groupId: string | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (name: string) => shoppingListsApi.create(groupId!, name),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["lists", groupId] });
    },
  });
}
