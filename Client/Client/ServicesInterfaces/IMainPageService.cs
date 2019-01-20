using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ServicesInterfaces
{
    public interface IMainPageService
    {
        void GoToFeed();

        void GoToIdentity();

        void GoToFollowed();

        void GoToFollowers();

        void GoToBlocked();

        void LogOut();
    }
}
