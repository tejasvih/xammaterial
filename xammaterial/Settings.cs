using System;
using System.Collections.Generic;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Calibre
{
    public static class Settings
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public static bool IsTermsAndConditionsAgreed
        {
            get => AppSettings.GetValueOrDefault(nameof(IsTermsAndConditionsAgreed), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsTermsAndConditionsAgreed), value);
        }
        public static bool HasEditingRights
        {
            get => AppSettings.GetValueOrDefault("HasEditingRights", false);
            set => AppSettings.AddOrUpdateValue("HasEditingRights", value);
        }
        public static int LoggedInUserId
        {
            get => AppSettings.GetValueOrDefault("UserId", 0);
            set => AppSettings.AddOrUpdateValue("UserId", value);
        }
        public static string LoggedInUserName
        {
            get => AppSettings.GetValueOrDefault("UserName", null);
            set => AppSettings.AddOrUpdateValue("UserName", value);
        }
        public static string LoggedInUserMobileNumber
        {
            get => AppSettings.GetValueOrDefault("MobileNumber", null);
            set => AppSettings.AddOrUpdateValue("MobileNumber", value);
        }
        public static string LoggedInUserFullName
        {
            get => AppSettings.GetValueOrDefault("FullName", null);
            set => AppSettings.AddOrUpdateValue("FullName", value);
        }
        public static string LoggedInUserFullNameInEnglish
        {
            get => AppSettings.GetValueOrDefault("FullNameInEnglish", null);
            set => AppSettings.AddOrUpdateValue("FullNameInEnglish", value);
        }
        public static string AuthToken
        {
            get => AppSettings.GetValueOrDefault("AuthToken", "");
            set => AppSettings.AddOrUpdateValue("AuthToken", value);
        }
    }
}
