import { MainLayout } from "@/components/layout/main-layout"
import { SIPForm } from "@/components/mutual-funds/sip-form"

export default function AddSIPPage() {
  return (
    <MainLayout title="Add New SIP">
      <SIPForm />
    </MainLayout>
  )
}
