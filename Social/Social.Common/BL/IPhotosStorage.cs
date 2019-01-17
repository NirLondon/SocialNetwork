namespace Social.Common.BL
{
    public interface IPhotosStorage
    {
        void UploadPhoto(byte[] photo, out string photoURL);
    }
}
