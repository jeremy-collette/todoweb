using System;
using System.Collections.Generic;
using System.Text;

namespace todoweb.Client
{
    public static class ClientFactory
    {
        public static ITodoClient CreateTodoClient(System.Net.Http.HttpClient httpClient)
        {
            return new TodoClient(new ClientConfiguration { BaseUrl = httpClient.BaseAddress.GetLeftPart(UriPartial.Authority) }, httpClient);
        }

        public static IUserClient CreateUserClient(System.Net.Http.HttpClient httpClient)
        {
            return new UserClient(new ClientConfiguration { BaseUrl = httpClient.BaseAddress.GetLeftPart(UriPartial.Authority) }, httpClient);
        }
    }
}
