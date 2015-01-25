using MyCode;
using MyCode.ObjectSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProject.Entity;

namespace TestProject
{
	class Program
	{
		static void Main(string[] args)
		{
			var entity = new TestEntity1();
			entity.TestString = "ass";
			var entityTo = ObjectMapper.Map(entity, new TestEntity2());

			string entiryForString = ObjectSerializer.Serialize(entityTo);

			var entityToFromString = ObjectSerializer.Deserialize<TestEntity2>(entiryForString);

			var rules = TimeZoneInfo.Local.GetAdjustmentRules().FirstOrDefault();

			Console.WriteLine("The TestString of ToEntity is {0}", entityToFromString.TestString);
			Console.ReadLine();
		}
	}
}
