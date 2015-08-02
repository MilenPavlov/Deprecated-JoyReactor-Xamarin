﻿using Autofac;
using JoyReactor.Core.Model.Web;
using SQLite.Net.Interop;
using SQLite.Net.Platform.XamarinAndroid;

namespace JoyReactor.Android.Model
{
    public class AndroidInjectModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SQLitePlatformAndroid>().As<ISQLitePlatform>();
            builder.RegisterInstance(new AndroidWebDownloader()).As<WebDownloader>();
        }
    }
}