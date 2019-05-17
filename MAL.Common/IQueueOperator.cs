using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MAL.Common
{
    public interface IQueueOperator :IDisposable
    {
        void TryConnect(string connectUri, string amqUser, string amqPassword, AutoResetEvent queueConnectEvent, int connectTimeoutMs = 10000, int connectRetryMS = 2000, bool forceReconnect = false);

        void ProduceMessage(string msgBody, IDictionary<string, string> properties);

        void AddMessageListener(string destinationUri, MessageListener messagesListener);

    }
}
