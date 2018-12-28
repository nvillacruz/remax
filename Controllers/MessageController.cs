using Microsoft.AspNetCore.Mvc;

namespace Remax.Web.Server
{

    /// <summary>
    /// 
    /// </summary>
    [Produces("application/json")]
    [Route("api/message")]

    public class MessageController : Controller
    {

        //private IHubContext<MessageHub> _messageHubContext;

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="messageHubContext"></param>
        //public MessageController(IHubContext<MessageHub> messageHubContext)
        //{
        //    _messageHubContext = messageHubContext;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //public IActionResult Post()
        //{

        //    _messageHubContext.Clients.All.SendAsync("send", "Hello from the server");
        //    return Ok();
        //}
    }
}
