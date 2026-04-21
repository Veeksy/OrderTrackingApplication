import React, { useEffect, useState } from 'react';
import { signalRService } from '../services/signalRService';
import '../styles/signalR/ConnectionStatus.css';

const ConnectionStatus: React.FC = () => {
  const [isConnected, setIsConnected] = useState(false);

  useEffect(() => {
    const checkConnection = setInterval(() => {
      setIsConnected(signalRService.isConnected());
    }, 1000);

    return () => clearInterval(checkConnection);
  }, []);

  return (
    <div className={`connection-status ${isConnected ? 'connected' : 'disconnected'}`}>
      <span className="status-dot"></span>
      <span className="status-text">
        {isConnected ? 'Онлайн' : 'Офлайн'}
      </span>
    </div>
  );
};

export default ConnectionStatus;