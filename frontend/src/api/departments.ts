import { api } from './client'
import type { Department } from '../types'

export const departmentsApi = {
  getByGroup: async (groupId: string) => {
    const { data } = await api.get<Department[]>(`/groups/${groupId}/departments`)
    return data
  },

  create: async (groupId: string, name: string, icon: string) => {
    const { data } = await api.post<Department>(`/groups/${groupId}/departments`, { name, icon })
    return data
  },

  delete: async (groupId: string, departmentId: string) => {
    await api.delete(`/groups/${groupId}/departments/${departmentId}`)
  },
}