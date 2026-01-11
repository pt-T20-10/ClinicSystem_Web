import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import { BrowserRouter } from 'react-router-dom'
import { ConfigProvider } from 'antd'

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <BrowserRouter>
      <ConfigProvider
        theme={{
          token: {
            // Primary Color: LightBlue (Giống Desktop App)
            colorPrimary: '#03a9f4', 
            // Border Radius cho mềm mại
            borderRadius: 6,
            // Font chữ (Tùy chọn)
            fontFamily: 'Inter, -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, "Helvetica Neue", Arial',
          },
          components: {
            Button: {
              // Nút bấm sẽ dùng màu Primary
              colorPrimary: '#03a9f4',
              algorithm: true, 
            },
            Layout: {
              headerBg: '#ffffff', // Header nền trắng cho sạch
            }
          }
        }}
      >
        <App />
      </ConfigProvider>
    </BrowserRouter>
  </React.StrictMode>,
)