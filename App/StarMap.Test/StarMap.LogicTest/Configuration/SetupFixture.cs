using Microsoft.Practices.Unity;
using NUnit.Framework;
using StarMap.LogicTest.Classes;
using System;

namespace StarMap.Common.Test
{
  [SetUpFixture]
  public class SetupFixture
  {
    public IUnityContainer Container { get; private set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
      Container = new UnityContainer();
      UnityBootstrapper.RegisterTypes(Container);
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
      // TODO: Add code here that is run after
      //  all tests in the assembly have been run
    }
  }
}