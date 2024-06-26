﻿using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class Subscriber : BaseEntity,IEntity
    {
        public int? UserId { get; set; }
        public int? ChannelId { get; set; }

        public virtual User User { get; set; }
        public virtual Channel Channel { get; set; }
    }
}
