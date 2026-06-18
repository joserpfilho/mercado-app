import { api } from './client'
import type { Group, GroupMember } from '../types'

export const groupsApi = {
  getMyGroups: async () => {
    const { data } = await api.get<Group[]>('/groups')
    return data
  },

  create: async (name: string) => {
    const { data } = await api.post<Group>('/groups', { name })
    return data
  },

  getMembers: async (groupId: string) => {
    const { data } = await api.get<GroupMember[]>(`/groups/${groupId}/members`)
    return data
  },

  addMember: async (groupId: string, email: string) => {
    const { data } = await api.post<GroupMember>(`/groups/${groupId}/members`, { email })
    return data
  },
}