using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        public static async Task<bool> UpdatePost(this PostModel post, string userId, IPhoto photoUploads, IPhotoAccessor photoAccessor, IYoutube youtubeuploader, IVideo videoContext, IPost postContext, Post model,IMapper mapper)
        {
            model.UserId = userId;
            PhotoUpLoadResult photoUpload;
        if (post.Photo != null)
        {
            photoUpload = photoAccessor.AddPhoto(post.Photo);
            var storedPhotoPublicId = await photoUploads.FindPhotoId(post.Id, post.ImageId);
            if (photoUpload != null)
            {
              var delete =  photoAccessor.DeletePhoto(storedPhotoPublicId.PublicId);
                if (delete.ToLower().Equals("ok"))
                {
                    
                    var photo = new PhotoModel
                    {
                       
                        PublicId = photoUpload.PublicId,
                        Url = photoUpload.Url, 
                        UploadTime = DateTime.Now
                        
                    };
                    storedPhotoPublicId.PublicId = photo.PublicId;
                    storedPhotoPublicId.Url = photo.Url;
                    storedPhotoPublicId.UploadTime = photo.UploadTime;
                    
                        await  photoUploads.Update(storedPhotoPublicId);
                    // 

                }
               

            }
        }

        if (post.Video != null)
            {
              var storedVideoId =  await videoContext.GetVideoPublicId(post.Id, post.VideoId);
              

                UploadViewModel upload = new UploadViewModel();
                upload.Description = post.Video.Name;
                upload.Type = post.Video.ContentType;
                upload.CategoryId = String.Empty;
                upload.Title = post.Video.FileName;
                upload.VideoTags = new string[] { "tag1", "tag2" };
                upload.Private = false;
                var videoUpload = await youtubeuploader.UpdateVideo(storedVideoId.PublicId, post.Video,upload);
                
                if (!string.IsNullOrEmpty(videoUpload.VideoId))
                {
                    // video
                    
                    storedVideoId.DateUpdated = DateTime.Now;
                    storedVideoId.PublicId = videoUpload.VideoId;
                    await videoContext.Update(storedVideoId);

                    // mapper.Map(video,storedVideoId);
                }
            }

             
            return await videoContext.SaveChanges();


        }
    }
}
