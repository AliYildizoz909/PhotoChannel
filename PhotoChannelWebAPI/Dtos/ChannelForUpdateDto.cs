﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoChannelWebAPI.Dtos
{
    public class ChannelForUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ChannelPhotoUrl { get; set; }
    }
}