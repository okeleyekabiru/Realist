using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Plugins;
using Plugins.Youtube;
using Realist.Data.Infrastructure;
using Realist.Data.Model;
using Realist.Data.ViewModels;

namespace Realist.Data.Extensions
{
  public static class RealistExtension
    {
        public static async Task<bool> UploadPost(this PostModel post, string userId, IPhoto photoUploads, IPhotoAccessor photoAccessor, IYoutube youtubeuploader, IVideo videoContext, IPost postContext,Post model)
        {
            model.UserId = userId;
            var postId = Guid.NewGuid();
            model.Id = postId;

            if (post.Photo != null)
            {
                var photoUpload = photoAccessor.AddPhoto(post.Photo);
                var photo = new Photo
                {
                    PostId = postId,
                    PublicId = photoUpload.PublicId,
                    UploadTime = DateTime.Now,
                    Url = photoUpload.Url,
                    UserId = userId
                };


                await photoUploads.UploadImageDb(photo);
            }

            if (post.Video != null)
            {
                var video = new Videos();

                UploadViewModel upload = new UploadViewModel();
                upload.Description = post.Video.Name;
                upload.Type = post.Video.ContentType;
                upload.CategoryId = String.Empty;
                upload.Title = post.Video.FileName;
                upload.VideoTags = new string[] { "tag1", "tag2" };
                upload.Private = false;
                var videoUpload = await youtubeuploader.UploadVideo(upload, post.Video);

                if (!string.IsNullOrEmpty(videoUpload.VideoId))
                {
                    video.DateUploaded = DateTime.Now;
                    video.UserId = userId;
                    video.PublicId = videoUpload.VideoId;
                    video.PostId = model.Id;
                     await videoContext.Post(video);
                }
            }
            await postContext.Post(model);
            return await videoContext.SaveChanges();


        }
        public static async Task<bool> UpdatePost(this PostModel post, string userId, IPhoto photoUploads, IPhotoAccessor photoAccessor, IYoutube youtubeuploader, IVideo videoContext, IPost postContext, Post model)
        {
            model.UserId = userId;
        var postId =  postContext.Update(model);

        PhotoUpLoadResult photoUpload;
        if (post.Photo != null)
        {
            photoUpload = photoAccessor.AddPhoto(post.Photo);
            var storedPhotoPublicId = await photoUploads.FindPhotoId(postId.ToString(), post.ImageId);
            if (photoUpload != null)
            {
                photoAccessor.DeletePhoto(storedPhotoPublicId);
                var photo = new Photo
                {
                    PostId = model.Id,
                    PublicId = photoUpload.PublicId,
                    UploadTime = DateTime.Now,
                    Url = photoUpload.Url,
                    UserId = userId
                };


                await photoUploads.Update(photo);

            }
        }

        if (post.Video != null)
            {
              var storedVideoId =  await videoContext.GetVideoPublicId(postId.ToString(), post.VideoId);
                var video = new Videos();

                UploadViewModel upload = new UploadViewModel();
                upload.Description = post.Video.Name;
                upload.Type = post.Video.ContentType;
                upload.CategoryId = String.Empty;
                upload.Title = post.Video.FileName;
                upload.VideoTags = new string[] { "tag1", "tag2" };
                upload.Private = false;
                var videoUpload = await youtubeuploader.UpdateVideo(storedVideoId, post.Video,upload);
                
                if (!string.IsNullOrEmpty(videoUpload.VideoId))
                {
                    // video
                    video.UserId = userId;
                    video.PublicId = videoUpload.VideoId;
                    video.PostId = model.Id;
                    await videoContext.Update(video);
                }
            }

            return await videoContext.SaveChanges();


        }
    }
}
