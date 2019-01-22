namespace Client.Models.UploadedDTOs
{
    public class UploadedPost
    {
        public string Content { get; set; }
        public byte[] Image { get; set; }    
        public string[] TagedUsersIds { get; set; }
        public PostVisibility  Visibility { get; set; }        
    }
}
