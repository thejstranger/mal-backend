//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace MAL.DAL
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            var context = new MyDbContext();

//            Console.WriteLine("Entity Framework(EF7) Code-First sample");
//            Console.WriteLine();

//            Console.WriteLine("Users");
//            Console.WriteLine("");

//            var query = context.Users.Where(p => p.Username != null).ToList();

//            foreach (var user in query)
//                Console.WriteLine(user.Id.ToString() + user.Username.ToString() + user.Password);

//            Console.ReadKey();

//        }
//    }
//}
