﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class ChannelAdminForDeleteDto
    {
        public int UserId { get; set; }
        public int ChannelId { get; set; }
    }
}
