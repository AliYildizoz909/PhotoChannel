﻿using System;
using System.Collections.Generic;
using System.Text;
using Entities.Abstract;

namespace Core.Entities.Concrete
{
    public class OperationClaim : BaseEntity,IEntity
    {
        public string? ClaimName { get; set; }
    }
}
