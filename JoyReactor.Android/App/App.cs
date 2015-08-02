﻿using System;
using Android.App;
using Android.Runtime;
using JoyReactor.Android.Model;
using JoyReactor.Android.Platform;
using JoyReactor.Core.Model;
using Microsoft.Practices.ServiceLocation;

namespace JoyReactor.Android.App
{
    [Application(Theme = "@style/AppTheme")]
    public class App : Application
    {
        public static Application Instance { get; private set; }

        public App(IntPtr handle, JniHandleOwnership transfer)
            : base(handle, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            #if !DEBUG
            Xamarin.Insights.Initialize("1664e416e9def27db9e1d4ddc74255f3537a2c16", this);
            #endif

            Instance = this;
            InitializePlatformCode();
        }

        static void InitializePlatformCode()
        {
            var locator = new DefaultServiceLocator(new AndroidInjectModule());
            ServiceLocator.SetLocatorProvider(() => locator);

            PrivateMessageChecker.Instance = new PlatformPrivateMessageChecker();
            PrivateMessageChecker.Instance.Initialize();
            MessageService.Instance = new PlatformMessageService();
        }
    }
}