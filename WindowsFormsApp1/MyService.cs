using Autofac;
using CRUD.Core;
using CRUD.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class MyService
    {
        static IContainer Container { get; set; }
        static MyService()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<StudentRepository>().As<IStudentRepository>();
            Container = builder.Build();
        }
        public static IStudentRepository Student
        {
            get
            {
                return Container.Resolve<IStudentRepository>();

            }
        }
    }
}
