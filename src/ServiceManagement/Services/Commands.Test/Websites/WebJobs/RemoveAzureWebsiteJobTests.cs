﻿// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using System.Management.Automation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Commands.Test.Utilities.Websites;
using Microsoft.WindowsAzure.Commands.Utilities.Websites;
using Microsoft.WindowsAzure.Commands.Websites.WebJobs;
using Microsoft.WindowsAzure.WebSitesExtensions.Models;
using Moq;

namespace Microsoft.WindowsAzure.Commands.Test.Websites
{
    [TestClass]
    public class RemoveAzureWebsiteJobTests : WebsitesTestBase
    {
        private const string websiteName = "website1";

        private const string slot = "staging";

        private Mock<IWebsitesClient> websitesClientMock;

        private RemoveAzureWebsiteJobCommand cmdlet; 

        private Mock<ICommandRuntime> commandRuntimeMock;

        [TestInitialize]
        public override void SetupTest()
        {
            websitesClientMock = new Mock<IWebsitesClient>();
            commandRuntimeMock = new Mock<ICommandRuntime>();
            cmdlet = new RemoveAzureWebsiteJobCommand()
            {
                CommandRuntime = commandRuntimeMock.Object,
                WebsitesClient = websitesClientMock.Object,
                Name = websiteName,
                Slot = slot,
                Force = true
            };
        }

        [TestMethod]
        public void DeletesTriggeredWebJob()
        {
            // Setup
            string jobName = "myWebJob";
            WebJobType jobType = WebJobType.Triggered;
            websitesClientMock.Setup(f => f.DeleteWebJob(websiteName, slot, jobName, jobType)).Verifiable();
            cmdlet.JobName = jobName;
            cmdlet.JobType = jobType;
            commandRuntimeMock.Setup(f => f.ShouldProcess(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            websitesClientMock.Verify(f => f.DeleteWebJob(websiteName, slot, jobName, jobType), Times.Once());
            commandRuntimeMock.Verify(f => f.WriteObject(true), Times.Once());
        }

        [TestMethod]
        public void DeletesContinuousWebJob()
        {
            // Setup
            string jobName = "myWebJob";
            WebJobType jobType = WebJobType.Continuous;
            websitesClientMock.Setup(f => f.DeleteWebJob(websiteName, slot, jobName, jobType)).Verifiable();
            cmdlet.JobName = jobName;
            cmdlet.JobType = jobType;
            commandRuntimeMock.Setup(f => f.ShouldProcess(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

            // Test
            cmdlet.ExecuteCmdlet();

            // Assert
            websitesClientMock.Verify(f => f.DeleteWebJob(websiteName, slot, jobName, jobType), Times.Once());
            commandRuntimeMock.Verify(f => f.WriteObject(true), Times.Once());
        }
    }
}
