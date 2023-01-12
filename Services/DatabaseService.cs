using OpenRSS_1.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OpenRSS_1.Services
{
    public static class DatabaseService
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;

            // Get the current working directory
            string appDirectory = Environment.CurrentDirectory;

            // Combine the app directory with the desired path to the database file
            string databasePath = Path.Combine(appDirectory, "database.db");

            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Subscriptions>();

        }

        public static async Task DropDataTables()
        {
            if (db != null)
            {
                await db.DeleteAllAsync<Subscriptions>();
            }
        }

        public static async Task AddNewSubscription(Subscriptions subscriptions)
        {
            await Init();
            await db.InsertAsync(subscriptions);
        }

        public static async Task<List<Subscriptions>> GetActiveFeeds()
        {
            await Init();

            List<Subscriptions> Subscriptions = await db.QueryAsync<Subscriptions>("select * from Subscriptions Where Active = TRUE");

            Debug.WriteLine(Subscriptions.Count);
            return Subscriptions;
        }

        public static async Task<Subscriptions> getSubscriptionInfoAsync(string rowId)
        {
            await Init();           
            Subscriptions sub = await db.GetAsync<Subscriptions>(rowId);
            return sub;
        }


    }
}
