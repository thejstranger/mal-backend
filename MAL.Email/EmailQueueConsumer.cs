using MAL.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Apache.NMS;
using System.Threading;

namespace MAL.Email
{
    public class EmailQueueConsumer : IEmailQueueConsumer
    {
        public EmailQueueConsumer(IConfigProvider configProvider, IQueueOperator queueOperator)
        {
            string connectUri = $"activemq:tcp://{configProvider.GetActiveMqHost()}:{configProvider.GetActiveMqPort()}";

            AutoResetEvent queueConnectedEvent = new AutoResetEvent(false);

            queueOperator.TryConnect(connectUri, configProvider.GetActiveMqUser(), configProvider.GetActiveMqPassword(),
                queueConnectedEvent,-1, 2000);

            if (queueConnectedEvent.WaitOne())
            {
                queueOperator.AddMessageListener("queue://Email", MessagesListener);
            }
            else
            {
                Cancel();
            }
        }

        private void MessagesListener(IMessage message)
        {
            ITextMessage textMessage = message as ITextMessage;

            Console.WriteLine(textMessage.Text);

            message.Acknowledge();
        }

        public void Cancel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
