using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services;
using Xunit;

namespace Tests.Tests
{
    [TestCaseOrderer("Tests.AuditLogsTests.AlphabeticalOrderer", "Tests")]
    public class AuditLogsTests
    {
        private readonly HolidayDbContext _context;
        private readonly AuditLogsService _auditLogsService;
        public AuditLogsTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;

            var auditLogsRepository = new AuditLogsRepository(_context);
            _auditLogsService = new AuditLogsService(auditLogsRepository);
        }

        [Theory]
        [InlineData("Employee")]
        [InlineData("NotExisting")]
        [InlineData(null)]
        public async Task When_GettingAuditLogsByEntityType_Expect_ReturnedAuditLogsCountIsCorrect(string entityType)
        {
            var auditLogsCount = (await _auditLogsService.GetByType(entityType, 1, 100)).Logs.Count;

            var auditLogsCountInJSON = (await _auditLogsService.GetByType(entityType, 1, 100)).TotalCount;

            int actualCount = 0;
            var allAuditLogs = await _context.AuditLogs.ToArrayAsync();
            if(entityType == null)
            {
                actualCount = allAuditLogs.Length;
            }
            else
            {
                foreach (var auditLog in allAuditLogs)
                {
                    if (auditLog.EntityType == entityType)
                    {
                        actualCount++;
                    }
                }
            }

            Assert.Equal(auditLogsCount, actualCount);
            Assert.Equal(auditLogsCountInJSON, actualCount);
        }

        [Theory]
        [InlineData(null, 1, 20)]
        [InlineData(null, 2, 3)]
        [InlineData(null, 3, 1)]
        public async Task When_GettingAuditLogsPage_Expect_ReturnedAuditLogsCountIsCorrect(string entityType, int page, int pageSize)
        {
            var auditLogsCount = (await _auditLogsService.GetByType(entityType , page, pageSize)).Logs.Count;

            int actualCount = 0;

            var allAuditLogs = await _context.AuditLogs.ToArrayAsync();

            int startFrom = (page - 1) * pageSize;
            int countTo = startFrom + pageSize;
            for (int i = startFrom; i < countTo; i++)
            {
                try
                {
                    if (allAuditLogs[i] != null)
                    {
                        actualCount++;
                    }
                }
                catch { }
            }

            Assert.Equal(auditLogsCount, actualCount);
        }

    }
}
