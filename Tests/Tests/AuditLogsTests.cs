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
        private readonly int _auditLogsCount;
        public AuditLogsTests()
        {
            var setup = new SetUp();
            setup.Initialize();
            _context = setup.HolidayDbContext;
            _auditLogsCount = setup.GetCount("auditLogs");

            var mapper = setup.Mapper;
            var auditLogsRepository = new AuditLogsRepository(_context);
            _auditLogsService = new AuditLogsService(auditLogsRepository);
        }

        [Fact]
        public async Task When_GettingAllAuditLogs_Expect_ReturnsAuditLogs()
        {
            var auditLogsCount = (await _auditLogsService.GetAll()).Count;

            Assert.Equal(auditLogsCount, _auditLogsCount);
        }

        [Theory]
        [InlineData("Employee")]
        [InlineData("NotExisting")]
        public async Task When_GettingAuditLogsByEntityType_Expect_ReturnsAuditLogs(string entityType)
        {
            var auditLogsCount = (await _auditLogsService.GetByEntityType(entityType, 1, 100)).Count;

            int actualCount = 0;
            var allAuditLogs = await _auditLogsService.GetAll();
            foreach (var auditLog in allAuditLogs)
            {
                if(auditLog.EntityType == entityType)
                {
                    actualCount++;
                }
            }

            Assert.Equal(auditLogsCount, actualCount);
        }

        [Theory]
        [InlineData(1, 20)]
        [InlineData(2, 3)]
        [InlineData(3, 1)]
        public async Task When_GettingAuditLogsPage_Expect_ReturnsAuditLogs(int page, int pageSize)
        {
            var auditLogsCount = (await _auditLogsService.GetPage(page, pageSize)).Count;

            int actualCount = 0;

            var allAuditLogs = await _auditLogsService.GetAll();

            AuditLog[] AuditLogsArray = new AuditLog[allAuditLogs.Count];
            allAuditLogs.CopyTo(AuditLogsArray, 0);

            int startFrom = (page - 1) * pageSize;
            int countTo = startFrom + pageSize;
            for (int i = startFrom; i < countTo; i++)
            {
                try
                {
                    if (AuditLogsArray[i] != null)
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
