import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { TrendingUp, TrendingDown, Wallet, BarChart3 } from "lucide-react"
import type { PortfolioSummary } from "@/types"
import { cn } from "@/lib/utils"

interface SummaryCardsProps {
  summary: PortfolioSummary
}

export function SummaryCards({ summary }: SummaryCardsProps) {
  const isPositive = summary.totalReturns >= 0

  const cards = [
    {
      title: "Total Invested",
      value: `₹${summary.totalInvested.toLocaleString("en-IN")}`,
      icon: Wallet,
      description: "Across all investments",
    },
    {
      title: "Current Value",
      value: `₹${summary.currentValue.toLocaleString("en-IN")}`,
      icon: BarChart3,
      description: "Portfolio worth today",
    },
    {
      title: "Total Returns",
      value: `₹${Math.abs(summary.totalReturns).toLocaleString("en-IN")}`,
      icon: isPositive ? TrendingUp : TrendingDown,
      description: `${isPositive ? "+" : "-"}${summary.totalReturnsPercentage.toFixed(2)}%`,
      positive: isPositive,
    },
    {
      title: "Holdings",
      value: `${summary.mutualFundsCount + summary.stocksCount}`,
      icon: BarChart3,
      description: `${summary.mutualFundsCount} MFs, ${summary.stocksCount} Stocks`,
    },
  ]

  return (
    <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
      {cards.map((card) => (
        <Card key={card.title}>
          <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
            <CardTitle className="text-sm font-medium text-muted-foreground">{card.title}</CardTitle>
            <card.icon
              className={cn(
                "h-5 w-5",
                card.positive !== undefined
                  ? card.positive
                    ? "text-green-600"
                    : "text-red-600"
                  : "text-muted-foreground",
              )}
            />
          </CardHeader>
          <CardContent>
            <div
              className={cn(
                "text-2xl font-bold",
                card.positive !== undefined ? (card.positive ? "text-green-600" : "text-red-600") : "text-foreground",
              )}
            >
              {card.positive !== undefined && (isPositive ? "+" : "-")}
              {card.value}
            </div>
            <p className="text-xs text-muted-foreground mt-1">{card.description}</p>
          </CardContent>
        </Card>
      ))}
    </div>
  )
}
