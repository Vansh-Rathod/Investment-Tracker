import api from "@/lib/axios"
import type { StockInvestment, StockFormData, ApiResponse } from "@/types"

export const stocksService = {
  // Get all stock investments
  getAll: async (): Promise<StockInvestment[]> => {
    const response = await api.get<ApiResponse<StockInvestment[]>>("/stocks")
    return response.data.data
  },

  // Get single stock investment by ID
  getById: async (id: string): Promise<StockInvestment> => {
    const response = await api.get<ApiResponse<StockInvestment>>(`/stocks/${id}`)
    return response.data.data
  },

  // Create new stock investment
  create: async (data: StockFormData): Promise<StockInvestment> => {
    const response = await api.post<ApiResponse<StockInvestment>>("/stocks", data)
    return response.data.data
  },

  // Update existing stock investment
  update: async (id: string, data: Partial<StockFormData>): Promise<StockInvestment> => {
    const response = await api.put<ApiResponse<StockInvestment>>(`/stocks/${id}`, data)
    return response.data.data
  },

  // Delete stock investment
  delete: async (id: string): Promise<void> => {
    await api.delete(`/stocks/${id}`)
  },
}
