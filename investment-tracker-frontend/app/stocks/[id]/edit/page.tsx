import { MainLayout } from "@/components/layout/main-layout"
import { StockForm } from "@/components/stocks/stock-form"
import { mockStocks } from "@/data/mock-data"

interface EditStockPageProps {
  params: Promise<{ id: string }>
}

export default async function EditStockPage({ params }: EditStockPageProps) {
  const { id } = await params
  // In real app, fetch from API
  const stock = mockStocks.find((s) => s.id === id)

  if (!stock) {
    return (
      <MainLayout title="Stock Not Found">
        <div className="text-center py-12">
          <p className="text-muted-foreground">The stock you are looking for does not exist.</p>
        </div>
      </MainLayout>
    )
  }

  return (
    <MainLayout title="Edit Stock">
      <StockForm initialData={stock} />
    </MainLayout>
  )
}
