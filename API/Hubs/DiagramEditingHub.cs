using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Claims;
using AngleSharp.Common;
using Domain.Entities;
using Domain.Exceptions.DataflowDiagrams;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace API.Hubs
{
    public class DiagramEditingHub : Hub
    {

        private readonly IDataflowDiagramRepository _dataflowDiagramRepository;
        private readonly UserManager<User> _userManager;
        private readonly IProjectRepository _projectRepository;

        private static readonly Dictionary<string, DataflowDiagram> _sessions = new Dictionary<string, DataflowDiagram>();

        public DiagramEditingHub(IProjectRepository projectRepository, IDataflowDiagramRepository dataflowDiagramRepository, UserManager<User> userManager)
        {

            _projectRepository = projectRepository;
            _dataflowDiagramRepository = dataflowDiagramRepository;
            _userManager = userManager;
        }

        public async Task CreatePoint(string groupName, DataflowPoint point, DataflowAssociation association)
        {
            var diagram = _sessions[groupName];

            diagram.AddPoint(point);
            diagram.AddAssociation(association);
            await Clients.Group(groupName).SendAsync("createPoint", point, association);
        }


        public async Task UpdatePoint(string groupName, int index, string name, string color)
        {
            var diagram = _sessions[groupName];
            diagram.UpdatePoint(index, name, color);
            var updatedPoint = diagram.Points.ElementAt(index);
            await Clients.Group(groupName).SendAsync("updatePoint", index, updatedPoint);
        }

        public async Task CreateStartingPoint(string groupName)
        {
            var diagram = _sessions[groupName];
            var point = new DataflowPoint
            {
                Id = Guid.NewGuid(),
                X = 100,
                Y = 100,
                Width = 150,
                Height = 150,
                Classname = "Data Point",
                Color = "red",
                fillColor = "red",
                InputValue = "Data Point",
                isEditing = false
            };
            diagram.AddPoint(point);
            await Clients.Group(groupName).SendAsync("createStartingPoint", point);
        }
        
        public async Task DeletePoint(string groupName, int index)
        {
            var diagram = _sessions[groupName];
            var point = diagram.Points.ElementAt(index);
            diagram.RemovePoint(point);
            await Clients.Group(groupName).SendAsync("deletePoint", index);
        }

        public async Task SetupEditing(string projectId, string diagramId)
        {
            try
            {
                string groupName = $"{projectId}-{diagramId}";

                await Groups.AddToGroupAsync(Context.ConnectionId, groupName);

                var diagram = await _dataflowDiagramRepository.GetById(Guid.Parse(diagramId));
                if(diagram is null)
                {
                    throw new DataflowDiagramNotFoundException("could not find dataflow diagram");
                }


                if(!_sessions.ContainsKey(groupName))
                {
                    _sessions.Add(groupName, diagram);
                    await Clients.All.SendAsync("DiagramSync", diagram);
                }
                else
                {
                    await Clients.All.SendAsync("DiagramSync", _sessions[groupName]);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync("test");
                throw ex;
            }

        }

        private async Task<User> getLoggedInUser()
        {
            // Get the current user's ClaimsPrincipal
            ClaimsPrincipal userClaimsPrincipal = Context.User;

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

        public async override Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }
}
