﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO;

namespace Ecom.Core.Services
{
    public interface IEmailService
    {
        Task SendEmail(EmailDTO emailDTO);
    }
}
