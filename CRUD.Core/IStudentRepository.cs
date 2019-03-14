using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Core
{
    public interface IStudentRepository
    {
        List<Student> GetAll();

        int Insert(Student obj);

        bool Update(Student obj);

        bool Delete(int studentId);
    }
}
