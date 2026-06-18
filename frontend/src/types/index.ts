export const ItemUnit = {
  Unidade: 0,
  Kg: 1,
  G: 2,
  L: 3,
  Ml: 4,
  Caixa: 5,
} as const

export type ItemUnit = (typeof ItemUnit)[keyof typeof ItemUnit]

export const ItemUnitLabel: Record<ItemUnit, string> = {
  [ItemUnit.Unidade]: 'un',
  [ItemUnit.Kg]: 'kg',
  [ItemUnit.G]: 'g',
  [ItemUnit.L]: 'L',
  [ItemUnit.Ml]: 'ml',
  [ItemUnit.Caixa]: 'cx',
}

export const ListStatus = {
  Active: 0,
  Archived: 1,
} as const

export type ListStatus = (typeof ListStatus)[keyof typeof ListStatus]

export interface AuthResponse {
  token: string
  name: string
  email: string
}

export interface Group {
  id: string
  name: string
  createdAt: string
}

export interface Department {
  id: string
  name: string
  icon: string
}

export interface Item {
  id: string
  name: string
  unit: ItemUnit
}

export interface ListItemResponse {
  id: string
  itemId: string
  itemName: string
  unit: ItemUnit
  quantity: number
  isChecked: boolean
  departmentId: string
  departmentName: string
  departmentIcon: string
}

export interface ShoppingList {
  id: string
  name: string
  createdAt: string
  status: ListStatus
  items: ListItemResponse[]
}

export interface ShoppingListSummary {
  id: string
  name: string
  createdAt: string
  status: ListStatus
  totalItems: number
  checkedItems: number
}

export interface GroupMember {
  userId: string
  name: string
  email: string
  role: string
}