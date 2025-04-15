namespace Store.Common.Helper
{
    public class Helper
    {
        public static string GenerateUUID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
