﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossNull.Logic.Services
{
    class SMSService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            return Task.CompletedTask;
        }
    }
}
