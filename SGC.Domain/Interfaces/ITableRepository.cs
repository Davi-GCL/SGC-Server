﻿using SGC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGC.Domain.Interfaces
{
    public interface ITableRepository
    {
        //public void SetString(string value);
        //public string GetString();
        public IList<Table> GetAllMetaData(string connString);
        public Table GetMetaDataByTableName(string connString,string tableName);
    }
}
