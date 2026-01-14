// Mutual Fund SIP Interface
export interface MutualFundSIP {
  id: string
  orderId: string
  amcName: string
  schemeName: string
  folioNumber: string
  sipAmount: number
  sipDate: number // Day of month (1-28)
  startDate: string
  endDate?: string
  frequency: "monthly" | "quarterly" | "weekly"
  nav: number // NAV at purchase
  currentNav: number
  units: number
  investedValue: number
  currentValue: number
  returns: number
  returnsPercentage: number
  status: "active" | "paused" | "completed"
}

// Stock Investment Interface
export interface StockInvestment {
  id: string
  symbol: string
  companyName: string
  exchange: "NSE" | "BSE"
  quantity: number
  averagePrice: number
  currentPrice: number
  investedValue: number
  currentValue: number
  returns: number
  returnsPercentage: number
  purchaseDate: string
  sector: string
}

// Portfolio Summary Interface
export interface PortfolioSummary {
  totalInvested: number
  currentValue: number
  totalReturns: number
  totalReturnsPercentage: number
  mutualFundsValue: number
  stocksValue: number
  mutualFundsCount: number
  stocksCount: number
}

// Chart Data Interfaces
export interface AllocationData {
  name: string
  value: number
  fill: string
}

export interface PerformanceData {
  month: string
  mutualFunds: number
  stocks: number
  total: number
}

// API Response Types
export interface ApiResponse<T> {
  data: T
  success: boolean
  message?: string
}

// Form Types
export interface MutualFundSIPFormData {
  amcName: string
  schemeName: string
  folioNumber: string
  sipAmount: number
  sipDate: number
  startDate: string
  endDate?: string
  frequency: "monthly" | "quarterly" | "weekly"
  nav: number
}

export interface StockFormData {
  symbol: string
  companyName: string
  exchange: "NSE" | "BSE"
  quantity: number
  averagePrice: number
  purchaseDate: string
  sector: string
}
