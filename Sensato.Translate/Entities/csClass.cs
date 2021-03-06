﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Sensato.Translate.Entities
{
    public class csClass
    {
        public string inheritance { get; set; }
        public string name { get; set; }
        public string modifiers { get; set; }
        public string partial { get; set; }
        public List<csLine> lines { get; set; }
        public List<csConstructor> constructors { get; set; }

        public csClass()
        {
            this.lines = new List<csLine>();
            this.constructors = new List<csConstructor>();
        }
    }
}
