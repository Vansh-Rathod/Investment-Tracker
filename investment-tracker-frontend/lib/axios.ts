import axios, { type AxiosError, type InternalAxiosRequestConfig } from "axios"

// Create axios instance with base configuration
const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL || "https://api.example.com",
  timeout: 10000,
  headers: {
    "Content-Type": "application/json",
  },
})

// Request interceptor for JWT authentication
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    // Get token from localStorage (in real app, use secure storage)
    const token = typeof window !== "undefined" ? localStorage.getItem("authToken") : null

    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  },
  (error: AxiosError) => {
    return Promise.reject(error)
  },
)

// Response interceptor for error handling
api.interceptors.response.use(
  (response) => response,
  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean }

    // Handle 401 Unauthorized - token expired
    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true

      try {
        // Attempt to refresh token
        const refreshToken = localStorage.getItem("refreshToken")
        if (refreshToken) {
          const response = await axios.post(`${process.env.NEXT_PUBLIC_API_URL}/auth/refresh`, { refreshToken })
          const { accessToken } = response.data
          localStorage.setItem("authToken", accessToken)

          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${accessToken}`
          }
          return api(originalRequest)
        }
      } catch (refreshError) {
        // Redirect to login if refresh fails
        localStorage.removeItem("authToken")
        localStorage.removeItem("refreshToken")
        window.location.href = "/login"
      }
    }

    return Promise.reject(error)
  },
)

export default api
