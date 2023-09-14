using Microsoft.AspNetCore.Mvc;
using SGC_Server.Model;

namespace SGC_Server.Repositories
{
    public interface IConnectionRepository
    {
        Task<IActionResult> Add(FormConnection FormConn);




    }
}
