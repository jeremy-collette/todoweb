namespace todoweb.Client
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ClientBase
    {
        public string BaseUrl { get; set; }

        public ClientBase(ClientConfiguration configuration)
        {
            this.BaseUrl = configuration.BaseUrl;
        }
    }
}
