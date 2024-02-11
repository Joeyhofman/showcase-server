using Application.ContactForm.Commands.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMediator _mediator;

        public ContactController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("/contact")]
        [HttpPost]
        public async Task<IActionResult> Index(SendMessageCommand request)
        {
            var message = await _mediator.Send(request);
            if(message is null)
            {
                return BadRequest();
            }

            return Ok(message);
        }
    }
}
