﻿using System;
using System.Collections.Generic;
using Autofac;
using GalaSoft.MvvmLight.Messaging;
using JoyReactor.Core.Model;
using JoyReactor.Core.Model.DTO;
using JoyReactor.Core.Model.Web;
using JoyReactor.Core.Tests.Helpers;
using Microsoft.Practices.ServiceLocation;
using Moq;
using SQLite.Net;

namespace JoyReactor.Core.Tests
{
    static class TestExtensions
    {
        public static void SetFakeSite(string url, string filename)
        {
            var downloader = (MockWebDownloader)ServiceLocator.Current.GetInstance<WebDownloader>();
            downloader.RouteUrls[url] = filename;
        }

        public static int CreatePostIdDatabase(ID.SiteParser parser, string postId)
        {
            var post = new Post { PostId = parser + "-" + postId };
            return GetDatabase().Insert(post);
        }

        public static void SetUp()
        {
            var provider = new DefaultServiceLocator(new TestModule());
            ServiceLocator.SetLocatorProvider(() => provider);
        }

        public static void TearDown(object testInstance)
        {
            Messenger.Default.Unregister(testInstance);
        }

        public static SQLiteConnection GetDatabase()
        {
            return ServiceLocator.Current.GetInstance<SQLiteConnection>();
        }

        public static InjectBuilder BeginInjection()
        {
            return new InjectBuilder();
        }

        internal class InjectBuilder
        {
            Dictionary<object, Type> injections = new Dictionary<object, Type>();

            public InjectBuilder Add<T>(object value)
            {
                injections[value] = typeof(T);
                return this;
            }

            public InjectBuilder Add<T>() where T: class
            {
                return Add<T>(Mock.Of<T>());
            }

            public void Commit()
            {
                var provider = new DefaultServiceLocator(new TestModule(builder =>
                        {
                            foreach (var s in injections.Keys)
                                builder.RegisterInstance(s).As(injections[s]);
                        }));
                ServiceLocator.SetLocatorProvider(() => provider);
            }
        }
    }
}