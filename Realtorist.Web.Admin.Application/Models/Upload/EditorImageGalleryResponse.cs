namespace Realtorist.Web.Admin.Application.Models.Upload
{
    public class EditorImageGalleryResponse
    {
        public class EditorImageGalleryResponseFile 
        {
            public string Src { get; set; }
        }

        public string ErrorMessage { get; set; }

        public EditorImageGalleryResponseFile[] Result { get; set; }
    }
}