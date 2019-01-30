using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Notifications.Common.DAL;
using Notifications.Common.Models.ReceivedDTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.DAL
{
    public class DynamoDBNotificationsRepository : INotificationsRepository
    {
        private readonly Table _notificationsTable;

        public DynamoDBNotificationsRepository()
        {
            var dbClient = new AmazonDynamoDBClient();

            _notificationsTable = Table.LoadTable(dbClient, "Notifications");
        }

        public Task Save(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task Save(Notification notification, bool beenRead)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> UnReadItemsOf(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
