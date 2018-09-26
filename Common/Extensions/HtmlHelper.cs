using System.IO;
using Microsoft.AspNetCore.Html;

namespace Common.Extensions
{
    public static class HtmlHelper
    {
        public static IHtmlContent VersionedJs(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper helper, string filename)
        {
            string version = GetVersion(filename);
            return new HtmlContentBuilder().AppendHtml("<script type=\"text/javascript\" src=\"" + filename + version + "\"></script>");
        }

        public static IHtmlContent VersionedCss(this Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper helper, string filename)
        {
            string version = GetVersion(filename);
            return new HtmlContentBuilder().AppendHtml("<link rel=\"stylesheet\" href=\"" + filename + version + "\"/>");
        }
        private static string GetVersion( string filename)
        {
            var physicalPath = $"{Directory.GetCurrentDirectory()}/wwwroot/{filename}";
            var version = $"?v={new FileInfo(physicalPath).LastWriteTime:yyyyMMddHHmmss}";
            return version;
        }
    }
}
