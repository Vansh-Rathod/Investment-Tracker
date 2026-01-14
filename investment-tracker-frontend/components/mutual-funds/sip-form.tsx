"use client"

import type React from "react"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import type { MutualFundSIP, MutualFundSIPFormData } from "@/types"

interface SIPFormProps {
  initialData?: MutualFundSIP
  onSubmit?: (data: MutualFundSIPFormData) => void
}

const amcOptions = [
  "HDFC Mutual Fund",
  "SBI Mutual Fund",
  "ICICI Prudential",
  "Axis Mutual Fund",
  "Kotak Mutual Fund",
  "Nippon India",
  "Aditya Birla Sun Life",
  "UTI Mutual Fund",
]

export function SIPForm({ initialData, onSubmit }: SIPFormProps) {
  const router = useRouter()
  const [formData, setFormData] = useState<MutualFundSIPFormData>({
    amcName: initialData?.amcName || "",
    schemeName: initialData?.schemeName || "",
    folioNumber: initialData?.folioNumber || "",
    sipAmount: initialData?.sipAmount || 0,
    sipDate: initialData?.sipDate || 1,
    startDate: initialData?.startDate || "",
    endDate: initialData?.endDate || "",
    frequency: initialData?.frequency || "monthly",
    nav: initialData?.nav || 0,
  })

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    onSubmit?.(formData)
    router.push("/mutual-funds")
  }

  return (
    <Card className="max-w-2xl">
      <CardHeader>
        <CardTitle>{initialData ? "Edit SIP" : "Add New SIP"}</CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="space-y-6">
          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="amcName">AMC Name</Label>
              <Select value={formData.amcName} onValueChange={(value) => setFormData({ ...formData, amcName: value })}>
                <SelectTrigger id="amcName">
                  <SelectValue placeholder="Select AMC" />
                </SelectTrigger>
                <SelectContent>
                  {amcOptions.map((amc) => (
                    <SelectItem key={amc} value={amc}>
                      {amc}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="schemeName">Scheme Name</Label>
              <Input
                id="schemeName"
                value={formData.schemeName}
                onChange={(e) => setFormData({ ...formData, schemeName: e.target.value })}
                placeholder="e.g., HDFC Flexi Cap Fund"
                required
              />
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="folioNumber">Folio Number</Label>
              <Input
                id="folioNumber"
                value={formData.folioNumber}
                onChange={(e) => setFormData({ ...formData, folioNumber: e.target.value })}
                placeholder="e.g., 1234567890"
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="sipAmount">SIP Amount (₹)</Label>
              <Input
                id="sipAmount"
                type="number"
                min={500}
                step={100}
                value={formData.sipAmount || ""}
                onChange={(e) => setFormData({ ...formData, sipAmount: Number(e.target.value) })}
                placeholder="e.g., 5000"
                required
              />
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-3">
            <div className="space-y-2">
              <Label htmlFor="sipDate">SIP Date</Label>
              <Select
                value={formData.sipDate.toString()}
                onValueChange={(value) => setFormData({ ...formData, sipDate: Number(value) })}
              >
                <SelectTrigger id="sipDate">
                  <SelectValue placeholder="Select date" />
                </SelectTrigger>
                <SelectContent>
                  {Array.from({ length: 28 }, (_, i) => i + 1).map((day) => (
                    <SelectItem key={day} value={day.toString()}>
                      {day}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="frequency">Frequency</Label>
              <Select
                value={formData.frequency}
                onValueChange={(value: "monthly" | "quarterly" | "weekly") =>
                  setFormData({ ...formData, frequency: value })
                }
              >
                <SelectTrigger id="frequency">
                  <SelectValue placeholder="Select frequency" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="weekly">Weekly</SelectItem>
                  <SelectItem value="monthly">Monthly</SelectItem>
                  <SelectItem value="quarterly">Quarterly</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="nav">NAV at Purchase</Label>
              <Input
                id="nav"
                type="number"
                step={0.01}
                value={formData.nav || ""}
                onChange={(e) => setFormData({ ...formData, nav: Number(e.target.value) })}
                placeholder="e.g., 45.50"
                required
              />
            </div>
          </div>

          <div className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2">
              <Label htmlFor="startDate">Start Date</Label>
              <Input
                id="startDate"
                type="date"
                value={formData.startDate}
                onChange={(e) => setFormData({ ...formData, startDate: e.target.value })}
                required
              />
            </div>

            <div className="space-y-2">
              <Label htmlFor="endDate">End Date (Optional)</Label>
              <Input
                id="endDate"
                type="date"
                value={formData.endDate || ""}
                onChange={(e) => setFormData({ ...formData, endDate: e.target.value })}
              />
            </div>
          </div>

          <div className="flex gap-4">
            <Button type="submit">{initialData ? "Update SIP" : "Create SIP"}</Button>
            <Button type="button" variant="outline" onClick={() => router.back()}>
              Cancel
            </Button>
          </div>
        </form>
      </CardContent>
    </Card>
  )
}
