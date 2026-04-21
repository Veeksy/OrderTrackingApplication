import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { Provider } from 'react-redux'
import { store } from './store'
import { signalRService } from './services/signalRService'
import './index.css'
import App from './App'

if ('Notification' in window) {
  Notification.requestPermission()
}

signalRService.startConnection()

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <App />
    </Provider>
  </StrictMode>,
)