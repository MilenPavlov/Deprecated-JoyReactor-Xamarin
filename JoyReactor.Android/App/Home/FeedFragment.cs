﻿using Android.OS;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;
using Android.Views;
using JoyReactor.Android.App.Base;
using JoyReactor.Android.Widget;
using JoyReactor.Core.ViewModels;
using JoyReactor.Core.ViewModels.Common;

namespace JoyReactor.Android.App.Home
{
    public class FeedFragment : BaseFragment
    {
        FeedViewModel viewmodel;
        FeedRecyclerView list;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
            viewmodel = Scope.New<FeedViewModel>();

            MessengerInstance.Register<Messages.SelectTagMessage>(this, _ => list.ResetScrollToTop());
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.fragment_feed, null);

            list = view.FindViewById<FeedRecyclerView>(Resource.Id.list);
            list.SetAdapter(new FeedAdapter(viewmodel.Posts, viewmodel));

            var refresher = view.FindViewById<SwipeRefreshLayout>(Resource.Id.refresher);
            refresher.SetCommand(viewmodel.RefreshCommand);
            Bindings.Add(viewmodel, () => viewmodel.IsBusy, refresher, () => refresher.Refreshing);

            var applyButton = view.FindViewById<ReloadButton>(Resource.Id.apply);
            applyButton.Command = viewmodel.ApplyCommand;
            Bindings
                .Add(viewmodel, () => viewmodel.HasNewItems, applyButton, () => applyButton.Visibility)
				.ConvertSourceToTarget(s => s ? ViewStates.Visible : ViewStates.Gone);

            var error = view.FindViewById(Resource.Id.error);
            Bindings
                .Add(viewmodel, () => viewmodel.Error, error, () => error.Visibility)
                .ConvertSourceToTarget(s => s == FeedViewModel.ErrorType.NotError ? ViewStates.Gone : ViewStates.Visible);

            var toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((HomeActivity)Activity).SetSupportActionBar(toolbar);
            toolbar.SetNavigationIcon(Resource.Drawable.ic_menu_white_24dp);

            return view;
        }
    }
}