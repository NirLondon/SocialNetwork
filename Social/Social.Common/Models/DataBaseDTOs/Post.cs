using Social.Common.Models.UploadedDTOs;

namespace Social.Common.Models.DataBaseDTOs
{
    public class DataBasePost
    {
        public string Content { get; set; }
        public string ImageURL { get; set; }
        public string[] TagedUsersIds { get; set; }
        public PostVisibility Visibility { get; set; }
    }
}
