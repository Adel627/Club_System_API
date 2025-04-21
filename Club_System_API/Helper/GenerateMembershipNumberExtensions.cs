namespace Club_System_API.Helper
{
    public static class GenerateMembershipNumberExtensions
    {
        public static string GenerateMembershipNumber()
        {
            return $"MBR-{Guid.NewGuid().ToString().Substring(0, 8)}"; // Example: MBR-1a2b3c4d
        }
    }
}
