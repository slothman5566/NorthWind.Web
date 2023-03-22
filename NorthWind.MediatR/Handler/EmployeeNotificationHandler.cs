using MediatR;
using Microsoft.Extensions.Logging;
using NorthWind.MediatR.Event;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWind.MediatR.Handler
{
    public class EmployeeNotificationHandler : INotificationHandler<EmployeeNotifyEvent>
    {
        private readonly ILogger _logger;
        public EmployeeNotificationHandler(ILogger<INotification> logger)
        {
            _logger = logger;

        }
        public Task Handle(EmployeeNotifyEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Notify, {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
