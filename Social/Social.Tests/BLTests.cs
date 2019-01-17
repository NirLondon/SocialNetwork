using Microsoft.VisualStudio.TestTools.UnitTesting;
using Social.BL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Tests
{
    [TestClass]
    public class BLTests
    {
        [TestMethod]
        public void UploadPhoto()
        {
            var storage = new PhotoStorage();

            using (var fs = new FileStream(@"C:\Users\ניר לונדון\Desktop\pic.jpg", FileMode.Open))
            {
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                storage.UploadPhoto(buffer, out string url);
            }
        }
    }
}
