using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hangman.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace hangman.Data
{
    public class HangmanContext : DbContext
    {
        public string DbPath { get; }

        public HangmanContext(IWebHostEnvironment env)
        {
            var root = env.ContentRootPath;
            DbPath = root + "/Data/hangman.db";
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HighScore> HighScores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }
}
