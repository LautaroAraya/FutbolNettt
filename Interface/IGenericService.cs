using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FutbolNet.Interface
{
    public interface IGenericService<T> where T : class
    {
        public Task<List<T>?> GetAllAsync();
        public Task<T?> GetByIdAsync(int id);
        public Task<T?> AddAsync(T? entity);
        public Task UpdateAsync(T? entity);
        public Task DeleteAsync(int id);
    }
}
