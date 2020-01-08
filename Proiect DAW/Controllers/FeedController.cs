using System;
using System.Collections.Generic;
using System.Linq;
using Proiect_DAW.Code.Base;
using System.IO;
using Proiect_DAW.Models.FeedModels;
using Domain;
using Proiect_DAW.Models.JsonModels;
using System.Web.Mvc;
using Services.Post;
using Services.Comment;
using Services.Photo;
using Services.Reaction;
using DataAccess;

namespace Proiect_DAW.Controllers
{
    public class FeedController : BaseController
    {
        private readonly Services.Post.PostService postService;
        private readonly Services.Comment.CommentService commentService;
        private readonly Services.Photo.PhotoService photoService;
        private readonly Services.Reaction.ReactionService reactionService;
        public FeedController() : base()
        {
            postService = new PostService(new SocializRUnitOfWork(new SocializRContext()));
            commentService = new CommentService(new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()));
            reactionService = new ReactionService(new SocializRUnitOfWork(new SocializRContext()));
            PageSize = 10;
        }
        [Authorize]
        [HttpDelete]
        public void RemoveComment(int commentId)
        {
            MakeCurrentUser();

            if (commentService.CanDeleteComment(commentId, currentUser))
            {
                commentService.RemoveComment(commentId);
            }
        }
        [Authorize]
        [HttpDelete]
        public bool RemovePost(int postId)
        {
            MakeCurrentUser();
            if (postService.CanDetelePost(postId, currentUser))
            {
                postService.RemovePost(postId);
                return true;
            }
            return false;
        }

        [HttpGet]
        public ActionResult Index()
        {
            MakeCurrentUser();
            ViewBag.CurrentUser = currentUser;
            var postModelAdd = new PostAddModel();
            return View(postModelAdd);
        }


        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetComments(int postId, int toSkip)
        {
            MakeCurrentUser();
            if (postService.CanSeePost(postId, currentUser))
            {
                var comments = commentService.GetComments(toSkip, PageSize, postId).Select(e =>

                   new CommentJsonModel()
                   {
                       Id = e.Id,
                       IsAdmin = currentUser.IsAdmin,
                       ProfilePhoto = e.User.PhotoId,
                       Text = e.Content,
                       UserId = e.UserId,
                       UserName = e.User.Name + " " + e.User.Surname,
                       IsMine = currentUser.Id == e.UserId

                   }
                ).ToList();
                return Json(comments);
            }
            return Json(new List<int>());
        }

        [HttpGet]
        public JsonResult GetPosts(int toSkip)
        {
            MakeCurrentUser();
            if (currentUser.IsAuthenticated)
            {
                var posts = postService.GetNewsfeed(toSkip, PageSize, currentUser).Select(e =>


                     new PostJsonModel()
                     {
                         Id = e.Id,
                         IsAdmin = currentUser.IsAdmin,
                         IsMine = e.UserId == currentUser.Id,
                         Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id),
                         NoReactions = e.Reaction.Count,
                         PhotoId = e.Photo != null ? new List<int>() { e.Photo.Id } : new List<int>(),
                         ProfilePhoto = e.User.PhotoId,
                         Text = e.Text,
                         UserId = e.UserId,
                         UserName = e.User.Name + " " + e.User.Surname
                     }

                ).ToList();
                return Json(posts,JsonRequestBehavior.AllowGet);
            }
            else
            {
                var posts = postService.GetPublicNewsfeed(toSkip, PageSize).Select(e =>

                new PostJsonModel()
                {
                    Id = e.Id,
                    IsAdmin = currentUser.IsAdmin,
                    IsMine = false,
                    Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id),
                    NoReactions = e.Reaction.Count,
                    PhotoId = e.Photo != null ? new List<int>() { e.Photo.Id } : new List<int>(),
                    ProfilePhoto = e.User.PhotoId,
                    Text = e.Text,
                    UserId = e.UserId,
                    UserName = e.User.Name + " " + e.User.Surname
                }
                ).ToList();
                return Json(posts, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetPersonPosts(int toSkip, string userId)
        {
            MakeCurrentUser();
            var posts = postService.GetPersonPost(toSkip, PageSize, userId).Select(e =>
             new PostJsonModel()
             {
                 Id = e.Id,
                 IsAdmin = currentUser.IsAdmin,
                 IsMine = false,
                 Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id),
                 NoReactions = e.Reaction.Count,
                 PhotoId = e.Photo != null ? new List<int>() { e.Photo.Id } : new List<int>(),
                 ProfilePhoto = e.User.PhotoId,
                 Text = e.Text,
                 UserId = e.UserId,
                 UserName = e.User.Name + " " + e.User.Surname
             }

            ).ToList();
            return Json(posts);

        }

        [HttpPost]
        public ActionResult AddPost(PostAddModel post)
        {
            MakeCurrentUser();
            if (ModelState.IsValid)
            {
                var newPost = new Post()
                {
                    Text = post.Text,
                    Confidentiality = post.Visibility

                };
                postService.AddPost(newPost, currentUser);

                if (post.Binar != null)
                {
                    var photo = new Photo()
                    {
                        Position = 1,
                        PostId = newPost.Id,
                        MIMEType = post.Binar.ContentType
                    };

                    using (var memoryStream = new MemoryStream())
                    {
                        post.Binar.InputStream.CopyTo(memoryStream);
                        photo.Binary = memoryStream.ToArray();
                    }
                    photoService.AddPhoto(photo, currentUser);
                }
                return RedirectToAction("Index");
            }

            return View("Index", post);
        }

        [HttpPut]
        public bool Reaction(int postId)
        {
            MakeCurrentUser();
            if (postService.CanSeePost(postId, currentUser)) return reactionService.ChangeReaction(postId, currentUser);
            return false;
        }

        [HttpPost]
        public int Comment(int postId, string comentariu)
        {
            MakeCurrentUser();
            if (postService.CanSeePost(postId, currentUser) && !string.IsNullOrEmpty(comentariu))
            {
                int id = commentService.AddComment(comentariu.Trim(), postId, currentUser);
                return id;

            }
            return -1;
        }
        [HttpGet]
        public ActionResult Privacy()
        {
            MakeCurrentUser();
            return View();
        }

    }
}
