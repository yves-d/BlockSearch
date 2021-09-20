using BlockSearch.Common.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace BlockSearch.ExternalClients.Tests
{
    [TestClass]
    public class NethereumClientTests
    {
        [TestMethod]
        public void When_Options_Are_Intialised_NethereumClient_Should_Not_Throw_InitialisationFailureException()
        {
            // arrange
            var action = new Action(() => new NethereumClientTestHarness().Build());

            // act & assert
            action.ShouldNotThrow();
        }

        [TestMethod]
        public void When_Options_Has_Empty_BaseUri_NethereumClient_Should_Throw_InitialisationFailureException()
        {
            // arrange
            var action = new Action(() => new NethereumClientTestHarness().WithEmptyBaseUri().Build());

            // act & assert
            action.ShouldThrow<InitialisationFailureException>();
        }

        [TestMethod]
        public void When_Options_Has_Empty_ProjectId_NethereumClient_Should_Throw_InitialisationFailureException()
        {
            // arrange
            var action = new Action(() => new NethereumClientTestHarness().WithEmptyProjectId().Build());

            // act & assert
            action.ShouldThrow<InitialisationFailureException>();
        }
    }
}
