using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                String choice;

                do
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("1.) Create blog");
                    Console.WriteLine("2.) Add Post to blog");
                    Console.WriteLine("3.) Display all blogs");
                    Console.WriteLine("4.) Display Posts");
                    Console.WriteLine("5.) Edit Post");
                    Console.WriteLine("6.) Delete Post");
                    Console.WriteLine("7.) Edit Blog");
                    Console.WriteLine("8.) Delete Blog");


                    choice = Console.ReadLine();
                    if (choice == "1")
                    {


                        // Create and save a new Blog
                        Console.WriteLine("\n");
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        var blog = new Blog { Name = name };
                        var db = new BloggingContext();

                        if ((db.Blogs.Any(b => b.Name == blog.Name)))
                        {
                            logger.Error("Blog name already exists");
                        }
                        else
                        {
                            db.AddBlog(blog);
                            db.SaveChanges();
                            logger.Info("Blog added - {name}", name);
                        }



                    }

                      else if (choice == "2")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Post post = new Post();
                        Console.WriteLine("\n");
                        Console.WriteLine("Select the blog you would like to post to:");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            if (db.Blogs.Any(b => b.BlogId == BlogId))
                            {
                                post.BlogId = BlogId;
                                Console.WriteLine("Enter post title: ");
                                post.Title = Console.ReadLine();
                                Console.WriteLine("Enter post content: ");
                                post.Content = Console.ReadLine();
                                db.AddPost(post);
                                db.SaveChanges();
                                logger.Info("Post added - {Title}", post.Title);

                                Console.WriteLine("Here are the posts:");
                                foreach (var item in db.Posts)
                                {
                                    Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                                }

                            }
                            else
                            {
                                logger.Error("There are no blogs saved with that id");
                            }
                        }

                        else
                        {
                            logger.Error("Invalid blog id");
                        }
                    }

                    else if (choice == "3")
                    {

                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine("\n");

                        // Show all blogs
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }




                    }
                    else if (choice == "4")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine("Select the blog's posts to display:");
                        Console.WriteLine("0) posts from all blogs");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}) Posts from {item.Name}");
                        }

                        if (int.TryParse(Console.ReadLine(), out int BlogId))
                        {
                            IEnumerable<Post> Posts;
                            if (BlogId != 0 && db.Blogs.Count(b => b.BlogId == BlogId) == 0)
                            {
                                logger.Error("There are no Blogs saved with that Id");
                            }
                            else
                            {
                                Posts = db.Posts.OrderBy(p => p.Title);

                                if (BlogId == 0)
                                {
                                    Posts = db.Posts.OrderBy(p => p.Title);
                                }
                                else
                                {
                                    Posts = db.Posts.Where(p => p.BlogId == BlogId).OrderBy(p => p.Title);
                                }
                                Console.WriteLine($"{Posts.Count()} post(s) returned:");
                                foreach (var item in Posts)
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                                }
                            }

                        }
                        else
                        {
                            logger.Error("Invalid Blog Id");
                        }


                    }

                    else if (choice == "5")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("Enter a post name");
                        var postChoice = Console.ReadLine();
                        var post = db.Posts.FirstOrDefault(p => p.Title == postChoice);

                        Console.WriteLine("Enter new post content");
                        var newContent = Console.ReadLine();

                        post.Content = newContent;
                        db.SaveChanges();


                        Console.WriteLine("Here are the posts:");
                        foreach (var item in db.Posts)
                        {
                            Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                        }


                    }



                    else if (choice == "6")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.Write("Enter a Post name: ");
                        var postToDelete = Console.ReadLine();
                        var deletePost = db.Posts.FirstOrDefault(p => p.Title == postToDelete);

                        db.Posts.Remove(deletePost);
                        db.SaveChanges();


                        Console.WriteLine("Here are the posts:");
                        foreach (var item in db.Posts)
                        {
                            Console.WriteLine($"title: {item.Title}; content: {item.Content}");
                        }


                    }

                    else if (choice == "7")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.WriteLine("Enter a blog name");
                        var blogChoice = Console.ReadLine();
                        var blog = db.Blogs.FirstOrDefault(b => b.Name == blogChoice);

                        Console.WriteLine("Enter new blog title");
                        var newBlogTitle = Console.ReadLine();

                        blog.Name = newBlogTitle;
                        db.SaveChanges();


                        Console.WriteLine("Here are the blogs");
                        foreach (var item in db.Blogs)
                        {
                            Console.WriteLine(item.Name);
                        }
                    }


                    else if(choice == "8")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);

                        Console.Write("Enter a blog name: ");
                        var blogToDelete = Console.ReadLine();
                        var deleteBlog = db.Blogs.FirstOrDefault(b => b.Name == blogToDelete);
                        

                        db.Blogs.Remove(deleteBlog);
   
                        db.SaveChanges();


                        Console.WriteLine("Here are the blogs");
                        foreach (var item in db.Blogs)
                        {
                            Console.WriteLine(item.Name);
                        }

                    }



                }
                while (choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5" || choice =="6" || choice == "7" || choice == "8");


            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
            }
        }
    }


