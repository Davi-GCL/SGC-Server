using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using SGC_Server.Model;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Xml.Linq;


namespace SGC_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectionController : ControllerBase
    {
        

        [HttpPost("/connect")]
        public IActionResult Connect([FromBody] FormConnection formConnection)
        {
            return Ok();
        }
    }
}
//"Data Source=OPERACIONAL39\\SQLEXPRESS;Initial Catalog=sistema_banco;Persist Security Info=True;User ID=sa;Password=root"