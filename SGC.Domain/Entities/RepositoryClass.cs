﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Entities
{
    public class RepositoryClass<T>
    {
        public RepositoryClass(T driver) 
        {
            Driver=driver;
        }

        public T Driver { get; }
    }
}
