export type OrderStatus = 
  | 0
  | 1
  | 2
  | 3;

export interface Order {
  orderNumber: number;
  description: string;
  status: OrderStatus;
  createdAt: string;
  updatedAt: string;
}

export interface OrdersResponse {
  items: Order[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

export interface CreateOrderRequest {
  description: string;
}

export interface UpdateOrderRequest {
  OrderNumber: number;
  Status: number;
}