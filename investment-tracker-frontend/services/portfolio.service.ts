import api from "@/lib/axios"
import type { PortfolioSummary, AllocationData, PerformanceData, ApiResponse } from "@/types"

export const portfolioService = {
  // Get portfolio summary
  getSummary: async (): Promise<PortfolioSummary> => {
    const response = await api.get<ApiResponse<PortfolioSummary>>("/portfolio/summary")
    return response.data.data
  },

  // Get allocation data for pie chart
  getAllocation: async (): Promise<AllocationData[]> => {
    const response = await api.get<ApiResponse<AllocationData[]>>("/portfolio/allocation")
    return response.data.data
  },

  // Get performance data for line chart
  getPerformance: async (): Promise<PerformanceData[]> => {
    const response = await api.get<ApiResponse<PerformanceData[]>>("/portfolio/performance")
    return response.data.data
  },
}
