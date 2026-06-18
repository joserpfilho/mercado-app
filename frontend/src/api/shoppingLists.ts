import { api } from './client'
import type { ShoppingList, ShoppingListSummary, ListStatus } from '../types'

export const shoppingListsApi = {
  getByGroup: async (groupId: string, status?: ListStatus) => {
    const { data } = await api.get<ShoppingListSummary[]>(`/groups/${groupId}/lists`, {
      params: status !== undefined ? { status } : undefined,
    })
    return data
  },

  create: async (groupId: string, name: string) => {
    const { data } = await api.post<ShoppingListSummary>(`/groups/${groupId}/lists`, { name })
    return data
  },

  getById: async (listId: string) => {
    const { data } = await api.get<ShoppingList>(`/lists/${listId}`)
    return data
  },

  addItem: async (listId: string, itemId: string, departmentId: string, quantity: number) => {
    const { data } = await api.post<ShoppingList>(`/lists/${listId}/items`, {
      itemId,
      departmentId,
      quantity,
    })
    return data
  },

  updateItem: async (listId: string, listItemId: string, payload: { isChecked?: boolean; quantity?: number }) => {
    const { data } = await api.patch<ShoppingList>(`/lists/${listId}/items/${listItemId}`, payload)
    return data
  },

  archive: async (listId: string) => {
    const { data } = await api.patch<ShoppingListSummary>(`/lists/${listId}/archive`)
    return data
  },
}