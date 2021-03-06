﻿using JoyReactor.Core.Model;
using JoyReactor.Core.Model.DTO;
using JoyReactor.Core.Tests.Helpers;
using Microsoft.Practices.ServiceLocation;
using NUnit.Framework;
using SQLite.Net;
using System;

namespace JoyReactor.Core.Tests
{
    [TestFixture]
	public class PostModelTests
	{
		const int TestPostId = 1;
        PostService model;

		[SetUp]
		public void SetUp ()
		{
			var provider = new DefaultServiceLocator (new TestModule ());
			ServiceLocator.SetLocatorProvider (() => provider);
            model = new PostService ();
		}

		[Test]
        [Ignore] // FIXME:
		public void GetTopComments ()
		{
//			SyncMockPost ();
//
//			var actual = model.GetTopCommentsAsync (TestPostId, 10).Result;
//
//			Assert.IsNotNull (actual);
//			Assert.AreEqual (10, actual.Count);
		}

		[Test]
        [Ignore] // FIXME:
		public void GetAttachments ()
		{
//			SyncMockPost ();
//
//			var actual = model.GetAttachmentsAsync (1).Result;
//
//			Assert.IsNotNull (actual);
//			Assert.AreEqual (89, actual.Count, "Count = " + actual.Count);
//
//			foreach (var a in actual) {
//				Assert.IsNotNull (a.Url);
//				Assert.IsTrue (Uri.IsWellFormedUriString (a.Url, UriKind.Absolute));
//			}
		}

        // FIXME:
		void SyncMockPost ()
		{
//			var conn = ServiceLocator.Current.GetInstance<SQLiteConnection> ();
//			conn.Insert (new Post {
//				PostId = ID.SiteParser.Chan4 + "-b,572092321"
//			});
//			model.GetPostAsync (TestPostId).Wait ();
		}
	}
}