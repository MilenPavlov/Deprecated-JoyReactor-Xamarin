﻿using System.Linq;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using JoyReactor.Android.App.Common;
using JoyReactor.Android.App.Home;
using JoyReactor.Android.Widget;
using JoyReactor.Core.ViewModels;

namespace JoyReactor.Android.App.Posts
{
    public class HeaderRow : RecyclerView.ViewHolder, PostFragment.Adapter.PostViewHolder
    {
        readonly PostViewModel viewmodel;

        readonly WebImageView image;
        readonly WebImageView imageBackground;

        readonly ViewGroup thumbnails;
        readonly TextView imageCount;

        public HeaderRow(ViewGroup parent, PostViewModel viewmodel)
            : base(LayoutInflater.FromContext(parent.Context).Inflate(Resource.Layout.layout_post, parent, false))
        {
            this.viewmodel = viewmodel;
            image = ItemView.FindViewById<WebImageView>(Resource.Id.image);
            imageBackground = ItemView.FindViewById<WebImageView>(Resource.Id.imageBackground);

            image.SetCommand(viewmodel.OpenImageCommand);
            thumbnails = ItemView.FindViewById<ViewGroup>(Resource.Id.thumbnails);
            imageCount = ItemView.FindViewById<TextView>(Resource.Id.imageCount);
        }

        public void OnBindViewHolder(int position)
        {
            SetImageSource(image);
            SetImageSource(imageBackground);

            CorrectImagePosition(); 

            for (int i = 0; i < thumbnails.ChildCount; i++)
            {
                var iv = (WebImageView)thumbnails.GetChildAt(i);
                iv.SetImageSource(viewmodel.CommentImages.Skip(i).FirstOrDefault());
                iv.SetCommand(viewmodel.OpenThumbnailCommand, i);
            }

            #if FUTURE
            var notVisibleImageCount = viewmodel.CommentImages.Count - thumbnails.ChildCount;
            #else
            var notVisibleImageCount = 0;
            #endif
            imageCount.Visibility = notVisibleImageCount > 0 ? ViewStates.Visible : ViewStates.Gone;
            imageCount.Text = "+" + notVisibleImageCount;
        }

        void SetImageSource(WebImageView target)
        {
            target.SetImageSource(viewmodel.Image, 200.ToPx(), FeedAdapter.NormalizeAspect(viewmodel.ImageAspect));
        }

        void CorrectImagePosition()
        {
            if (viewmodel.CommentImages.Count > 0)
            {
                if (viewmodel.IsDataFromWeb)
                {
                    imageBackground.Animate().ScaleX(1).ScaleY(1).TranslationX(0);
                    image.Animate().ScaleX(1).ScaleY(1).TranslationX(0);
                }
                else
                {
                    imageBackground.ScaleX = imageBackground.ScaleY = 1;
                    image.ScaleX = image.ScaleY = 1;
                    imageBackground.TranslationX = image.TranslationX = 0;
                }
            }
            else
            {
                var dm = image.Resources.DisplayMetrics;
                var scale = dm.WidthPixels / (dm.WidthPixels - 100 * dm.Density);
                var translate = 50 * dm.Density;

                imageBackground.ScaleX = imageBackground.ScaleY = scale;
                image.ScaleX = image.ScaleY = scale;
                imageBackground.TranslationX = image.TranslationX = translate;
            }
        }
    }
}