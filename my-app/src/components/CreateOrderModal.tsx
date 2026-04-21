import React, { useState } from 'react';
import { useAppDispatch, useAppSelector } from '../hooks';
import { createOrder } from '../features/orderSlice';
import '../styles/order/CreateOrderModal.css';

interface CreateOrderModalProps {
  isOpen: boolean;
  onClose: () => void;
  onSuccess: () => void;
}

const CreateOrderModal: React.FC<CreateOrderModalProps> = ({
  isOpen,
  onClose,
  onSuccess,
}) => {
  const dispatch = useAppDispatch();
  const { creating } = useAppSelector((state) => state.orders);
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!description.trim()) {
      setError('Введите описание заказа');
      return;
    }

    try {
      await dispatch(createOrder({ description })).unwrap();
      setDescription('');
      setError('');
      onSuccess();
      onClose();
    } catch (err) {
      setError('Не удалось создать заказ');
    }
  };

  const handleClose = () => {
    setDescription('');
    setError('');
    onClose();
  };

  if (!isOpen) return null;

  return (
    <div className="modal-overlay" onClick={handleClose}>
      <div className="modal-content" onClick={(e) => e.stopPropagation()}>
        <div className="modal-header">
          <h2>Создать новый заказ</h2>
          <button className="modal-close" onClick={handleClose}>
            ✕
          </button>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="modal-body">
            <div className="form-group">
              <label htmlFor="description">Описание заказа *</label>
              <textarea
                id="description"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Введите описание заказа..."
                rows={4}
                required
                disabled={creating}
              />
            </div>
            
            {error && <div className="error-message">{error}</div>}
          </div>

          <div className="modal-footer">
            <button
              type="button"
              className="btn-cancel"
              onClick={handleClose}
              disabled={creating}
            >
              Отмена
            </button>
            <button
              type="submit"
              className="btn-save"
              disabled={creating}
            >
              {creating ? 'Создание...' : 'Создать'}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateOrderModal;