using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Constraint
{
    public static class FileExtensionConstraint
    {
        public static bool IsFileType(this string extension,string type) =>
            type.Contains(extension);
        
        public const string Image = ".jpg,.jpeg,.png,.gif,.bmp";
        public const string Document = ".doc,.docx,.pdf,.xls,.xlsx,.ppt,.pptx,.txt";
        public const string Video = ".mp4,.avi,.flv,.wmv,.mov,.mpg,.mpeg";
        public const string Audio = ".mp3,.wav,.wma,.ogg,.flac";
        public const string Archive = ".zip,.rar,.7z,.tar,.gz";
        public const string SpreadSheet = ".xls,.xlsx,.xltx,.xlsb,.ods";
    }
}
