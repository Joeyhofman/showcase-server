using System.Security.Claims;
using API.Requests;
using Application.Projects.Commands.AcceptInvitationCommand;
using Application.Projects.Commands.CreateDiagramCommand;
using Application.Projects.Commands.CreateProjectCommand;
using Application.Projects.Commands.DeleteDiagramCommand;
using Application.Projects.Commands.DeleteInvitationCommand;
using Application.Projects.Commands.DeleteProjectCommand;
using Application.Projects.Commands.RejectInvitationCommand;
using Application.Projects.Commands.SendInvitationCommand;
using Application.Projects.Commands.UpdateDiagramCommand;
using Application.Projects.Queries.GetAllDiagramsForProjectQuery;
using Application.Projects.Queries.GetDiagramQuery;
using Application.Projects.Queries.GetInivtationsForProjectQuery;
using Application.Projects.Queries.GetInvitationsForUserQuery;
using Application.Projects.Queries.GetProjectForUserQuery;
using Application.Users.Queries;
using Application.Users.Queries.GetUserByEmailQuery;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    
    public class ProjectsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly UserManager<User> _userManager;

        public ProjectsController(IMediator mediator, UserManager<User> userManager)
        {
            _mediator = mediator;
            _userManager = userManager;
        }

        private async Task<User> getLoggedInUser()
        {
            // Get the current user's ClaimsPrincipal
            ClaimsPrincipal userClaimsPrincipal = HttpContext.User;

            // Check if the user is authenticated
            if (!userClaimsPrincipal.Identity.IsAuthenticated)
            {
                return null;
            }

            // Get the user's ID from the claims
            string userId = _userManager.GetUserId(userClaimsPrincipal);

            // Get the user from the UserManager using the ID
            User user = await _userManager.FindByIdAsync(userId);

            // You have the current user in the 'user' variable
            return user;
        }

        [Authorize]
        [HttpPost]
        [Route("/projects")]
        public async Task<IActionResult> CreateProejct([FromBody] CreateProjectRequest createProjectRequest)
        {

            var user  = await getLoggedInUser();
            var comamnd = new CreateProjectCommand(Guid.NewGuid(), user, createProjectRequest.Name, createProjectRequest.Description);

            var createdProejct = _mediator.Send(comamnd);


            return Ok(createdProejct);
        }

        [Authorize]
        [HttpGet]
        [Route("/projects")]
        public async Task<IActionResult> GetProjects()
        {
            var projectsQuery = new GetProjectForUserQuery(await getLoggedInUser());

            var projects = _mediator.Send(projectsQuery);
            return Ok(projects);
        }

        [Authorize]
        [HttpDelete]
        [Route("/projects/{id:Guid}")]
        public async Task<IActionResult> GetProjects(Guid id)
        {
            var deleteprojectCommand = new DeleteProjectCommand(id);

            var deleted = await  _mediator.Send(deleteprojectCommand);
            return Ok(deleted);
        }

        [Authorize]
        [HttpGet]
        [Route("/projects/{projectId:Guid}/invitations")]
        public async Task<IActionResult> GetInviationsForProject(Guid projectId)
        {
            var command = new GetInivtationForProjectQuery(projectId);
            var invitations = await _mediator.Send(command);

            return Ok(invitations);
        }

        [Authorize]
        [HttpGet]
        [Route("/projects/invitations")]
        public async Task<IActionResult> getInvitationForUser()
        {
            var user = await getLoggedInUser();
            var command = new GetInivtationForProjectQuery(user.Id);
            var invitations = await _mediator.Send(command);

            return Ok(invitations);
        }


        [Authorize]
        [HttpPost]
        [Route("/projects/{projectId:Guid}/invitations")]
        public async Task<IActionResult> SendInvitation([FromBody] SendInvitationRequest request, Guid projectId)
        {

            if(request.Email is null)
            {
                return BadRequest("email cannot be null");
            }
            var user = await  _mediator.Send(new GetUserByEmailQuery(request.Email));

            var comamnd = new SendInvitationCommand(user.Id, projectId);

             await _mediator.Send(comamnd);


            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("invitations/accept")]
        public async Task<IActionResult> AcceptInvitation([FromBody] AcceptInvitationRequest request)
        {
            var comamnd = new AcceptInvitationCommand(request.InvitationId);

            await _mediator.Send(comamnd);

            return Ok();
        }

        [Authorize]
        [HttpPut]
        [Route("invitations/reject")]
        public async Task<IActionResult> RejectInvitation([FromBody] RejectInvitationRequest request)
        {
            var user = await getLoggedInUser();

            var comamnd = new RejectInvitationCommand(user.Id, request.InvitationId);

            await _mediator.Send(comamnd);

            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("/invitations")]
        public async Task<IActionResult> GetInvitationForUser()
        {
            var loggedIn = await getLoggedInUser();
            var comamnd = new GetInvitationsForUserQuery(loggedIn.Id);

            var invitations = await _mediator.Send(comamnd);

            return Ok(invitations);
        }

        [Authorize]
        [HttpDelete]
        [Route("/projects/{projectId:Guid}/invitations/{InvitationIdToDelete:Guid}")]
        public async Task<IActionResult> DeleteInvitation(Guid projectId, Guid InvitationIdToDelete)
        {
            var user = await getLoggedInUser();
            var comamnd = new DeleteInvitationCommand(user.Id, projectId, InvitationIdToDelete);

            await _mediator.Send(comamnd);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("/projects/{projectId:Guid}/diagrams/")]
        public async Task<IActionResult> GetDiagrams(Guid projectId)
        {
            var comamnd = new GetDiagramsForProjectQuery(projectId);

            var diagrams = await _mediator.Send(comamnd);
            return Ok(diagrams);
        }

        [Authorize]
        [HttpPost]
        [Route("/projects/{projectId:Guid}/diagrams/")]
        public async Task<IActionResult> CreateDiagrams(Guid projectId, [FromBody] CreateDiagramRequest request)
        {
            var comamnd = new CreateDiagramCommand(request.Name, projectId);

            await _mediator.Send(comamnd);
            return Created();
        }

        [Authorize]
        [HttpDelete]
        [Route("/projects/{projectId:Guid}/diagrams/{diagramId:Guid}")]
        public async Task<IActionResult> CreateDiagrams(Guid projectId, Guid diagramId)
        {

            var user = await getLoggedInUser();

            var comamnd = new DeleteDiagramCommand(projectId, diagramId, user.Id);

            await _mediator.Send(comamnd);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        [Route("/projects/{projectId:Guid}/diagrams/{diagramId:Guid}")]
        public async Task<IActionResult> GetDiagram(Guid projectId, Guid diagramId)
        {

            var user = await getLoggedInUser();

            var comamnd = new GetDiagramQuery(user.Id, diagramId, projectId);

            var diagram = await _mediator.Send(comamnd);
            return Ok(diagram);
        }
        
        [Authorize]
        [HttpPut]
        [Route("/projects/{projectId:Guid}/diagrams/{diagramId:Guid}")]
        public async Task<IActionResult> UpdateDiagram(Guid projectId, Guid diagramId, [FromBody] UpateDiagramRequest request)
        {

            var user = await getLoggedInUser();


            var diagram = new DataflowDiagram(
                request.Id,
                request.Name,
                request.Points,
                request.Associations
                );
            var comamnd = new UpdateDiagramCommand(user.Id, projectId, diagram);

            await _mediator.Send(comamnd);
            return Ok();
        }

    }
}
