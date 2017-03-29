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
        public static XboxLiveUser User { get; private set; }
        public static XboxLiveContext Context { get; private set; }
        public static XboxSocialUser Profile { get; private set; }
        public static BitmapImage GamerPic { get; private set; }

        private static PeopleHubService peopleHubService;
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
            if (signinResult.Status== SignInStatus.Success)
            {
                Context = new XboxLiveContext(User);
                peopleHubService = new PeopleHubService(Context.Settings, Context.AppConfig);
                Profile = await peopleHubService.GetProfileInfo(User, SocialManagerExtraDetailLevel.None);
                GamerPic = new BitmapImage(new Uri(Profile.DisplayPicRaw));
                isInitialized = true;
            }
            return isInitialized == true;
        }

    }
}
