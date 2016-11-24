using Microsoft.WindowsAzure.MobileServices;
using MovieFinder_Application.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MovieFinder_Application
{
    public class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<WatchList> watchListTable;

        private AzureManager()
        { 
            this.client = new MobileServiceClient("http://movieliztapp.azurewebsites.net");
            this.watchListTable = this.client.GetTable<WatchList>();
        }

        public MobileServiceClient AzureClient
        {
            get { return client; }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }

                return instance;
            }
        }

        public async Task AddWatchList(WatchList watchlist)
        {
            await this.watchListTable.InsertAsync(watchlist);
        }

        public async Task UpdateWatchList(WatchList watchlist)
        {
            await this.watchListTable.UpdateAsync(watchlist);
        }

        public async Task DeleteWatchList(WatchList watchlist)
        {
            await this.watchListTable.DeleteAsync(watchlist);
        }

        public async Task<List<WatchList>> GetWatchList()
        {
            return await this.watchListTable.ToListAsync();
        }

    }
}