﻿using System.Threading.Tasks;
using Geev.BackgroundJobs;
using Shouldly;
using Xunit;

namespace Geev.Tests.BackgroundJobs
{
    public class InMemoryBackgroundJobStore_Tests
    {
        private readonly InMemoryBackgroundJobStore _store;

        public InMemoryBackgroundJobStore_Tests()
        {
            _store = new InMemoryBackgroundJobStore();
        }

        [Fact]
        public async Task Test_All()
        {
            var jobInfo = new BackgroundJobInfo
            {
                JobType = "TestType",
                JobArgs = "{}"
            };
            
            await _store.InsertAsync(jobInfo);
            (await _store.GetWaitingJobsAsync(1000)).Count.ShouldBe(1);

            var jobInfoFromStore = await _store.GetAsync(1);
            jobInfoFromStore.ShouldNotBeNull();
            jobInfoFromStore.JobType.ShouldBeSameAs(jobInfo.JobType);
            jobInfoFromStore.JobArgs.ShouldBeSameAs(jobInfo.JobArgs);

            await _store.DeleteAsync(jobInfo);
            (await _store.GetWaitingJobsAsync(1000)).Count.ShouldBe(0);
        }
    }
}
