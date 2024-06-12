using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFormsApp.Service.Abstract
{
    public interface ISessionService
    {
        void SetSessionValue(string key, string value);
        string GetSessionValue(string key);
        bool Validate();
    }
}
