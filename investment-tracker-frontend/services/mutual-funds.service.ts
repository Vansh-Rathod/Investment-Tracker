import api from "@/lib/axios"
import type { MutualFundSIP, MutualFundSIPFormData, ApiResponse } from "@/types"

export const mutualFundsService = {
  // Get all mutual fund SIPs
  getAll: async (): Promise<MutualFundSIP[]> => {
    const response = await api.get<ApiResponse<MutualFundSIP[]>>("/mutual-funds")
    return response.data.data
  },

  // Get single mutual fund SIP by ID
  getById: async (id: string): Promise<MutualFundSIP> => {
    const response = await api.get<ApiResponse<MutualFundSIP>>(`/mutual-funds/${id}`)
    return response.data.data
  },

  // Create new mutual fund SIP
  create: async (data: MutualFundSIPFormData): Promise<MutualFundSIP> => {
    const response = await api.post<ApiResponse<MutualFundSIP>>("/mutual-funds", data)
    return response.data.data
  },

  // Update existing mutual fund SIP
  update: async (id: string, data: Partial<MutualFundSIPFormData>): Promise<MutualFundSIP> => {
    const response = await api.put<ApiResponse<MutualFundSIP>>(`/mutual-funds/${id}`, data)
    return response.data.data
  },

  // Delete mutual fund SIP
  delete: async (id: string): Promise<void> => {
    await api.delete(`/mutual-funds/${id}`)
  },

  // Pause SIP
  pause: async (id: string): Promise<MutualFundSIP> => {
    const response = await api.patch<ApiResponse<MutualFundSIP>>(`/mutual-funds/${id}/pause`)
    return response.data.data
  },

  // Resume SIP
  resume: async (id: string): Promise<MutualFundSIP> => {
    const response = await api.patch<ApiResponse<MutualFundSIP>>(`/mutual-funds/${id}/resume`)
    return response.data.data
  },
}
