using AutoMapper;
using System;
using System.Threading.Tasks;
using XplicityApp.Dtos.NotificationSettings;
using XplicityApp.Infrastructure.Repositories;
using XplicityApp.Services.Interfaces;

namespace XplicityApp.Services
{
    public class NotificationSettingsService : INotificationSettingsService
    {
        private readonly INotificationSettingsRepository _notificationSettingsRepository;
        private readonly IMapper _mapper;
        public NotificationSettingsService(INotificationSettingsRepository notificationSettingsRepository, IMapper mapper)
        {
            _notificationSettingsRepository = notificationSettingsRepository;
            _mapper = mapper;
        }
        public async Task<NotificationSettingsDto> GetByEmployeeId(int employeeId)
        {
            var notificationSettings = await _notificationSettingsRepository.GetByEmployeeId(employeeId);
            var notificationSettingsDto = _mapper.Map<NotificationSettingsDto>(notificationSettings);

            return notificationSettingsDto;
        }

        public async Task<bool> Update(int employeeId, NotificationSettingsDto notificationSettingsDto)
        {
            if (notificationSettingsDto == null)
            {
                throw new ArgumentNullException(nameof(notificationSettingsDto));
            }

            var notificationSettings = await _notificationSettingsRepository.GetByEmployeeId(employeeId);

            if (notificationSettings == null)
            {
                throw new InvalidOperationException();
            }

            _mapper.Map(notificationSettingsDto, notificationSettings);
            await _notificationSettingsRepository.Update(notificationSettings);

            return await _notificationSettingsRepository.Update(notificationSettings);

        }
    }
}
