﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.DAL.Mongo.Models
{
    public class SimpleValueChanged : IPropertyChanged
    {
        public object Previous { get; set; }

        public object New { get; set; }

        public string PropertyName { get; set; }
    }
}
