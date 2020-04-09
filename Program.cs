using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

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
                    Console.WriteLine("1.) Create blog");
                    Console.WriteLine("2. Add Post to blog");
                    Console.WriteLine("3.) Display all blogs");
                    choice = Console.ReadLine();
                    if (choice == "1")
                    {


                        // Create and save a new Blog
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        var blog = new Blog { Name = name };
                        var db = new BloggingContext();

                        if((db.Blogs.Any(b => b.Name == blog.Name)))
                        {
                            logger.Error("Blog name already exists");
                        }
                        else
                        {
                             db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }

                        
                       
                    }
                    else if (choice == "2")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Post post = new Post();
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
                                logger.Info("Post added - {Title}", post.Title);
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

                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }

                    }


                }
                while (choice == "1" || choice == "2" || choice == "3");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
            }
        }
    }


