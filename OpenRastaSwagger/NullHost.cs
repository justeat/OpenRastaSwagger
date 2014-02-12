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

        IDependencyResolverAccessor IHost.ResolverAccessor
        {
            get
            {
                return (IDependencyResolverAccessor)this;
            }
        }

        public event EventHandler<IncomingRequestProcessedEventArgs> IncomingRequestProcessed;

        public event EventHandler<IncomingRequestReceivedEventArgs> IncomingRequestReceived;

        public event EventHandler Start;

        public event EventHandler Stop;

        public NullHost(IConfigurationSource configuration, string path="/")
        {
            this._configuration = configuration;
            this.Resolver = (IDependencyResolver)new InternalDependencyResolver();
            this.ApplicationVirtualPath = path;
            this.HostManager = HostManager.RegisterHost((IHost)this);
            this.RaiseStart();
        }

        public void Close()
        {
            this.RaiseStop();
            HostManager.UnregisterHost((IHost)this);
            this._isDisposed = true;
        }

        //public IResponse ProcessRequest(IRequest request)
        //{
        //    this.CheckNotDisposed();
        //    AmbientContext ambientContext = new AmbientContext();
        //    InMemoryCommunicationContext communicationContext = new InMemoryCommunicationContext()
        //    {
        //        ApplicationBaseUri = new Uri("http://localhost"),
        //        Request = request,
        //        Response = (IResponse)new InMemoryResponse()
        //    };
        //    try
        //    {
        //        using (new ContextScope((object)ambientContext))
        //            this.RaiseIncomingRequestReceived((ICommunicationContext)communicationContext);
        //    }
        //    finally
        //    {
        //        using (new ContextScope((object)ambientContext))
        //            this.RaiseIncomingRequestProcessed((ICommunicationContext)communicationContext);
        //    }
        //    return communicationContext.Response;
        //}

        void IDisposable.Dispose()
        {
            this.Close();
        }

        bool IHost.ConfigureLeafDependencies(IDependencyResolver resolver)
        {
            this.CheckNotDisposed();
            return true;
        }

        bool IHost.ConfigureRootDependencies(IDependencyResolver resolver)
        {
            this.CheckNotDisposed();
            DependencyResolverExtensions.AddDependencyInstance<IContextStore>(resolver, (object)new InMemoryContextStore());
            if (this._configuration != null)
                DependencyResolverExtensions.AddDependencyInstance<IConfigurationSource>(this.Resolver, (object)this._configuration, DependencyLifetime.Singleton);
            return true;
        }

        //protected virtual void RaiseIncomingRequestProcessed(ICommunicationContext context)
        //{
        //    EventHandlerExtensions.Raise<IncomingRequestProcessedEventArgs>(this.IncomingRequestProcessed, (object)this, new IncomingRequestProcessedEventArgs(context));
        //}

        //protected virtual void RaiseIncomingRequestReceived(ICommunicationContext context)
        //{
        //    EventHandlerExtensions.Raise<IncomingRequestReceivedEventArgs>(this.IncomingRequestReceived, (object)this, new IncomingRequestReceivedEventArgs(context));
        //}

        protected virtual void RaiseStart()
        {
            EventHandlerExtensions.Raise(this.Start, (object)this, EventArgs.Empty);
        }

        protected virtual void RaiseStop()
        {
            EventHandlerExtensions.Raise(this.Stop, (object)this, EventArgs.Empty);
        }

        private void CheckNotDisposed()
        {
            if (this._isDisposed)
                throw new ObjectDisposedException("HttpListenerHost");
        }
    }
}