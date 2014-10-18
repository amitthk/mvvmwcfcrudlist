﻿using MvvmWcfCrudList.Service.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmWcfCrudList.Service.MvvmWcfCrudList.Persistence
{
    interface ITodoPersistence
    {
        Guid Add(Todo todo);
        void Delete(Guid id);
        List<Todo> List();
        bool Update(Todo todo);
        Todo Get(Guid id);
    }
}
