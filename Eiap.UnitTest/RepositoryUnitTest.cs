using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Eiap.NetFramework;

namespace Eiap.UnitTest
{
    [TestClass]
    public class RepositoryUnitTest
    {
        [TestMethod]
        public void RepositoryAddTest()
        {
            using (IRepository<School, int> testschool = (IRepository<School, int>)DependencyManager.Instance.Resolver(typeof(IRepository<School, int>)))
            {
                for (int i = 0; i < 5; i++)
                {
                    School school = testschool.Add(new School { Name = "School" + i.ToString() });
                    using (IRepository<Class, int> testclass = (IRepository<Class, int>)DependencyManager.Instance.Resolver(typeof(IRepository<Class, int>)))
                    {
                        for (int j = 0; j < 5; j++)
                        {
                            Class cla = testclass.Add(new Class { Name = "Class" + j.ToString(), SchoolId = school.Id });
                        }
                    }
                }
            }
        }
    }
}
