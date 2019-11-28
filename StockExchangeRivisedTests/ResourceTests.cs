using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockExchangeRivised;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockExchangeRivised.Tests
{
    [TestClass()]
    public class ResourceTests
    {

        Instances ic;
        [TestInitialize]
        public void Setup()
        {
            ic = new Instances();
            ic.companyList.Add(new Company(ic, "bread1", 3, 10, 0.01, 5, 1, 1, "wheat-bread"));
            ic.companyList.Add(new Company(ic, "bread2", 3, 10, 0.01, 5, 1, 1, "wheat-bread"));
            ic.companyList.Add(new Company(ic, "wheat1", 3, 10, 0.01, 5, 1, 1, "-wheat"));
            ic.resourceList.Add(new Resource(ic, "bread", "base", 0.6, 0.3));
            ic.resourceList.Add(new Resource(ic, "wheat", "natural", 0.3, 0.3));

            List<ResourceAmount> t1 = new List<ResourceAmount>();
            t1.Add(new ResourceAmount("wheat",1));
            List<ResourceAmount> t2 = new List<ResourceAmount>();
            t2.Add(new ResourceAmount("bread", 1));
            ic.productionRecipeList.Add(new ProductionRecipe("wheat-bread",3,t1,t2));
            ic.productionRecipeList.Add(new ProductionRecipe("wheat-bread", 3, new List<ResourceAmount>(), t2));
            

            ic.population = new Population(ic, 1000, 1000, 0.1);
            ic.populationDemandList.Add(new ResourceAmount("bread",0.1));
        }

        [TestMethod()]
        public void ResourceDemandTest()
        {
			ic.resourceList[0].sales.Add(new ResourceSale(10, 1, "bread1"));
			ic.resourceList[0].sales[0].soldLastTick = 10;
			Assert.AreEqual( 0.5, ic.FindResource("bread").ResourceDemand());
        }
        [TestMethod()]
        public void PopulationBuyTest()
        {
			ic.resourceList[0].sales.Add(new ResourceSale(10, 1, "bread1"));
			ic.resourceList[0].sales[0].soldLastTick = 10;
			ic.resourceList[0].sales.Add(new ResourceSale(50, 2, "bread2"));
			ic.resourceList[1].sales.Add(new ResourceSale(10, 1, "wheat1"));

			ic.population.PopulationBuy();
            Assert.AreEqual(ic.resourceList[0].sales[0].soldThisTick, 10);
            Assert.AreEqual(ic.resourceList[0].sales[0].amount, 0);
            ic.companyList[0].SoldResources();
            Assert.AreEqual(ic.companyList[0].revenue, 10);
            Assert.AreEqual(ic.resourceList[0].sales[1].soldThisTick, 50);
            Assert.AreEqual(ic.resourceList[0].sales[1].amount, 0);
            ic.companyList[1].SoldResources();
            Assert.AreEqual(ic.companyList[1].revenue, 100);
        }
    }
}