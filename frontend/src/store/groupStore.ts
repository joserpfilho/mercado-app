import { create } from 'zustand'

interface GroupState {
  activeGroupId: string | null
  activeGroupName: string | null
  setActiveGroup: (id: string, name: string) => void
}

export const useGroupStore = create<GroupState>((set) => ({
  activeGroupId: localStorage.getItem('activeGroupId'),
  activeGroupName: localStorage.getItem('activeGroupName'),

  setActiveGroup: (id, name) => {
    localStorage.setItem('activeGroupId', id)
    localStorage.setItem('activeGroupName', name)
    set({ activeGroupId: id, activeGroupName: name })
  },
}))