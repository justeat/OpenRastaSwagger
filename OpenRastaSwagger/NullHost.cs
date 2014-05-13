using System;
using OpenRasta;
using OpenRasta.Configuration;
using OpenRasta.DI;
using OpenRasta.Hosting;
using OpenRasta.Hosting.InMemory;
using OpenRasta.Pipeline;

namespace OpenRastaSwagger
{
    public class NullHost : IHost, IDependencyResolverAccessor, IDisposable
    {
        private readonly IConfigurationSource _configuration;
        private bool _isDisposed;

        public string ApplicationVirtualPath { get; set; }
        public HostManager HostManager { get; private set; }
        public IDependencyResolver Resolver { get; private set; }
        
        IDependencyResolverAccessor IHost.ResolverAccessor { get { return this; } }
        
        public event EventHandler<IncomingRequestProcessedEventArgs> IncomingRequestProcessed;
        public event EventHandler<IncomingRequestReceivedEventArgs> IncomingRequestReceived;
        public event EventHandler Start;
        public event EventHandler Stop;

        public NullHost(IConfigurationSource configuration, string path = "/")
        {
            _configuration = configuration;
            Resolver = new InternalDependencyResolver();
            ApplicationVirtualPath = path;
            HostManager = HostManager.RegisterHost(this);
            RaiseStart();
        }

        public void Close()
        {
            RaiseStop();
            HostManager.UnregisterHost(this);
            _isDisposed = true;
        }
        
        void IDisposable.Dispose()
        {
            Close();
        }

        bool IHost.ConfigureLeafDependencies(IDependencyResolver resolver)
        {
            CheckNotDisposed();
            return true;
        }

        bool IHost.ConfigureRootDependencies(IDependencyResolver resolver)
        {
            CheckNotDisposed();
            resolver.AddDependencyInstance<IContextStore>(new InMemoryContextStore());
            
            if (_configuration != null)
            {
                Resolver.AddDependencyInstance<IConfigurationSource>(_configuration, DependencyLifetime.Singleton);
            }

            return true;
        }

        protected virtual void RaiseStart()
        {
            Start.Raise(this, EventArgs.Empty);
        }

        protected virtual void RaiseStop()
        {
            Stop.Raise(this, EventArgs.Empty);
        }

        private void CheckNotDisposed()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("HttpListenerHost");
            }
        }
    }
}