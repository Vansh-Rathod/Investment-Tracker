"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import type { StockInvestment, StockFormData } from "@/types"

interface StockFormProps {
  initialData?: StockInvestment
  onSubmit?: (data: StockFormData) => void
}

const sectorOptions = [
  "IT",
  "Banking",
  "Energy",
  "Telecom",
  "Pharma",
  "Auto",
  "FMCG",
  "Metals",
  "Infrastructure",
  "Finance",
]

export function StockForm({ initialData, onSubmit }: StockFormProps) {
  const router = useRouter()
  const [formData, setFormData] = useState<StockFormData>({
    symbol: initialData?.symbol || "",
    companyName: initialData?.companyName || "",
    exchange: initialData?.exchange || "NSE",
    quantity: initialData?.quantity || 0,
    averagePrice: initialData?.averagePrice || 0,
    purchaseDate: initialData?.purchaseDate || "",
    sector: initialData?.sector || "",
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSubmit?.(formData)
    router.push("/stocks")
  }

  return (
    <Card className="max-w-2xl">
      <CardHeader>
        <CardTitle>{initialData ? "Edit Stock" : "Add New Stock"}</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="symbol">Stock Symbol</Label>
              <Input
                id="symbol"
                value={formData.symbol}
                onChange={(e) => setFormData({ ...formData, symbol: e.target.value.toUpperCase() })}
                placeholder="e.g., RELIANCE"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="companyName">Company Name</Label>
              <Input
                id="companyName"
                value={formData.companyName}
                onChange={(e) => setFormData({ ...formData, companyName: e.target.value })}
                placeholder="e.g., Reliance Industries Ltd"
                required
              />
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="exchange">Exchange</Label>
              <Select
                value={formData.exchange}
                onValueChange={(value: "NSE" | "BSE") => setFormData({ ...formData, exchange: value })}
              >
                <SelectTrigger id="exchange">
                  <SelectValue placeholder="Select exchange" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="NSE">NSE</SelectItem>
                  <SelectItem value="BSE">BSE</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="sector">Sector</Label>
              <Select value={formData.sector} onValueChange={(value) => setFormData({ ...formData, sector: value })}>
                <SelectTrigger id="sector">
                  <SelectValue placeholder="Select sector" />
                </SelectTrigger>
                <SelectContent>
                  {sectorOptions.map((sector) => (
                    <SelectItem key={sector} value={sector}>
                      {sector}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-3">
            <div className="space-y-2">
              <Label htmlFor="quantity">Quantity</Label>
              <Input
                id="quantity"
                type="number"
                min={1}
                value={formData.quantity || ""}
                onChange={(e) => setFormData({ ...formData, quantity: Number(e.target.value) })}
                placeholder="e.g., 50"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="averagePrice">Average Price (₹)</Label>
              <Input
                id="averagePrice"
                type="number"
                min={0}
                step={0.01}
                value={formData.averagePrice || ""}
                onChange={(e) => setFormData({ ...formData, averagePrice: Number(e.target.value) })}
                placeholder="e.g., 2450.00"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="purchaseDate">Purchase Date</Label>
              <Input
                id="purchaseDate"
                type="date"
                value={formData.purchaseDate}
                onChange={(e) => setFormData({ ...formData, purchaseDate: e.target.value })}
                required
              />
            </div>
          </div>

          <div className="flex gap-4">
            <Button type="submit">{initialData ? "Update Stock" : "Add Stock"}</Button>
            <Button type="button" variant="outline" onClick={() => router.back()}>
              Cancel
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  )
}
