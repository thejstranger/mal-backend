using MAL.Common;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace MAL.Email
{
    class Program
    {
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private static IEmailQueueConsumer _emailQueueConsumer;

        public static void Main(string[] args)
        {
            var serviceCollection = ConfigureServices();
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Start queue consumer that sends emails.
            _emailQueueConsumer = serviceProvider.GetService<IEmailQueueConsumer>();

            Console.CancelKeyPress += OnExit;
            _closing.WaitOne();
        }

        private static void OnExit(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("Email app exited by cancel key press.");
            _emailQueueConsumer.Dispose();
            _closing.Set();
        }

        private static ServiceCollection ConfigureServices()
        {
            var srvCollection = new ServiceCollection();
            srvCollection.AddSingleton<IConfigProvider, ConfigProvider>();
            srvCollection.AddTransient<IQueueOperator, AMQOperator>();
            srvCollection.AddTransient<IEmailQueueConsumer, EmailQueueConsumer>();

            srvCollection.AddTransient<CancellationTokenSource, CancellationTokenSource>();

            return srvCollection;
        }
    }
}
