using CRUD.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace CRUD.DataAccess
{
    public class StudentRepository : IStudentRepository
    {
        public bool Delete(int studentId)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("delete from Student where StudentID = @StudentID", new { StudentID = studentId }, commandType: CommandType.Text);
                return result != 0 ;

            }
        }

        public List<Student> GetAll()
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                return  db.Query<Student>("select * from Student", commandType: CommandType.Text).ToList();
            }
        }

        public int Insert(Student obj)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@StudentID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    p.AddDynamicParams(new { FullName = obj.FullName, Email = obj.Email, Address = obj.Address, Gender = obj.Gender, Birthday = obj.Birthday, ImageUrl = obj.ImageUrl });
                    db.Execute("sp_Students_Insert", p, commandType: CommandType.StoredProcedure);
                    return p.Get<int>("@StudentID");
                
            }
        }

        public bool Update(Student obj)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnectionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                int result = db.Execute("sp_Students_Update", new { StudentID = obj.StudentID, FullName = obj.FullName, Email = obj.Email, Address = obj.Address, Gender = obj.Gender, Birthday = obj.Birthday, ImageUrl = obj.ImageUrl }, commandType: CommandType.StoredProcedure);
                return result != 0;

            }
        }
    }
}
