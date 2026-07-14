using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Attachments
{
    public class AttachmentRules
    {
        public const long MaxBytes = 5 * 1024 * 1024; // 5 MB
        public static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg", ".jpeg", ".png", ".gif", ".pdf", ".docx", ".xlsx"
        };


    }
}
