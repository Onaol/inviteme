using System.Drawing.Imaging;
using System.Drawing;

namespace InviteMe.Extensions;

public static class BitmapExtensions
{
    public static byte[] BitmapToByteArray(this Bitmap bitmap)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }

    public static string Base64toString(byte[] byteArray)
    {
        return Convert.ToBase64String(byteArray);
    }
}
