﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ItemAPI
{
    public class CustomDungeonFlagsComparer : IEqualityComparer<CustomDungeonFlags>
    {
        public bool Equals(CustomDungeonFlags x, CustomDungeonFlags y)
        {
            return x == y;
        }

        public int GetHashCode(CustomDungeonFlags obj)
        {
            return (int)obj;
        }
    }
}
