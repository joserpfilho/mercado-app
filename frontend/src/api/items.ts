import { api } from './client'
import type { Item, ItemUnit } from '../types'

export const itemsApi = {
  getByGroup: async (groupId: string) => {
    const { data } = await api.get<Item[]>(`/groups/${groupId}/items`)
    return data
  },

  create: async (groupId: string, name: string, unit: ItemUnit) => {
    const { data } = await api.post<Item>(`/groups/${groupId}/items`, { name, unit })
    return data
  },

  delete: async (groupId: string, itemId: string) => {
    await api.delete(`/groups/${groupId}/items/${itemId}`)
  },
}