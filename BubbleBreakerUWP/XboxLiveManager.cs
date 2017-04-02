using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Social;
using Microsoft.Xbox.Services.Social.Manager;
using System.Threading;
using Windows.UI.Xaml.Media.Imaging;

namespace BubbleBreakerUWP
{
    internal class XboxLiveManager
    {
        public static XboxLiveUser User { get; private set; } = null;
        public static XboxLiveContext Context { get; private set; } = null;
        public static XboxSocialUser Profile { get; private set; } = null;
        public static BitmapImage GamerPic { get; private set; } = null;
        public static XboxSocialUserGroup Friends { get; private set; } = null;
        public static XboxSocialUserGroup Favorites { get; private set; } = null;

        //private static PeopleHubService peopleHubService;
        private static Boolean isInitialized = false;

        /// <summary>
        /// Initializing the Manager object
        /// </summary>
        /// <returns>if Xbox Live SignIn fails returns false otherwise true</returns>
        public static async Task<Boolean> InititializeAsync()
        {
            if (isInitialized) return isInitialized;
            User = new XboxLiveUser();
            SignInResult signinResult = await User.SignInSilentlyAsync();
            if (signinResult.Status == SignInStatus.UserInteractionRequired)
            {
                signinResult = await User.SignInAsync();
            }
            if (signinResult.Status == SignInStatus.Success)
            {
                Context = new XboxLiveContext(User);

                await SocialManager.Instance.AddLocalUser(User, SocialManagerExtraDetailLevel.None);
                ulong userId = ulong.Parse(User.XboxUserId);
                var group = SocialManager.Instance.CreateSocialUserGroupFromList(User, new List<ulong> { userId });
                Profile = group.GetUser(userId);

                // Peoplehub approach. MS recommends to use SocialManager instead
                //peopleHubService = new PeopleHubService(Context.Settings, Context.AppConfig);
                //Profile = await peopleHubService.GetProfileInfo(User, SocialManagerExtraDetailLevel.None);

                GamerPic = new BitmapImage(new Uri(Profile.DisplayPicRaw));
                isInitialized = true;
            }
            return isInitialized == true;
        }

        public static bool RetrieveFriendsAndFavorites()
        {
            if (!isInitialized) return false;
            Friends = SocialManager.Instance.CreateSocialUserGroupFromFilters(User, PresenceFilter.All, RelationshipFilter.Friends, Context.AppConfig.TitleId);
            Favorites = SocialManager.Instance.CreateSocialUserGroupFromFilters(User, PresenceFilter.All, RelationshipFilter.Favorite, Context.AppConfig.TitleId);
            return true;
        }

    }
}
