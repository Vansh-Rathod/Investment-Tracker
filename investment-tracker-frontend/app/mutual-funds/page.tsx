import Link from "next/link"
import { MainLayout } from "@/components/layout/main-layout"
import { SIPTable } from "@/components/mutual-funds/sip-table"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Plus, TrendingUp, Wallet } from "lucide-react"
import { mockMutualFunds } from "@/data/mock-data"

export default function MutualFundsPage() {
  const totalInvested = mockMutualFunds.reduce((sum, mf) => sum + mf.investedValue, 0)
  const totalCurrent = mockMutualFunds.reduce((sum, mf) => sum + mf.currentValue, 0)
  const totalReturns = totalCurrent - totalInvested
  const activeSIPs = mockMutualFunds.filter((mf) => mf.status === "active").length

  return (
    <MainLayout title="Mutual Funds">
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
              <div className="text-2xl font-bold text-green-600">+₹{totalReturns.toLocaleString("en-IN")}</div>
            </CardContent>
          </Card>
          <Card>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
              <CardTitle className="text-sm font-medium text-muted-foreground">Active SIPs</CardTitle>
            </CardHeader>
            <CardContent>
              <div className="text-2xl font-bold">
                {activeSIPs}/{mockMutualFunds.length}
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Action Bar */}
        <div className="flex items-center justify-between">
          <h2 className="text-lg font-semibold">Your SIP Investments</h2>
          <Button asChild>
            <Link href="/mutual-funds/add">
              <Plus className="mr-2 h-4 w-4" />
              Add New SIP
            </Link>
          </Button>
        </div>

        {/* SIP Table */}
        <SIPTable sips={mockMutualFunds} />
      </div>
    </MainLayout>
  )
}
