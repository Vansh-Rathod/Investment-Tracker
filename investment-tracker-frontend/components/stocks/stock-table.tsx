"use client"

import { useState } from "react"
import Link from "next/link"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { MoreHorizontal, Pencil, Trash2 } from "lucide-react"
import type { StockInvestment } from "@/types"
import { cn } from "@/lib/utils"

interface StockTableProps {
  stocks: StockInvestment[]
  onDelete?: (id: string) => void
}

export function StockTable({ stocks, onDelete }: StockTableProps) {
  const [data, setData] = useState(stocks)

  const handleDelete = (id: string) => {
    setData(data.filter((stock) => stock.id !== id))
    onDelete?.(id)
  }

  return (
    <div className="rounded-lg border border-border bg-card">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Stock</TableHead>
            <TableHead>Sector</TableHead>
            <TableHead className="text-right">Quantity</TableHead>
            <TableHead className="text-right">Avg. Price</TableHead>
            <TableHead className="text-right">Current Price</TableHead>
            <TableHead className="text-right">Invested</TableHead>
            <TableHead className="text-right">Current Value</TableHead>
            <TableHead className="text-right">Returns</TableHead>
            <TableHead className="w-[50px]" />
          </TableRow>
        </TableHeader>
        <TableBody>
          {data.map((stock) => (
            <TableRow key={stock.id}>
              <TableCell>
                <div>
                  <div className="flex items-center gap-2">
                    <span className="font-medium text-foreground">{stock.symbol}</span>
                    <Badge variant="outline" className="text-xs">
                      {stock.exchange}
                    </Badge>
                  </div>
                  <div className="text-sm text-muted-foreground">{stock.companyName}</div>
                </div>
              </TableCell>
              <TableCell>
                <Badge variant="secondary">{stock.sector}</Badge>
              </TableCell>
              <TableCell className="text-right font-medium">{stock.quantity}</TableCell>
              <TableCell className="text-right">₹{stock.averagePrice.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">₹{stock.currentPrice.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">₹{stock.investedValue.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">₹{stock.currentValue.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">
                <div className={cn("font-medium", stock.returns >= 0 ? "text-green-600" : "text-red-600")}>
                  {stock.returns >= 0 ? "+" : ""}₹{stock.returns.toLocaleString("en-IN")}
                  <div className="text-xs">({stock.returnsPercentage.toFixed(2)}%)</div>
                </div>
              </TableCell>
              <TableCell>
                <DropdownMenu>
                  <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon">
                      <MoreHorizontal className="h-4 w-4" />
                    </Button>
                  </DropdownMenuTrigger>
                  <DropdownMenuContent align="end">
                    <DropdownMenuItem asChild>
                      <Link href={`/stocks/${stock.id}/edit`}>
                        <Pencil className="mr-2 h-4 w-4" />
                        Edit
                      </Link>
                    </DropdownMenuItem>
                    <DropdownMenuItem className="text-destructive" onClick={() => handleDelete(stock.id)}>
                      <Trash2 className="mr-2 h-4 w-4" />
                      Delete
                    </DropdownMenuItem>
                  </DropdownMenuContent>
                </DropdownMenu>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  )
}
