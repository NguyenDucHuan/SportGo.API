﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository.SMTPs.Models
{

    public class MailSettings
    {
        public string SystemName { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Host { get; set; } = string.Empty;
    }
}
