﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.DAL.Mongo.Models
{
    public interface IPropertyChanged
    {
        string PropertyName { get; set; }
    }
}
