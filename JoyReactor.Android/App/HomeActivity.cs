﻿using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using JoyReactor.Android.App.Base;
using JoyReactor.Android.App.Home;
using JoyReactor.Core.ViewModels.Common;

namespace JoyReactor.Android.App
{
    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.Toolbar",
        LaunchMode = global::Android.Content.PM.LaunchMode.SingleTop,
        ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    [Register("net.itwister.joyreactor2.HomeActivity")]
    public class HomeActivity : BaseActivity
    {
        ViewPager pager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_home);

            pager = FindViewById<ViewPager>(Resource.Id.pager);
            pager.Adapter = new Adapter(SupportFragmentManager);
            pager.CurrentItem = 1;
        }

        protected override void OnResume()
        {
            base.OnResume();
            MessengerInstance.Register<Messages.SelectTagMessage>(this, _ => pager.CurrentItem = 1);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.home, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    pager.CurrentItem = 0;
                    return true;
                case Resource.Id.profile:
                    StartActivity(typeof(ProfileActivity));
                    return true;
                case Resource.Id.messages:
                    StartActivity(typeof(MessageActivity));
                    return true;
                case Resource.Id.addTag:
                    new CreateTagDialog().Show(SupportFragmentManager, null);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public override void OnBackPressed()
        {
            if (pager.CurrentItem == 0)
                pager.CurrentItem = 1;
            else
                base.OnBackPressed();
        }
    }

    public class Adapter : FragmentPagerAdapter
    {
        public Adapter(global::Android.Support.V4.App.FragmentManager fm)
            : base(fm)
        {
        }

        public override int Count
        {
            get { return 3; }
        }

        public override float GetPageWidth(int position)
        {
            var met = App.Instance.Resources.DisplayMetrics;
            var panelWidth = 260 * met.Density / met.WidthPixels;
            return position == 1 ? 1 : panelWidth;
        }

        public override global::Android.Support.V4.App.Fragment GetItem(int position)
        {
            if (position == 0)
                return new LeftMenuFragment();
            if (position == 1)
                return new FeedFragment();
            return new RightMenuFragment();
        }
    }

    public class EmptyFragment : BaseFragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = new View(container.Context);
            view.SetBackgroundColor(Color.Green);
            return view;
        }
    }

    [Activity(
        Label = "@string/app_name",
        Theme = "@style/AppTheme.Launcher",
        MainLauncher = true,
        ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(typeof(HomeActivity));
            OverridePendingTransition(Resource.Animation.abc_fade_in, Resource.Animation.abc_fade_out);
            Finish();
        }
    }
}