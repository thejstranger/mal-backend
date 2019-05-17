using Apache.NMS;
using Apache.NMS.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MAL.Common
{
    public class AMQOperator : IQueueOperator
    {
        private IConnection _queueConnection;

        public void TryConnect(string connectUri, string amqUser, string amqPassword, 
            AutoResetEvent queueConnectEvent, int connectTimeoutMs = 10000, int connectRetryMS = 2000, bool forceReconnect = false)
        {
            Task.Factory.StartNew(() => ConnectThread(connectUri, amqUser, amqPassword, 
                queueConnectEvent, connectTimeoutMs, connectRetryMS, forceReconnect));
        }

        private void ConnectThread(string connectUri, string amqUser, string amqPassword, AutoResetEvent connectEvent, int connectTimeoutMs, int connectRetryMs, bool forceReconnect)
        {
            Stopwatch sw = new Stopwatch();
            if (connectTimeoutMs >= 0)
            {
                sw.Start();
            }
            while (sw.Elapsed.TotalSeconds < connectTimeoutMs || connectTimeoutMs <= -1)
            {
                if (forceReconnect)
                {
                    _queueConnection?.Close();
                }

                if (!_queueConnection?.IsStarted ?? true)
                {
                    if (Connect(connectUri, amqUser, amqPassword))
                    {
                        Console.WriteLine($"Connected to {connectUri}.");
                        connectEvent.Set();
                        sw.Stop();
                        break;
                    }
                }

                Console.WriteLine($"Could not connect to {connectUri}. Retrying in {connectRetryMs}ms.");
                Thread.Sleep(connectRetryMs);
            }
        }

        private bool Connect(string uri, string amqUser, string amqPassword)
        {
            try
            {
                Uri connectUri = new Uri(uri);
                IConnectionFactory factory = new NMSConnectionFactory(connectUri);
                _queueConnection = factory.CreateConnection(amqUser, amqPassword);

                _queueConnection.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

            return true;
        }

        public void ProduceMessage(string msgBody, IDictionary<string, string> properties = null)
        {
            using (ISession session = _queueConnection.CreateSession(AcknowledgementMode.ClientAcknowledge))
            {
                IDestination destination = SessionUtil.GetDestination(session, "queue://Email");

                using (IMessageProducer producer = session.CreateProducer(destination))
                {
                    producer.DeliveryMode = MsgDeliveryMode.Persistent;

                    ITextMessage request = session.CreateTextMessage(msgBody);

                    foreach (var kv in properties)
                    {
                        request.Properties[kv.Key] = kv.Value;
                    }

                    producer.Send(request);
                }
            }
        }

        public void AddMessageListener(string destinationUri, MessageListener messagesListener)
        {
            ISession session = _queueConnection.CreateSession();
            IDestination destination = SessionUtil.GetDestination(session, destinationUri);

            IMessageConsumer consumer = session.CreateConsumer(destination);

            consumer.Listener += messagesListener;
        }

        public void Dispose()
        {
            _queueConnection?.Dispose();
        }
    }
}
