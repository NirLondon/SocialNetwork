﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Client.ServicesInterfaces
{
    public interface IPostService
    {
        Task<byte[]> ChooseImage();
    }
}
