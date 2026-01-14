"use client"

import { useState } from "react"
import Link from "next/link"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { DropdownMenu, DropdownMenuContent, DropdownMenuItem, DropdownMenuTrigger } from "@/components/ui/dropdown-menu"
import { MoreHorizontal, Pencil, Trash2, Pause, Play } from "lucide-react"
import type { MutualFundSIP } from "@/types"
import { cn } from "@/lib/utils"

interface SIPTableProps {
  sips: MutualFundSIP[]
  onDelete?: (id: string) => void
  onPause?: (id: string) => void
  onResume?: (id: string) => void
}

export function SIPTable({ sips, onDelete, onPause, onResume }: SIPTableProps) {
  const [data, setData] = useState(sips)

  const handleDelete = (id: string) => {
    setData(data.filter((sip) => sip.id !== id))
    onDelete?.(id)
  }

  const handleToggleStatus = (id: string, currentStatus: string) => {
    setData(
      data.map((sip) =>
        sip.id === id ? { ...sip, status: currentStatus === "active" ? "paused" : "active" } : sip,
      ) as MutualFundSIP[],
    )
    if (currentStatus === "active") {
      onPause?.(id)
    } else {
      onResume?.(id)
    }
  }

  return (
    <div className="rounded-lg border border-border bg-card">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Scheme</TableHead>
            <TableHead>AMC</TableHead>
            <TableHead className="text-right">SIP Amount</TableHead>
            <TableHead className="text-right">Invested</TableHead>
            <TableHead className="text-right">Current Value</TableHead>
            <TableHead className="text-right">Returns</TableHead>
            <TableHead>Status</TableHead>
            <TableHead className="w-[50px]" />
          </TableRow>
        </TableHeader>
        <TableBody>
          {data.map((sip) => (
            <TableRow key={sip.id}>
              <TableCell>
                <div>
                  <div className="font-medium text-foreground">{sip.schemeName}</div>
                  <div className="text-sm text-muted-foreground">Folio: {sip.folioNumber}</div>
                </div>
              </TableCell>
              <TableCell className="text-muted-foreground">{sip.amcName}</TableCell>
              <TableCell className="text-right font-medium">₹{sip.sipAmount.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">₹{sip.investedValue.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">₹{sip.currentValue.toLocaleString("en-IN")}</TableCell>
              <TableCell className="text-right">
                <div className={cn("font-medium", sip.returns >= 0 ? "text-green-600" : "text-red-600")}>
                  {sip.returns >= 0 ? "+" : ""}₹{sip.returns.toLocaleString("en-IN")}
                  <div className="text-xs">({sip.returnsPercentage.toFixed(2)}%)</div>
                </div>
              </TableCell>
              <TableCell>
                <Badge
                  variant={sip.status === "active" ? "default" : sip.status === "paused" ? "secondary" : "outline"}
                >
                  {sip.status}
                </Badge>
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
                      <Link href={`/mutual-funds/${sip.id}/edit`}>
                        <Pencil className="mr-2 h-4 w-4" />
                        Edit
                      </Link>
                    </DropdownMenuItem>
                    <DropdownMenuItem onClick={() => handleToggleStatus(sip.id, sip.status)}>
                      {sip.status === "active" ? (
                        <>
                          <Pause className="mr-2 h-4 w-4" />
                          Pause SIP
                        </>
                      ) : (
                        <>
                          <Play className="mr-2 h-4 w-4" />
                          Resume SIP
                        </>
                      )}
                    </DropdownMenuItem>
                    <DropdownMenuItem className="text-destructive" onClick={() => handleDelete(sip.id)}>
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
