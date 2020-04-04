using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Plugins.Youtube
{
    public class Youtube : IYoutube
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        public static string VideoId { get; set; }

        public Youtube(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [STAThread]
        public async Task<UploadVideoResult> UploadVideo(UploadViewModel uploadViewModel, IFormFile videos)
        {
            CreatePlayList().Wait();
            var filePath = "";
            UserCredential credential;
            using (var stream =
                new FileStream(_env.WebRootPath + "/client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] {YouTubeService.Scope.YoutubeUpload},
                    "user",
                    CancellationToken.None
                );
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });

                var video = new Video();
                video.Snippet = new VideoSnippet();
                video.Snippet.Title = uploadViewModel.Title;
                video.Snippet.Description = uploadViewModel.Description;
                video.Snippet.Tags = uploadViewModel.VideoTags;
                video.Snippet.CategoryId =
                    uploadViewModel
                        .CategoryId; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                video.Status = new VideoStatus();
                video.Status.PrivacyStatus = "public"; // or "private" or "public"
                filePath = CopyToFolder(videos); // Replace with path to actual movie file.

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var videosInsertRequest =
                        youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                    videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                    videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                    await videosInsertRequest.UploadAsync();
                }

            }

            DeleteFromFolder(filePath);
            return new UploadVideoResult
            {
                VideoId = VideoId
            };
        }

        public async Task<List<UploadVideoResult>> RetreiveVideo(string videoId)
        {
            List<UploadVideoResult> uploadVideoResult = new List<UploadVideoResult>();
            UploadVideoResult retrieve = new UploadVideoResult();
            UserCredential credential;
            using (var stream =
                new FileStream(_env.WebRootPath + "/client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] {YouTubeService.Scope.YoutubeReadonly},
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            var channelsListRequest = youtubeService.Channels.List("contentDetails");
            channelsListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var channelsListResponse = await channelsListRequest.ExecuteAsync();

            foreach (var channel in channelsListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var uploadsListId = channel.ContentDetails.RelatedPlaylists.Uploads;
                retrieve.VideoId = channel.Id;
                uploadVideoResult.Add(retrieve);
                Console.WriteLine("Videos in list {0}", uploadsListId);

                var nextPageToken = "";

                while (nextPageToken != null)
                {
                    var playlistItemsListRequest = youtubeService.PlaylistItems.List("snippet");
                    playlistItemsListRequest.PlaylistId = uploadsListId;
                    playlistItemsListRequest.MaxResults = 50;
                    playlistItemsListRequest.PageToken = nextPageToken;

                    // Retrieve the list of videos uploaded to the authenticated user's channel.
                    var playlistItemsListResponse = await playlistItemsListRequest.ExecuteAsync();

                    foreach (var playlistItem in playlistItemsListResponse.Items)
                    {
                        // Print information about each video.
                        Console.WriteLine("{0} ({1})", playlistItem.Snippet.Title,
                            playlistItem.Snippet.ResourceId.VideoId);
                    }

                    nextPageToken = playlistItemsListResponse.NextPageToken;
                }
            }

            return uploadVideoResult;
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }



        void videosInsertRequest_ResponseReceived(Video video)
        {
            VideoId = video.Id;
            Console.WriteLine("Video id '{0}' was successfully uploaded.", video.Id);
        }

        public UploadVideoResult DeleteVideo(string id)
        {
            throw new NotImplementedException();

        }

        public string CopyToFolder(IFormFile video)
        {
            var filePath = _env.WebRootPath + "/youtube";
            ;
            if (!Directory.Exists(filePath))
            {
                var directory = Directory.CreateDirectory(filePath);
                directory.Create();
            }

            string path = Path.Combine(filePath, video.FileName);
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                video.CopyTo(outputFileStream);
            }

            return path;
        }

        public void DeleteFromFolder(string path)
        {
            if (File.Exists(path))
            {
                // If file found, delete it    
                File.Delete(path);
            }
        }

        public async Task CreatePlayList()
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for full read/write access to the
                    // authenticated user's account.
                    new[] { YouTubeService.Scope.Youtube },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = "Test Playlist";
            newPlaylist.Snippet.Description = "A playlist created with the YouTube API v3";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";
            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();

            // Add a video to the newly created playlist.
            var newPlaylistItem = new PlaylistItem();
            newPlaylistItem.Snippet = new PlaylistItemSnippet();
            newPlaylistItem.Snippet.PlaylistId = newPlaylist.Id;
            newPlaylistItem.Snippet.ResourceId = new ResourceId();
            newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
            newPlaylistItem.Snippet.ResourceId.VideoId = "GNRMeaz6QRI";
            newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();

            Console.WriteLine("Playlist item id {0} was added to playlist id {1}.", newPlaylistItem.Id, newPlaylist.Id);
        }
    }
    }


        


