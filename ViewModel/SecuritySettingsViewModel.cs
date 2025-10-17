using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeatBox.ViewModel
{
    public class SecuritySettingsViewModel
    {
        public string Id { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool HasAuthenticator { get; set; }
        public int RecoveryCodesLeft { get; set; }
    }
}