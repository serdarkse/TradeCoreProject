using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Net.Sockets;

namespace TradeCore.EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        public IConnectionFactory _connectionFactory { get; }
        private IConnection _connection;
        private object lock_object = new object();
        private readonly int retryCount;
        private bool _disposed;
        private readonly List<AmqpTcpEndpoint> _endpoints;
        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, List<AmqpTcpEndpoint> endpoints, int retryCount=5)
        {
            this._connectionFactory = connectionFactory;
            this.retryCount = retryCount;
            _endpoints = endpoints;
        }
        public bool IsConnected => _connection != null && _connection.IsOpen;


        public IModel CreateModel()
        {
            return _connection.CreateModel();
        }
        public void Dispose()
        {
            _disposed = true;
            _connection.Dispose();  
        }

        public bool TryConnect()
        {
            lock(lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2,retryAttemp)),(ex,time) =>
                    {

                    }
                    );

                policy.Execute(()=>
                {
                    _connection = _connectionFactory.CreateConnection(_endpoints);    
                }
                );

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += Connection_ConnectionShutdown;
                    _connection.CallbackException += Connection_CallbackException;
                    _connection.ConnectionBlocked += Connection_ConnectionBlocked;
                    // log

                    return true;
                }

                return false;
            }
        }

        private void Connection_ConnectionBlocked(object? sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if(_disposed) return;
            TryConnect();
        }

        private void Connection_CallbackException(object? sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if(_disposed) return;
            TryConnect();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            //log Connection_ConnectionShutdown
           
            if(_disposed) return;
            TryConnect();
        }
    }
}
