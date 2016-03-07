// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace IdeaTrackr.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        static ISettings AppSettings => CrossSettings.Current;

        #region Setting Constants

        const string LastSyncKey = "last_sync";
        static readonly DateTime LastSyncDefault = DateTime.Now.AddDays(-30);

        const string UserIdKey = "userid";
        static readonly string UserIdDefault = string.Empty;

        const string AuthTokenKey = "authtoken";
        static readonly string AuthTokenDefault = string.Empty;

        const string LoginAttemptsKey = "login_attempts";
        const int LoginAttemptsDefault = 0;

        const string NeedsSyncKey = "needs_sync";
        const bool NeedsSyncDefault = true;

        const string HasSyncedDataKey = "has_synced";
        const bool HasSyncedDataDefault = false;

        #endregion

#if DEBUG
        public static bool NeedsSync { get { return true; } set { } }
#else
        public static bool NeedsSync
        {
        get { return AppSettings.GetValueOrDefault(NeedsSyncKey, NeedsSyncDefault) || LastSync < DateTime.Now.AddDays(-1); }
        set { AppSettings.AddOrUpdateValue(NeedsSyncKey, value); }

        }
#endif

        public static bool HasSyncedData
        {
            get { return AppSettings.GetValueOrDefault(HasSyncedDataKey, HasSyncedDataDefault); }
            set { AppSettings.AddOrUpdateValue(HasSyncedDataKey, value); }

        }

        public static DateTime LastSync
        {
            get { return AppSettings.GetValueOrDefault(LastSyncKey, LastSyncDefault); }
            set { AppSettings.AddOrUpdateValue(LastSyncKey, value); }
        }


        public static string AuthToken
        {
            get { return AppSettings.GetValueOrDefault(AuthTokenKey, AuthTokenDefault); }
            set { AppSettings.AddOrUpdateValue(AuthTokenKey, value); }
        }

        public static string UserId
        {
            get { return AppSettings.GetValueOrDefault(UserIdKey, UserIdDefault); }
            set { AppSettings.AddOrUpdateValue(UserIdKey, value); }
        }

        public static int LoginAttempts
        {
            get { return AppSettings.GetValueOrDefault(LoginAttemptsKey, LoginAttemptsDefault); }
            set { AppSettings.AddOrUpdateValue(LoginAttemptsKey, value); }
        }

        public static bool IsLoggedIn => !string.IsNullOrWhiteSpace(UserId);
    }
}