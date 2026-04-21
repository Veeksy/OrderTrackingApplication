const config = {
  apiUrl: import.meta.env.VITE_API_BASE_URL ?? 'http://localhost:5000',
  hubUrl: import.meta.env.VITE_HUB_BASE_URL ?? 'http://localhost:5001/hubs/notification',
}

export default config;