import React from 'react';
import toast from 'react-hot-toast';
import type { OrderStatus } from '../types/order';

const statusConfig: Record<OrderStatus, { text: string; emoji: string; color: string }> = {
  0: { text: 'создан', emoji: '🆕', color: '#f39c12' },
  1: { text: 'отправлен', emoji: '🚚', color: '#9b59b6' },
  2: { text: 'доставлен', emoji: '✅', color: '#2ecc71' },
  3: { text: 'отменён', emoji: '❌', color: '#e74c3c' },
};

export const showOrderNotification = (orderId: number, status: OrderStatus) => {
  const config = statusConfig[status];
  
  toast.custom((t) => (
    <div
      className={`toast-notification ${t.visible ? 'toast-enter' : 'toast-exit'}`}
      style={{ borderLeftColor: config.color }}
    >
      <div className="toast-emoji">{config.emoji}</div>
      <div className="toast-content">
        <div className="toast-title">Заказ #{orderId}</div>
        <div className="toast-message">
          Статус изменён на "{config.text}"
        </div>
      </div>
      <button className="toast-close" onClick={() => toast.dismiss(t.id)}>
        ✕
      </button>
    </div>
  ), {
    duration: 5000,
    position: 'top-right',
  });
};

export const showSimpleNotification = (orderId: number, status: OrderStatus) => {
  const config = statusConfig[status];
  
  toast.success(
    `Заказ #${orderId} ${config.text}`,
    {
      icon: config.emoji,
      duration: 4000,
      position: 'top-right',
      style: {
        borderLeft: `4px solid ${config.color}`,
      },
    }
  );
};

const OrderNotification: React.FC = () => {
  return null;
};

export default OrderNotification;