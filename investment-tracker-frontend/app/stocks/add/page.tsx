import { MainLayout } from "@/components/layout/main-layout"
import { StockForm } from "@/components/stocks/stock-form"

export default function AddStockPage() {
  return (
    <MainLayout title="Add New Stock">
      <StockForm />
    </MainLayout>
  )
}
