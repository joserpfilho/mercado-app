import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { departmentsApi } from "../api/departments";
import { itemsApi } from "../api/items";
import type { Department, ItemUnit } from "../types";

export function useDepartments(groupId: string | null) {
  return useQuery({
    queryKey: ["departments", groupId],
    queryFn: () => departmentsApi.getByGroup(groupId!),
    enabled: !!groupId,
  });
}

export function useCreateDepartment(groupId: string | null) {
  const queryClient = useQueryClient();

  return useMutation<Department, Error, { name: string; icon: string }>({
    mutationFn: (payload) =>
      departmentsApi.create(groupId!, payload.name, payload.icon),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["departments", groupId] });
    },
  });
}

export function useItems(groupId: string | null) {
  return useQuery({
    queryKey: ["items", groupId],
    queryFn: () => itemsApi.getByGroup(groupId!),
    enabled: !!groupId,
  });
}

export function useCreateItem(groupId: string | null) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (payload: { name: string; unit: ItemUnit }) =>
      itemsApi.create(groupId!, payload.name, payload.unit),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["items", groupId] });
    },
  });
}
