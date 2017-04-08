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
using Microsoft.Xbox.Services.Stats.Manager;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xbox.Services.Leaderboard;

namespace BubbleBreakerUWP
{
    public class XboxLiveManager : INotifyPropertyChanged
    {
        private static XboxLiveManager m_instance = null;
        public static XboxLiveManager Instance => m_instance ?? (m_instance = new XboxLiveManager());

        private XboxLiveUser m_user;
        private XboxLiveContext m_context;
        private XboxSocialUser m_profile;
        private BitmapImage m_gamerpic;
        private XboxSocialUserGroup m_group;
        private bool m_canUseStats;
        private bool m_signedIn;

        public XboxLiveUser User => m_user;
        public bool SignedIn => m_signedIn;
        public bool StatsAvailable => m_canUseStats;
        public XboxSocialUser Profile => m_profile;
        public BitmapImage Gamerpic => m_gamerpic;
        public XboxSocialUserGroup Group => m_group;

        /// <summary>
        /// 
        /// </summary>
        private XboxLiveManager()
        {
            m_signedIn = false;
            m_canUseStats = false;
            m_user = new XboxLiveUser();
            DoMainLoop();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SignInAsync()
        {
            var signInResult = await m_user.SignInSilentlyAsync();

            if (signInResult.Status == SignInStatus.UserInteractionRequired)
                signInResult = await m_user.SignInAsync();

            if (signInResult.Status == SignInStatus.Success)
            {
                this.OnPropertyChanged("User");

                StatsManager.Instance.AddLocalUser(m_user);

                await SocialManager.Instance.AddLocalUser(this.m_user, SocialManagerExtraDetailLevel.None);
                ulong userId = ulong.Parse(m_user.XboxUserId);
                var group = SocialManager.Instance.CreateSocialUserGroupFromList(m_user, new List<ulong> { userId });
                m_profile = group.GetUser(userId);
                this.OnPropertyChanged("Profile");

                m_gamerpic = new BitmapImage(new Uri(m_profile.DisplayPicRaw));
                this.OnPropertyChanged("Gamerpic");

                m_context = new XboxLiveContext(m_user);

                m_signedIn = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void RetrieveFriends()
        {
            if (!m_signedIn) return;
            m_group = SocialManager.Instance.CreateSocialUserGroupFromFilters(m_user, PresenceFilter.All, RelationshipFilter.Friends, m_context.AppConfig.TitleId);
            this.OnPropertyChanged("Group");
        }

        /// <summary>
        /// 
        /// </summary>
        public void RetrieveFavorites()
        {
            if (!m_signedIn) return;
            m_group = SocialManager.Instance.CreateSocialUserGroupFromFilters(m_user, PresenceFilter.All, RelationshipFilter.Favorite, m_context.AppConfig.TitleId);
            this.OnPropertyChanged("Group");
        }

        /// <summary>
        /// 
        /// </summary>
        private async void DoMainLoop()
        {
            while (true)
            {
                if (m_user.IsSignedIn)
                {
                    // Perform the long running do work task on a background thread.
                    var doWorkTask = Task.Run(() => { return StatsManager.Instance.DoWork(); });

                    List<StatEvent> events = await doWorkTask;
                    foreach (StatEvent ev in events)
                    {
                        if (ev.EventType == StatEventType.LocalUserAdded)
                        {
                            m_canUseStats = true;
                        }

                        if (ev.EventType == StatEventType.StatUpdateComplete)
                        {

                        }
                    }
                }
                await Task.Delay(200);
            }
        }

        public void Flush()
        {
            if (!m_canUseStats) return;
            StatsManager.Instance.RequestFlushToService(m_user);
        }

        public long Highscore
        {
            get
            {
                if (m_canUseStats)
                {
                    var x = StatsManager.Instance.GetStat(User, "Score").AsInteger();
                }
                return (m_canUseStats ? StatsManager.Instance.GetStat(User, "Score").AsInteger() : 0);
            }
            set { if (m_canUseStats) StatsManager.Instance.SetStatAsInteger(User, "Score", value); }
        }

        //public static void SetHighscore(long h)
        //{
        //    if (isInitialized)
        //    {
        //        StatsManager.Instance.SetStatAsInteger(User, "Score", h);
        //    }
        //}

        //public static long GetHighscore()
        //{
        //    if (isInitialized)
        //    {
        //        var s = StatsManager.Instance.GetStat(User, "Score");
        //        return s.AsInteger();
        //    }
        //    else return -1;
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
