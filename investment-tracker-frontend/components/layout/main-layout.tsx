import type React from "react"
import { Sidebar } from "./sidebar"
import { Header } from "./header"

interface MainLayoutProps {
  children: React.ReactNode
  title: string
}

export function MainLayout({ children, title }: MainLayoutProps) {
  return (
    <div className="min-h-screen bg-background">
      <Sidebar />
      <div className="pl-64">
        <Header title={title} />
        <main className="p-6">{children}</main>
      </div>
    </div>
  )
}
