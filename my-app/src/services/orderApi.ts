import type { Order, OrdersResponse, CreateOrderRequest, UpdateOrderRequest } from '../types/order';
import config from '../config';


export const orderApi = {

   apiUrl: config.apiUrl,

  getOrders: async (pageNumber: number = 1, pageSize: number = 10): Promise<OrdersResponse> => {
    const response = await fetch(
      `${orderApi.apiUrl}Order?PageNumber=${pageNumber}&PageSize=${pageSize}`
    );
    
    if (!response.ok) {
      throw new Error('Failed to fetch orders');
    }
    
    return response.json();
  },

  createOrder: async (data: CreateOrderRequest): Promise<Order> => {
    const response = await fetch(`${orderApi.apiUrl}Order`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error('Failed to create order');
    }

    return response.json();
  },

  updateOrder: async (data: UpdateOrderRequest): Promise<Order> => {
    const response = await fetch(`${config.apiUrl}Order`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      throw new Error('Failed to update order');
    }

    return response.json();
  },

  getOrderById: async (id: string): Promise<Order> => {
    const response = await fetch(`${orderApi.apiUrl}Order/${id}`);
    
    if (!response.ok) {
      throw new Error('Failed to fetch order');
    }
    
    return response.json();
  },
};