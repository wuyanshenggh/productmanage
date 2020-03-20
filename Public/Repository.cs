using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using ProductMange.Model;
using ProductMange.Public;
using System.Data.Common;
using System.Data;
using System.Reflection;

namespace ProductMange
{
    public class Repository<T> where T : Prc_BaseInfo
    {
        private DbContext _context;
        private DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public DbSet<T> GetDBSet()
        {
            return _dbSet;
        }

        public T Get(Expression<Func<T,bool>> whereExpression)
        {
            return _dbSet.FirstOrDefault(whereExpression);
        }

        public List<T> GetAll()
        {
            return _dbSet.ToList();
        }
        public T GetBaseID(Guid ID)
        {
            List<Expression<Func<T, bool>>> whereConditions = new List<Expression<Func<T, bool>>>();
            DbSet<T> dbSet = _context.Set<T>();
            return dbSet.FirstOrDefault(j=>j.ID==ID);
        }
        public List<T> Search(Expression<Func<T,bool>> whereExpression)
        {
            if (whereExpression == null)
            {
                return GetAll();
            }

            return _dbSet.Where(whereExpression).ToList();
        }

        public List<T> SearchPage(PageInfo pageInfo,Expression<Func<T,bool>>[] whereExpressions)
        {
            List<T> result = new List<T>();
            var expression = GetCombinedExpression(whereExpressions);
            result = _dbSet.Where(expression).OrderBy(pageInfo.Sort, pageInfo.IsAscending).Skip(pageInfo.OffSet).Take(pageInfo.PageSize).ToList();
            pageInfo.Total = _dbSet.Count(expression);

            return result;
        }

        public List<E> SearchBySql<E>(string sqlText) where E : new()
        {
            DbConnection conn = _context.Database.GetDbConnection();
            try
            {
                DataTable dt = SearchBySql(sqlText);
                //dt转list
                List<E> ts = new List<E>();
                string tempName = "";
                foreach (DataRow dr in dt.Rows)
                {
                    E t = new E();
                    PropertyInfo[] propertys = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;
                        if (dt.Columns.Contains(tempName))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            if (pi.PropertyType.IsSubclassOf(typeof(Prc_BaseInfo)))
                            {
                                continue;
                            }

                            object value = dr[tempName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                    ts.Add(t);
                }
                return ts;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public DataTable SearchBySql(string sqlText)
        {
            DbConnection conn = _context.Database.GetDbConnection();
            try
            {
                var command = conn.CreateCommand();
                command.CommandText = sqlText;
                conn.Open();
                var reader = command.ExecuteReader();
                var dt = new DataTable();
                dt.Load(reader);
                reader.Close();
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }

        public void Add(T entity)
        {
            if (entity.ID == Guid.Empty)
            {
                entity.ID = Guid.NewGuid();
            }
            entity.CreateTime = DateTime.Now;
            if (LoginUserInfo.CurrUser != null)
            {
                entity.CreateOperateUser = LoginUserInfo.CurrUser.LoginName;
            }
            entity.LastUpdateTime = entity.CreateTime;
            entity.LastOperateUser = entity.CreateOperateUser;
            entity.IsDelete = false;
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            entity.LastUpdateTime = DateTime.Now;
            if (LoginUserInfo.CurrUser != null)
            {
                entity.LastOperateUser = LoginUserInfo.CurrUser.LoginName;
            }
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            entity.LastUpdateTime = DateTime.Now;
            if (LoginUserInfo.CurrUser != null)
            {
                entity.LastOperateUser = LoginUserInfo.CurrUser.LoginName;
            }
            entity.IsDelete = true;
            Update(entity);
        }

        public bool IsExist(Expression<Func<T,bool>> whereExpression)
        {
            bool isExist = false;
            isExist = Get(whereExpression)!=null;           
            return isExist;
        }

        private Expression<Func<T,bool>> GetCombinedExpression(Expression<Func<T,bool>>[] expressions)
        {
            Expression<Func<T,bool>> retExpression;
            if (expressions == null)
            {
                throw new CustomExecption("9999", "查询表达式不能为空");
            }
            if (expressions.Length == 0)
            {
                return x => 1==1;
            }
            if (expressions.Length == 1)
            {
                return expressions[0];
            }
            retExpression = expressions[0];
            for(int i = 1; i < expressions.Length; i++)
            {
                retExpression = retExpression.And(expressions[i]);
            }

            return retExpression;
        }
    }
}
