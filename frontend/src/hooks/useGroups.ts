import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { groupsApi } from "../api/groups";

export function useGroups() {
  return useQuery({
    queryKey: ["groups"],
    queryFn: groupsApi.getMyGroups,
  });
}

export function useCreateGroup() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (name: string) => groupsApi.create(name),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["groups"] });
    },
  });
}
