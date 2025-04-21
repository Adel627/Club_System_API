namespace Club_System_API.Helper
{
    public static class FormFileExtensions
    {
        public static byte[] ConvertToBytes(IFormFile file)
        {
            if (file == null) return null;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
       

    }

}
