﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTags.ApplicationServices.API.Domain
{
    public class ResponseBase<T>
    {
        public T Data { get; set; }
    }
}
