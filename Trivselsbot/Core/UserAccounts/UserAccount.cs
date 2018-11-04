using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trivselsbot.Core.UserAccounts
{
    public class UserAccount
    {
        public ulong ID { get; set; }

        public uint points { get; set; }

        public uint XP { get; set; }

        public uint BirthDay { get; set; }

        public uint BirthMonth { get; set; }

    }
}

