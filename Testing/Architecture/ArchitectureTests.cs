using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NetArchTest.Rules;

namespace Testing.Architecture
{
    public class ArchitectureTests
    {
        public const string DomainNamespace = "Domain";
        public const string ApplicationNamespace = "Application";
        public const string InfrastructureNamespace = "Infrastructure";
        public const string APiNamespace = "API";


        [Fact]
        public void Domain_Should_Not_Have_Reference_To_Any_Project()
        {
            var assembly = Assembly.Load(DomainNamespace);

            var otherProjects = new[]
            {
                ApplicationNamespace, InfrastructureNamespace, APiNamespace
            };

            var testResult = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_Should_Not_Reference_Presentation()
        {
            var assembly = Assembly.Load(ApplicationNamespace);

            var otherProjects = new[]
            {
               APiNamespace
            };

            var testResult = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_Should_Not_Reference_Presentation()
        {
            var assembly = Assembly.Load(InfrastructureNamespace);

            var otherProjects = new[]
            {
               APiNamespace
            };

            var testResult = Types.InAssembly(assembly)
                .ShouldNot()
                .HaveDependencyOnAll(otherProjects)
                .GetResult();

            testResult.IsSuccessful.Should().BeTrue();
        }
    }
}
