using Notifications.Common.DAL;
using Notifications.Common.Models.ReturnedDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Notifications.Server.Controllers
{
    public class RetrievingController : ApiController
    {
        private readonly INotificationsRepository _repository;

        public RetrievingController(INotificationsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        [Route("api/Notifications/{startTime}")]
        public ReturnedNotification NotificationsFrom(DateTime startTime)
        {
            _repository.NotificationsOf("", true, DateTime.MinValue);
        }
    }
}
