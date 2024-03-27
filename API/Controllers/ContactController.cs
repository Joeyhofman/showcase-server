using Application.ContactForm.Commands.SendMessage;
using FluentValidation;
using Ganss.Xss;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Security.Application;

namespace API.Controllers
{
    public class ContactController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IValidator<SendMessageCommand> _validator;

        public ContactController(IMediator mediator, IValidator<SendMessageCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }

        [Route("/contact")]
        [HttpPost]
        public async Task<IActionResult> Index([FromBody] SendMessageCommand request)
        {
            
            
            var validationResult = _validator.Validate(request);



            if (!validationResult.IsValid){
                return BadRequest(validationResult.ToDictionary());
            }

            var message = await _mediator.Send(request);
            if(message is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(message);
        }
    }
}
