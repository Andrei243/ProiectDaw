using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Proiect_DAW.Models;
using AutoMapper;
using Services;
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
        private readonly CurrentUser currentUser;
        private readonly Services.Photo.PhotoService photoService;
        private readonly Services.Reaction.ReactionService reactionService;
        public FeedController() : base(mapper)
        {
            postService = new PostService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            commentService = new CommentService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            photoService = new PhotoService(new SocializRUnitOfWork(new SocializRContext()), currentUser);
            reactionService = new ReactionService(currentUser, new SocializRUnitOfWork(new SocializRContext()));
            PageSize = 10;
        }
        [Authorize]
        [HttpDelete]
        public void RemoveComment(int commentId)
        {
            

            if (commentService.CanDeleteComment(commentId))
            {
                commentService.RemoveComment(commentId);
            }
        }
        [Authorize]
        [HttpDelete]
        public bool RemovePost(int postId)
        {
            if (postService.CanDetelePost(postId))
            { 
                postService.RemovePost(postId);
                return true;
            }
            return false;
        }
        
        [HttpGet]
        public ActionResult Index()
        {
            var postModelAdd = new PostAddModel();
            return View(postModelAdd);
        }

       
        [AllowAnonymous]
        [HttpGet]
        public JsonResult GetComments(int postId,int toSkip)
        {
            if (postService.CanSeePost(postId))
            {
                var comments = commentService.GetComments(toSkip, PageSize, postId).Select(e =>
                {
                    var comment = mapper.Map<CommentJsonModel>(e);
                    comment.IsMine = (currentUser.Id == comment.UserId);
                    comment.IsAdmin = currentUser.IsAdmin;
                    return comment;
                }).ToList();
                return Json(comments);
            }
            return Json(new List<int>());
        }

        [HttpGet]
        public JsonResult GetPosts(int toSkip)
        {
            
            if (currentUser.IsAuthenticated)
            {
                var posts = postService.GetNewsfeed(toSkip, PageSize).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = e.UserId == currentUser.Id;
                    post.IsAdmin = currentUser.IsAdmin;
                    return post;

                }).ToList();
                return Json(posts);
            }
            else
            {
                var posts = postService.GetPublicNewsfeed(toSkip, PageSize).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = false;
                    return post;

                }).ToList();
                return Json(posts);
            }
        }
        [HttpGet]
        public JsonResult GetPersonPosts(int toSkip,string userId)
        {

                var posts = postService.GetPersonPost(toSkip, PageSize,userId).Select(e =>
                {
                    var post = mapper.Map<PostJsonModel>(e);
                    post.Liked = e.Reaction.Select(f => f.UserId).Contains(currentUser.Id);
                    post.IsMine = e.UserId == currentUser.Id;
                    return post;
                }).ToList();
                return Json(posts);
            
        }

        [HttpPost]
        public ActionResult AddPost(PostAddModel post)
        {
            if (ModelState.IsValid)
            {
                
                var newPost = mapper.Map<Post>(post);
                postService.AddPost(newPost);

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
                        post.Binar.CopyTo(memoryStream);
                        photo.Binary = memoryStream.ToArray();
                    }
                    photoService.AddPhoto(photo);
                }
                return RedirectToAction("Index");
            }
            
            return View("Index", post);
        }

        [HttpPut]
        public bool Reaction(int postId)
        {
            if(postService.CanSeePost(postId)) return reactionService.ChangeReaction(postId);
            return false;
        }

        [HttpPost]
        public int Comment(int postId,string comentariu)
        {
            if (postService.CanSeePost(postId)&& !string.IsNullOrEmpty(comentariu))
            {
                int id = commentService.AddComment(comentariu.Trim(), postId);
                return id;

            }
            return -1;
        }
        [HttpGet]
        public ActionResult Privacy()
        {
            return View();
        }

        
        //public ActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
