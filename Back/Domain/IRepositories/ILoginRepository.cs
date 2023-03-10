using Back.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back.Domain.IRepositories
{
  public interface ILoginRepository
    {
        Task<Usuario> ValidateUser(Usuario usuario);
    }
}
