using Back.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Domain.IServices
{
   public interface ILoginService
    {
        Task<Usuario> ValidateUser(Usuario usuario);
    }
}
