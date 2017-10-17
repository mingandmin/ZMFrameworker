using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogAppDAL.Entities1;
using BlogAppDAL.Repository;

namespace BlogAppDAL.UoW
{
    public class UnitOfWork : IDisposable
    {
        private readonly BlogAppContext _context = null;
        private Repository<Blog> _blogRepository = null;
        private Repository<Category> _categoryRepository;
        private Repository<Comment> _commentRepository = null;
        private Repository<Role> _roleRepository = null;
        private Repository<User> _userRepository = null;
        private Repository<PSS_ExWarehouseMode> _pSS_ExWarehouseMode = null;

        private Repository<COM_User> _cOM_UserRepository = null;
        public Repository<COM_User> COM_UserRepository
        {
            get { return _cOM_UserRepository ?? (_cOM_UserRepository = new Repository<COM_User>(_context)); }
        }

        public Repository<PSS_ExWarehouseMode> PSS_ExWarehouseMode
        {
            get { return _pSS_ExWarehouseMode ?? (_pSS_ExWarehouseMode = new Repository<PSS_ExWarehouseMode>(_context)); }
        }
        public UnitOfWork()
        {
            _context = new BlogAppContext();
        }

        public Repository<Blog> BlogRepository
        {
            get { return _blogRepository ?? (_blogRepository = new Repository<Blog>(_context)); }
        }

        public Repository<Category> CategoryRepository
        {
            get { return _categoryRepository ?? (_categoryRepository = new Repository<Category>(_context)); }
        }

        public Repository<Comment> CommentRepository
        {
            get { return _commentRepository ?? (_commentRepository = new Repository<Comment>(_context)); }
        }

        public Repository<Role> RoleRepository
        {
            get { return _roleRepository ?? (_roleRepository = new Repository<Role>(_context)); }
        }

        public Repository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new Repository<User>(_context)); }
        }

        


        public void SaveChanges()
        {
            _context.SaveChanges();
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
