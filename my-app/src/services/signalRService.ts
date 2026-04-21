import * as signalR from '@microsoft/signalr';
import config from '../config';
import type { OrderStatus } from '../types/order';
import { store } from '../store';
import { updateOrderStatus } from '../features/orderSlice';
import { showSimpleNotification } from '../components/OrderNotification';
import toast from 'react-hot-toast';

class SignalRService {
  private connection: signalR.HubConnection | null = null;
  private reconnectAttempts = 0;
  private maxReconnectAttempts = 5;
  private subscribedOrders: Set<string> = new Set();

  async startConnection(): Promise<void> {
    if (this.connection) {
      console.log('SignalR connection already exists');
      return;
    }

    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(config.hubUrl)
      .withAutomaticReconnect({
        nextRetryDelayInMilliseconds: (retryContext) => {
          this.reconnectAttempts++;
          if (this.reconnectAttempts > this.maxReconnectAttempts) {
            console.error('Max reconnect attempts reached');
            toast.error('Не удалось подключиться к серверу уведомлений');
            return null;
          }
          return Math.min(1000 * Math.pow(2, retryContext.previousRetryCount), 30000);
        }
      })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    this.registerHandlers();

    try {
      await this.connection.start();
      console.log('SignalR Connected successfully');
      this.reconnectAttempts = 0;
      
      await this.resubscribeAll();
      
      toast.success('Подключено к серверу уведомлений', {
        icon: '🔔',
        duration: 2000,
      });
    } catch (err) {
      console.error('SignalR Connection Error:', err);
      toast.error('Ошибка подключения к серверу уведомлений');
      setTimeout(() => this.startConnection(), 5000);
    }
  }

    async subscribeToOrder(orderNumber: string | number): Promise<void> {
    const orderNumberStr = orderNumber.toString();
    
    if (this.connection && this.isConnected()) {
      try {
        await this.connection.invoke('SubscribeToOrder', orderNumberStr);
        this.subscribedOrders.add(orderNumberStr);
        console.log(`📡 Subscribed to order ${orderNumberStr}`);
      } catch (err) {
        console.error(`Error subscribing to order ${orderNumberStr}:`, err);
      }
    } else {
      console.warn('Cannot subscribe: SignalR not connected');
      this.subscribedOrders.add(orderNumberStr);
    }
  }

  async unsubscribeFromOrder(orderNumber: string | number): Promise<void> {
    const orderNumberStr = orderNumber.toString();
    
    if (this.connection && this.isConnected()) {
      try {
        await this.connection.invoke('UnsubscribeFromOrder', orderNumberStr);
        this.subscribedOrders.delete(orderNumberStr);
        console.log(`📡 Unsubscribed from order ${orderNumberStr}`);
      } catch (err) {
        console.error(`Error unsubscribing from order ${orderNumberStr}:`, err);
      }
    } else {
      this.subscribedOrders.delete(orderNumberStr);
    }
  }

  private async resubscribeAll(): Promise<void> {
    const orders = Array.from(this.subscribedOrders);
    for (const orderNumber of orders) {
      await this.subscribeToOrder(orderNumber);
    }
  }

  subscribeToAllOrders(orders: Array<{ orderNumber: number | string }>): void {
    orders.forEach(order => {
      this.subscribeToOrder(order.orderNumber);
    });
  }

  private registerHandlers(): void {
    if (!this.connection) return;

    this.connection.on('ReceiveOrderStatus', (data: { orderNumber: string; status: OrderStatus; updatedAt: string }) => {
      console.log('Order status updated:', data);
      
      store.dispatch(updateOrderStatus({ 
        orderId: parseInt(data.orderNumber), 
        status: data.status,
      }));
      
      showSimpleNotification(parseInt(data.orderNumber), data.status);
      
      this.showBrowserNotification({
        orderId: parseInt(data.orderNumber),
        status: data.status
      });
    });

    this.connection.onreconnecting((error) => {
      console.log('SignalR reconnecting...', error);
      toast.loading('Переподключение к серверу...', {
        id: 'reconnecting',
      });
    });

    this.connection.onreconnected((connectionId) => {
      console.log('SignalR reconnected. ConnectionId:', connectionId);
      this.reconnectAttempts = 0;
      toast.success('Соединение восстановлено', {
        id: 'reconnected',
        icon: '✅',
        duration: 3000,
      });
    });

    this.connection.onclose((error) => {
      console.log('SignalR connection closed:', error);
      if (error) {
        toast.error('Соединение с сервером уведомлений потеряно');
      }
    });
  }

  private showBrowserNotification(data: { orderId: number; status: OrderStatus }): void {
    const statusTexts: Record<OrderStatus, string> = {
      0: 'создан',
      1: 'отправлен',
      2: 'доставлен',
      3: 'отменён',
    };

    if ('Notification' in window && Notification.permission === 'granted') {
      new Notification('Обновление заказа', {
        body: `Заказ #${data.orderId} ${statusTexts[data.status]}`,
        icon: '/favicon.ico',
      });
    }
  }

  async stopConnection(): Promise<void> {
    if (this.connection) {
      await this.connection.stop();
      this.connection = null;
      console.log('SignalR Disconnected');
    }
  }

  isConnected(): boolean {
    return this.connection?.state === signalR.HubConnectionState.Connected;
  }

  async sendMessage(methodName: string, ...args: any[]): Promise<void> {
    if (this.connection && this.isConnected()) {
      try {
        await this.connection.send(methodName, ...args);
      } catch (err) {
        console.error(`Error sending ${methodName}:`, err);
      }
    } else {
      console.warn('Cannot send message: SignalR not connected');
    }
  }
}

export const signalRService = new SignalRService();