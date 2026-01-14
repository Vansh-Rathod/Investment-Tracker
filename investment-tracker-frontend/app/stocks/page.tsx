import Link from "next/link"
import { MainLayout } from "@/components/layout/main-layout"
import { StockTable } from "@/components/stocks/stock-table"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Plus, TrendingUp, Wallet, Briefcase } from "lucide-react"
import { mockStocks } from "@/data/mock-data"

export default function StocksPage() {
  const totalInvested = mockStocks.reduce((sum, stock) => sum + stock.investedValue, 0)
  const totalCurrent = mockStocks.reduce((sum, stock) => sum + stock.currentValue, 0)
  const totalReturns = totalCurrent - totalInvested
  const returnsPercentage = (totalReturns / totalInvested) * 100

  // Calculate best performer
  const bestPerformer = mockStocks.reduce((best, stock) =>
    stock.returnsPercentage > best.returnsPercentage ? stock : best,
  )

  return (
    <MainLayout title="Stocks">
      <div className="space-y-6">
        {/* Summary Cards */}
        <div className="grid gap-4 md:grid-cols-4">
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">Total Invested</CardTitle>
              <Wallet className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">₹{totalInvested.toLocaleString("en-IN")}</div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">Current Value</CardTitle>
              <TrendingUp className="h-4 w-4 text-muted-foreground" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">₹{totalCurrent.toLocaleString("en-IN")}</div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">Total Returns</CardTitle>
              <TrendingUp className="h-4 w-4 text-green-600" />
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold text-green-600">
                +₹{totalReturns.toLocaleString("en-IN")}
                <span className="text-sm font-normal ml-1">({returnsPercentage.toFixed(2)}%)</span>
              </div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">Best Performer</CardTitle>
              <Briefcase className="h-4 w-4 text-green-600" />
            </CardHeader>
            <CardContent>
              <div className="text-xl font-bold">{bestPerformer.symbol}</div>
              <p className="text-sm text-green-600">+{bestPerformer.returnsPercentage.toFixed(2)}%</p>
            </CardContent>
          </Card>
        </div>

        {/* Action Bar */}
        <div className="flex items-center justify-between">
          <h2 className="text-lg font-semibold">Your Stock Holdings</h2>
          <Button asChild>
            <Link href="/stocks/add">
              <Plus className="mr-2 h-4 w-4" />
              Add New Stock
            </Link>
          </Button>
        </div>

        {/* Stock Table */}
        <StockTable stocks={mockStocks} />
      </div>
    </MainLayout>
  )
}
