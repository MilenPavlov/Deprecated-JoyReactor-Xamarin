﻿using System;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using GalaSoft.MvvmLight.Messaging;
using JoyReactor.Android.App.Gallery;
using JoyReactor.Android.App.Posts;
using JoyReactor.Android.Model;
using JoyReactor.Core.ViewModels;
using JoyReactor.Core.ViewModels.Common;
using Messenger = GalaSoft.MvvmLight.Messaging.Messenger;

namespace JoyReactor.Android.App.Base
{
    public class BaseActivity : AppCompatActivity
    {
        public const string Arg1 = "arg1";
        public const string Arg2 = "arg2";
        public const string Arg3 = "arg3";
        public const string Arg4 = "arg4";

        protected ScopedViewModel.Scope Scope = new ScopedViewModel.Scope();

        protected BindingManager Bindings = new BindingManager();

        public IMessenger MessengerInstance
        {
            get { return Messenger.Default; }
        }

        protected static Intent NewIntent(Type activityType, params object[] args)
        {
            var t = new Intent(App.Instance, activityType);
            for (int i = 0; i < args.Length; i++)
            {
                var a = args[i];
                var key = "arg" + (i + 1);

                if (a is string)
                    t.PutExtra(key, (string)a);
                else if (a is int)
                    t.PutExtra(key, (int)a);
                else if (a is long)
                    t.PutExtra(key, (long)a);
            }
            return t;
        }

        public void NavigateToGallery(int postId)
        {
            StartActivity(NewIntent(typeof(GalleryActivity), postId));
        }

        public void NavigateToFullscreenGallery(int postId, int initPosition)
        {
            StartActivity(NewIntent(typeof(FullscreenGalleryActivity), postId, initPosition));
        }

        public void SetContentViewForFragment()
        {
            SetContentView(Resource.Layout.layout_activity_container);
        }

        public void SetRootFragment(Fragment fragment)
        {
            SupportFragmentManager.BeginTransaction()
				.Replace(Resource.Id.container, fragment)
				.Commit();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.feedback, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.feedback)
            {
                new FeedbackController(this).Send();
                return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnResume()
        {
            base.OnResume();
            MessengerInstance.Register<PostNavigationMessage>(
                this, s => StartActivity(PostActivity.NewIntent(s.PostId)));
            ViewModel.NavigationService = new NavigationService(this);

            BaseNavigationService.Reset(new JoyReactor.Android.NavigationService(this));
            Scope.OnActivated();
        }

        protected override void OnPause()
        {
            base.OnPause();
            MessengerInstance.Unregister(this);
            ViewModel.NavigationService = null;

            Scope.OnDeactivated();
            BaseNavigationService.Reset(null);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Bindings.Destroy();
        }
    }
}