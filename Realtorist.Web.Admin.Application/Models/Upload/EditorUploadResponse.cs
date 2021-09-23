namespace Realtorist.Web.Admin.Application.Models.Upload
{
    public class EditorUploadResponse
    {
        public string ErrorMessage { get; set; }

        public EditorUploadResponseFile[] Result { get; set; }
    }
}