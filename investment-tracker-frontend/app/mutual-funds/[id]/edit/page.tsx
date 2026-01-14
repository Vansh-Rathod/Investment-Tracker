import { MainLayout } from "@/components/layout/main-layout"
import { SIPForm } from "@/components/mutual-funds/sip-form"
import { mockMutualFunds } from "@/data/mock-data"

interface EditSIPPageProps {
  params: Promise<{ id: string }>
}

export default async function EditSIPPage({ params }: EditSIPPageProps) {
  const { id } = await params
  // In real app, fetch from API
  const sip = mockMutualFunds.find((mf) => mf.id === id)

  if (!sip) {
    return (
      <MainLayout title="SIP Not Found">
        <div className="text-center py-12">
          <p className="text-muted-foreground">The SIP you are looking for does not exist.</p>
        </div>
      </MainLayout>
    )
  }

  return (
    <MainLayout title="Edit SIP">
      <SIPForm initialData={sip} />
    </MainLayout>
  )
}
