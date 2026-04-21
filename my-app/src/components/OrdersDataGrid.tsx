import React, { useEffect, useState } from 'react';
import { useAppSelector, useAppDispatch } from '../hooks';
import { fetchOrders, setPage, setPageSize, updateOrder } from '../features/orderSlice';
import CreateOrderModal from './CreateOrderModal';
import { signalRService } from '../services/signalRService';
import '../styles/order/OrderDataGrid.css';


const OrdersDataGrid: React.FC = () => {
  const dispatch = useAppDispatch();
  const { orders, totalCount, currentPage, pageSize, loading } = useAppSelector(
    (state) => state.orders
  );
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [updatingOrderId, setUpdatingOrderId] = useState<number | null>(null);

  useEffect(() => {
    dispatch(fetchOrders({ pageNumber: currentPage, pageSize }));
  }, [dispatch, currentPage, pageSize]);

  useEffect(() => {
    if (orders.length > 0 && signalRService.isConnected()) {
      signalRService.subscribeToAllOrders(orders);
    }
  }, [orders]);

  const totalPages = Math.ceil(totalCount / pageSize);

  const handlePageChange = (newPage: number) => {
    if (newPage >= 1 && newPage <= totalPages) {
      dispatch(setPage(newPage));
    }
  };

  const handlePageSizeChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    dispatch(setPageSize(Number(e.target.value)));
  };

  const formatDate = (dateString: string): string => {
    return new Date(dateString).toLocaleString('ru-RU');
  };

  const handleOrderCreated = () => {
    dispatch(fetchOrders({ pageNumber: currentPage, pageSize }));
  };

  const handleStatusChange = async (orderNumber: number, newStatus: number) => {
    try {
      setUpdatingOrderId(orderNumber);
      await dispatch(updateOrder({ 
        OrderNumber: orderNumber, 
        Status: newStatus as 0 | 1 | 2 | 3,
      })).unwrap();
    } catch (error) {
      console.error('Ошибка при обновлении статуса:', error);
      alert('Не удалось обновить статус заказа');
    } finally {
      setUpdatingOrderId(null);
    }
  };

  return (
    <div className="orders-data-grid">
      <div className="grid-header">
        <button className="btn-create" onClick={() => setIsModalOpen(true)}>
          + Создать заказ
        </button>
        
        <div className="page-size-selector">
          <label>Показывать по:</label>
          <select value={pageSize} onChange={handlePageSizeChange}>
            <option value={5}>5</option>
            <option value={10}>10</option>
            <option value={20}>20</option>
            <option value={50}>50</option>
          </select>
        </div>
      </div>

      {loading ? (
        <div className="loading">Загрузка...</div>
      ) : (
        <>
          <table className="orders-table">
            <thead>
              <tr>
                <th>Номер заказа</th>
                <th>Описание</th>
                <th>Статус</th>
                <th>Дата создания</th>
                <th>Дата изменения</th>
              </tr>
            </thead>
            <tbody>
              {orders.length < 1 ? (
                <tr>
                  <td colSpan={5} className="no-data">
                    Нет заказов
                  </td>
                </tr>
              ) : (
                orders?.map((order) => (
                  <tr key={order.orderNumber}>
                    <td>{order.orderNumber}</td>
                    <td className="description-cell">{order.description}</td>
                    <td>
                      <select
                        className={`status-select status-${order.status}`}
                        value={order.status}
                        onChange={(e) => handleStatusChange(
                          order.orderNumber, 
                          Number(e.target.value),
                        )}
                        disabled={updatingOrderId === order.orderNumber}
                      >
                        <option value={0}>Создан</option>
                        <option value={1}>Отправлен</option>
                        <option value={2}>Доставлен</option>
                        <option value={3}>Отменен</option>
                      </select>
                      {updatingOrderId === order.orderNumber && (
                        <span className="updating-spinner">⟳</span>
                      )}
                    </td>
                    <td>{formatDate(order.createdAt)}</td>
                    <td>{formatDate(order.updatedAt)}</td>
                  </tr>
                ))
              )}
            </tbody>
          </table>

          {totalCount > 0 && (
            <div className="pagination">
              <div className="pagination-info">
                Показано {(currentPage - 1) * pageSize + 1}-
                {Math.min(currentPage * pageSize, totalCount)} из {totalCount} заказов
              </div>
              
              <div className="pagination-controls">
                <button
                  className="pagination-btn"
                  onClick={() => handlePageChange(currentPage - 1)}
                  disabled={currentPage === 1}
                >
                  ←
                </button>
                
                <span className="page-info">
                  Страница {currentPage} из {totalPages}
                </span>
                
                <button
                  className="pagination-btn"
                  onClick={() => handlePageChange(currentPage + 1)}
                  disabled={currentPage === totalPages}
                >
                  →
                </button>
              </div>
            </div>
          )}
        </>
      )}

      <CreateOrderModal
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={handleOrderCreated}
      />
    </div>
  );
};

export default OrdersDataGrid;