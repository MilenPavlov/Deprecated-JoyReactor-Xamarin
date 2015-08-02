﻿using System;
using System.Collections.Generic;
using Autofac;
using JoyReactor.Core.Model;
using JoyReactor.Core.Model.Database;
using JoyReactor.Core.Model.Messages;
using JoyReactor.Core.Model.Parser;
using JoyReactor.Core.Model.Web;
using JoyReactor.Core.ViewModels;
using Microsoft.Practices.ServiceLocation;
using Refractored.Xam.Settings;

namespace JoyReactor.Core.Model
{
	public class DefaultServiceLocator : ServiceLocatorImplBase
	{
		IContainer locator;

		public DefaultServiceLocator(params Module[] platformModule)
		{
			var b = new ContainerBuilder();

			b.RegisterModule(new DefaultModule());
			foreach (var s in platformModule)
			{
				b.RegisterModule(s);
			}

			locator = b.Build();
		}

		#region implemented abstract members of ServiceLocatorImplBase

		protected override object DoGetInstance(Type serviceType, string key)
		{
			return locator.Resolve(serviceType);
		}

		protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
		{
			throw new NotImplementedException();
		}

		#endregion

		#region Inner classes

		class DefaultModule : Module
		{
			protected override void Load(ContainerBuilder b)
			{
				b.RegisterType<WebDownloader>().As<WebDownloader>();
				b.Register(_ => SQLiteConnectionFactory.Create()).As<AsyncSQLiteConnection>();

				b.Register(_ => CrossSettings.Current).AsSelf();

                b.RegisterType<MessageRepository>().As<JoyReactor.Core.Model.Messages.MessageService.IStorage>();
				b.RegisterType<MessageRepository>().As<MessageFetcher.IStorage>();

				b.RegisterType<MessageService>().As<MessagesViewModel.IMessageService>();

				b.RegisterType<AuthRepository>().As<ProfileService.IAuthStorage>();
				b.RegisterType<AuthRepository>().As<ReactorMessageParser.IAuthStorage>();
				b.RegisterType<AuthRepository>().As<IProviderAuthStorage>();

				b.RegisterType<CommonRepository>().As<IProviderStorage>();
				b.RegisterType<CommonRepository>().As<PostService.IStorage>();

				b.RegisterType<PostService>().As<CreateTagViewModel.IPostService>();
                b.RegisterInstance(MemoryStorage.Intance).As<TagCollectionModel.Storage>();
			}
		}

		#endregion
	}
}