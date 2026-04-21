import { createSlice, createAsyncThunk, type PayloadAction } from '@reduxjs/toolkit';
import type { Order, CreateOrderRequest, UpdateOrderRequest, OrderStatus } from '../types/order';
import { orderApi } from '../services/orderApi';

interface OrdersState {
  orders: Order[];
  totalCount: number;
  currentPage: number;
  pageSize: number;
  loading: boolean;
  creating: boolean;
  error: string | null;
}

const initialState: OrdersState = {
  orders: [],
  totalCount: 0,
  currentPage: 1,
  pageSize: 10,
  loading: false,
  creating: false,
  error: null,
};

export const fetchOrders = createAsyncThunk(
  'orders/fetchOrders',
  async ({ pageNumber, pageSize }: { pageNumber: number; pageSize: number }) => {
    const response = await orderApi.getOrders(pageNumber, pageSize);
    return response;
  }
);

export const createOrder = createAsyncThunk(
  'orders/createOrder',
  async (data: CreateOrderRequest) => {
    const response = await orderApi.createOrder(data);
    return response;
  }
);

export const updateOrder = createAsyncThunk<Order, UpdateOrderRequest>(
  'orders/updateOrder',
  async (data: UpdateOrderRequest) => {
    const response = await orderApi.updateOrder(data);
    return response;
  }
);

const ordersSlice = createSlice({
  name: 'orders',
  initialState,
  reducers: {
    setPage: (state, action: PayloadAction<number>) => {
      state.currentPage = action.payload;
    },
    setPageSize: (state, action: PayloadAction<number>) => {
      state.pageSize = action.payload;
      state.currentPage = 1;
    },
    clearError: (state) => {
      state.error = null;
    },
    updateOrderStatus: (state, action: PayloadAction<{ orderId: number; status: OrderStatus }>) => {
      const order = state.orders.find((o) => o.orderNumber === action.payload.orderId);
      if (order) {
        order.status = action.payload.status;
        order.updatedAt = new Date().toISOString();
      }
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchOrders.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(fetchOrders.fulfilled, (state, action) => {
        state.loading = false;
        state.orders = action.payload.items;
        state.totalCount = action.payload.totalCount;
        state.currentPage = action.payload.pageNumber;
        state.pageSize = action.payload.pageSize;
      })
      .addCase(fetchOrders.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to fetch orders';
      })
      
      .addCase(createOrder.pending, (state) => {
        state.creating = true;
        state.error = null;
      })
      .addCase(createOrder.fulfilled, (state) => {
        state.creating = false;
      })
      .addCase(createOrder.rejected, (state, action) => {
        state.creating = false;
        state.error = action.error.message || 'Failed to create order';
      })
      
      .addCase(updateOrder.pending, (state) => {
        state.loading = true;
        state.error = null;
      })
      .addCase(updateOrder.fulfilled, (state, action) => {
        state.loading = false;
        const updatedOrder = action.payload;
        const index = state.orders.findIndex((o) => o.orderNumber === updatedOrder.orderNumber);
        if (index !== -1) {
          state.orders[index] = updatedOrder;
        }
      })
      .addCase(updateOrder.rejected, (state, action) => {
        state.loading = false;
        state.error = action.error.message || 'Failed to update order';
      });
  },
});

export const { setPage, setPageSize, clearError, updateOrderStatus } = ordersSlice.actions;
export default ordersSlice.reducer;