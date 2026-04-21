import OrdersDataGrid from './components/OrdersDataGrid';
import ConnectionStatus from './components/ConnectionStatus';
import './App.css';
import { Toaster } from 'react-hot-toast';

function App() {
  return (
    <div className="App">
      <Toaster />
      <header className="app-header">
        <h1>Система отслеживания заказов</h1>
        <ConnectionStatus />
      </header>
      <main className="app-main">
        <OrdersDataGrid />
      </main>
    </div>
  );
}

export default App;