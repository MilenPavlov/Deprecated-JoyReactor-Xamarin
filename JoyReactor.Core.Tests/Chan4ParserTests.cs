﻿using NUnit.Framework;
using System;
using Microsoft.Practices.ServiceLocation;
using JoyReactor.Core.Model.Inject;
using JoyReactor.Core.Model.Web.Parser;
using JoyReactor.Core.Tests.Inner;
using JoyReactor.Core.Model.Parser;
using System.Text.RegularExpressions;

namespace JoyReactor.Core.Tests
{
	[TestFixture]
	public class Chan4ParserTests
	{
		private SiteParser parser;

		[Test]
		public void TestGetThread572092321 ()
		{
			int attachemnts = 0;
			parser.NewPost += (sender, e) => {
				// TODO

				Assert.AreEqual (1, e.Attachments.Length, "Count = " + e.Attachments.Length);
				var a = e.Attachments [0];
				Assert.IsTrue (Uri.IsWellFormedUriString (a.Image, UriKind.Absolute), "Url = " + a.Image);
				Assert.AreEqual (720, a.Width, "Width = " + a.Width);
				Assert.AreEqual (720, a.Height, "Height = " + a.Height);

				attachemnts += e.Attachments.Length;
			};
			parser.NewComment += (sender, e) => {
				// TODO

				foreach (var a in e.Attachments) {
					Assert.IsTrue (Uri.IsWellFormedUriString (a.Image, UriKind.Absolute), "Url = " + a.Image);
					Assert.IsTrue (a.Width > 0, "Width = " + a.Width);
					Assert.IsTrue (a.Height > 0, "Height = " + a.Height);
				}

				attachemnts += e.Attachments.Length;
			};
			parser.ExtractPost ("b,572092321");

			Assert.AreEqual (90, attachemnts, "Count = " + attachemnts);
		}

		[Test]
		public void TestGetPostsFromB ()
		{
			int actualPostCount = 0;
			parser.ExtractTagPostCollection (ID.TagType.Good, "b", 0, null, state => {

				Assert.IsNotNull (state);
				if (state.State == CollectionExportState.ExportState.PostItem) {
					actualPostCount++;
					// TODO: дописать тест
					Assert.IsTrue (Regex.IsMatch (state.Post.Id, @"b,\d+"), "Post id = " + state.Post.Id);
				}

			});

			Assert.AreEqual (15, actualPostCount);
		}

		[Test]
		public void TestMultiPageLoading ()
		{
			for (int i = 0; i < 2; i++)
				parser.ExtractTagPostCollection (ID.TagType.Good, "b", i, null, state => {
					// Ignore
				});
		}

		[SetUp]
		public void SetUp ()
		{
			ServiceLocator.SetLocatorProvider (() => new DefaultServiceLocator (new TestModule ()));
			parser = new Chan4Parser ();
		}

		[Test]
		public void Chan4_GetPosts_WSG ()
		{
			parser.ExtractTagPostCollection (ID.TagType.Good, "wsg", 0, null, state => {
				Assert.IsNotNull (state);
				// TODO: дописать тест
			});
		}
	}
}