using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Trivselsbot.Core.UserAccounts
{
    public static class UserAccounts
    {
        private static List<UserAccount> accounts;

        private static string accountsfile = "Resources/accounts.json";

        static UserAccounts()
        {
            if (Datastorage.SaveExists(accountsfile))
            {
                accounts = Datastorage.LoadUserAccounts(accountsfile).ToList();
            }
            else
            {
                accounts = new List<UserAccount>();
                SaveAccounts();
            }
        }

        public static UserAccount GetAccount(SocketUser user)
        {
            return GetOrCreateAccount(user.Id);
        }

        public static void SaveAccounts()
        {
            Datastorage.SaveUserAccounts(accounts, accountsfile);
        }

        private static UserAccount GetOrCreateAccount(ulong id)
        {
            var result = from a in accounts
                where a.ID == id
                select a;
            var account = result.FirstOrDefault();
            if (account == null)
            {
                account = CreateAccount(id);
            }

            return account;
        }

        private static UserAccount CreateAccount(ulong id)
        {
            var newAccount = new UserAccount
            {
                BirthDay = 0,
                BirthMonth = 0,
                ID = id,
                points = 0,
                XP = 0
            };

            accounts.Add(newAccount);
            SaveAccounts();
            return newAccount;
        }

        public static void setBirthDay(SocketUser user, uint day)
        {
            var account = GetAccount(user);
            account.BirthDay = day;
            SaveAccounts();
        }

        public static void setBirthMonth(SocketUser user, uint month)
        {
            var account = GetAccount(user);
            account.BirthMonth = month;
            SaveAccounts();
        }
    }
}
