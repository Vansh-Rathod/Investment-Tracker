import { MainLayout } from "@/components/layout/main-layout"
import { SummaryCards } from "@/components/dashboard/summary-cards"
import { AllocationChart } from "@/components/dashboard/allocation-chart"
import { PerformanceChart } from "@/components/dashboard/performance-chart"
import { mockPortfolioSummary, mockAllocationData, mockPerformanceData, mockSectorAllocation } from "@/data/mock-data"

export default function DashboardPage() {
  return (
    <MainLayout title="Dashboard">
      <div className="space-y-6">
        <SummaryCards summary={mockPortfolioSummary} />

        <div className="grid gap-6 lg:grid-cols-2">
          <PerformanceChart data={mockPerformanceData} />
          <AllocationChart data={mockAllocationData} title="Asset Allocation" />
        </div>

        <div className="grid gap-6 lg:grid-cols-2">
          <AllocationChart data={mockSectorAllocation} title="Sector Allocation (Stocks)" />
        </div>
      </div>
    </MainLayout>
  )
}
