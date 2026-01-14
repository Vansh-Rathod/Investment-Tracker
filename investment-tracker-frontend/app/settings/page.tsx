import { MainLayout } from "@/components/layout/main-layout"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"
import { Switch } from "@/components/ui/switch"

export default function SettingsPage() {
  return (
    <MainLayout title="Settings">
      <div className="max-w-2xl space-y-6">
        <Card>
          <CardHeader>
            <CardTitle>API Configuration</CardTitle>
            <CardDescription>Configure your .NET Web API backend connection</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label htmlFor="apiUrl">API Base URL</Label>
              <Input id="apiUrl" placeholder="https://api.yourserver.com" defaultValue="https://api.example.com" />
            </div>
            <div className="space-y-2">
              <Label htmlFor="timeout">Request Timeout (ms)</Label>
              <Input id="timeout" type="number" defaultValue="10000" />
            </div>
            <Button>Save Configuration</Button>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Notifications</CardTitle>
            <CardDescription>Manage your notification preferences</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="font-medium">SIP Reminders</p>
                <p className="text-sm text-muted-foreground">Get notified before SIP deduction dates</p>
              </div>
              <Switch defaultChecked />
            </div>
            <div className="flex items-center justify-between">
              <div>
                <p className="font-medium">Price Alerts</p>
                <p className="text-sm text-muted-foreground">Get notified on significant price changes</p>
              </div>
              <Switch defaultChecked />
            </div>
            <div className="flex items-center justify-between">
              <div>
                <p className="font-medium">Weekly Summary</p>
                <p className="text-sm text-muted-foreground">Receive weekly portfolio summary emails</p>
              </div>
              <Switch />
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardHeader>
            <CardTitle>Display Preferences</CardTitle>
            <CardDescription>Customize how your portfolio is displayed</CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="flex items-center justify-between">
              <div>
                <p className="font-medium">Show Returns in Percentage</p>
                <p className="text-sm text-muted-foreground">Display returns as percentage instead of absolute value</p>
              </div>
              <Switch defaultChecked />
            </div>
            <div className="flex items-center justify-between">
              <div>
                <p className="font-medium">Dark Mode</p>
                <p className="text-sm text-muted-foreground">Use dark theme for the application</p>
              </div>
              <Switch />
            </div>
          </CardContent>
        </Card>
      </div>
    </MainLayout>
  )
}
