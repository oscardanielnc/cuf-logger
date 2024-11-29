using System;
using cuf_admision_domain.Models;

namespace cuf_admision_domain.Services
{
    public interface IUtilsService
    {
        public bool LogSimple(LogModel body);
    }
}

