using Microsoft.AspNetCore.Mvc;
using SGC_Server.Model;

namespace SGC_Server.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        public Task<IActionResult> Add(FormConnection FormConn)
        {
            throw new NotImplementedException();
        }
    }
}
