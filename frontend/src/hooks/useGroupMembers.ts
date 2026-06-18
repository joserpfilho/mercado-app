import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { groupsApi } from "../api/groups";
import type { GroupMember } from "../types";

export function useGroupMembers(groupId: string | null) {
  return useQuery<GroupMember[]>({
    queryKey: ["members", groupId],
    queryFn: () => groupsApi.getMembers(groupId!),
    enabled: !!groupId,
  });
}

export function useAddMember(groupId: string | null) {
  const queryClient = useQueryClient();

  return useMutation<GroupMember, Error, string>({
    mutationFn: (email: string) => groupsApi.addMember(groupId!, email),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["members", groupId] });
    },
  });
}
